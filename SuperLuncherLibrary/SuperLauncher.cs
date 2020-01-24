using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Threading;
using SuperLauncher.Data;

namespace SuperLauncher
{
    /// <summary>
    /// Main Super Launcher class
    /// Creates all others helper classes for GUI framwork (WPF) to render
    /// </summary>
    public class SuperLauncher
    {
        public event Action ViewUpdateReqested;
        public event Action ApplicationStarted;
        public event Action ApplicationClosed;

        public bool IsApplicationsListEmpty => ApplicationsData.Count == 0;
        public bool IsSessionsListEmpty => SessionsData.Count == 0;

        public ApplicationRuntimeData CurrentApplicationData { get; private set; }
        public ObservableCollection<ApplicationRuntimeData> ApplicationsData { get; private set; }
        public ObservableCollection<SessionRuntimeData> SessionsData { get; private set; }

        public Dictionary<int, bool> CurrentApplicationAchievements;
        public Dictionary<int, IAchievementChecker> AchievementsCheckers;

        private LauncherDatabase _launcherDatabase;
        private ApplicationProcess _currentProcess;

        private Dispatcher _dispatcher;

        public SuperLauncher(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _launcherDatabase = new LauncherDatabase();

            AchievementsCheckers = new Dictionary<int, IAchievementChecker>
            {
                { 1, new FirstAchievementChecker() },
                { 2, new SecondAchievementChecker() },
                { 3, new ThirdAchievementChecker() },
                { 4, new FourAchievementChecker() },
            };

            CurrentApplicationAchievements = new Dictionary<int, bool>
            {
                { 1, false },
                { 2, false },
                { 3, false },
                { 4, false },
            };

            SessionsData = new ObservableCollection<SessionRuntimeData>();
            CurrentApplicationData = new ApplicationRuntimeData();
            ApplicationsData = new ObservableCollection<ApplicationRuntimeData>();
            foreach (var applicationData in _launcherDatabase.ApplicationsData)
            {
                var runtimeData = new ApplicationRuntimeData(applicationData);

                ApplicationsData.Add(runtimeData);
                SelectApplication(runtimeData.AppGUID);
            }

            if (ApplicationsData.Count > 0)
            {
                SelectApplication(ApplicationsData[0].AppGUID);
            }
        }

        public void AddNewApplication(string applicationPath)
        {
            var appData = new ApplicationRuntimeData
            {
                AppGUID = Guid.NewGuid(),
                AppName = Path.GetFileNameWithoutExtension(applicationPath),
                AppExecutablePath = applicationPath,
            };

            using (var icon = Icon.ExtractAssociatedIcon(applicationPath))
            {
                using (var bitmap = icon.ToBitmap())
                {
                    var iconPath = Path.Combine(LauncherHelper.GetAppIconsPath(), appData.AppGUID + ".png");
                    appData.AppIconPath = iconPath;
                    using (var stream = new StreamWriter(iconPath))
                    {
                        bitmap.Save(stream.BaseStream, System.Drawing.Imaging.ImageFormat.Png);
                        stream.Close();
                    }
                }
            }

            ApplicationsData.Add(appData);
            SelectApplication(appData.AppGUID);

            _launcherDatabase.UpdateApplicationData(appData);
        }

        public void AddNewSession(Guid applicationGuid, (DateTime startTime, DateTime endTime) times)
        {
            var sessionData = new SessionRuntimeData
            {
                SessionGUID = Guid.NewGuid(),
                AppGUID = applicationGuid,
                StartSessionDate = times.startTime,
                EndSessionDate = times.endTime,
                SessionID = SessionsData.Count + 1
            };

            CurrentApplicationData.LastSession = times.endTime;

            SessionsData.Add(sessionData);
            _launcherDatabase.UpdateSessionData(sessionData);

            foreach (var achievementsChecker in AchievementsCheckers)
            {
                CurrentApplicationAchievements[achievementsChecker.Value.AchievementID] = AchievementsCheckers[achievementsChecker.Key]
                    .ValidateAchievement(CurrentApplicationData, SessionsData.ToList());
            }
            ViewUpdateReqested?.Invoke();
        }

        public void SelectApplication(Guid applicationGuid)
        {
            ApplicationRuntimeData applicationData = null;
            foreach (var appData in ApplicationsData)
            {
                if(appData.AppGUID == applicationGuid)
                {
                    applicationData = appData;
                    break;
                }
            }

            if (applicationData == null)
                return;

            if (CurrentApplicationData != null)
                CurrentApplicationData.Selected = false;

            CurrentApplicationData = applicationData;
            CurrentApplicationData.Selected = true;

            var sessions = _launcherDatabase.GetApplicationSessions(CurrentApplicationData.AppGUID)
                .OrderBy(x => x.StartSessionDate)
                .ToList();

            SessionsData.Clear();

            int index = 1;
            foreach (var session in sessions)
            {
                var runtimeData = new SessionRuntimeData(session)
                {
                    SessionID = index
                };

                SessionsData.Add(runtimeData);
                index++;
            }

            foreach (var achievementsChecker in AchievementsCheckers)
            {
                CurrentApplicationAchievements[achievementsChecker.Value.AchievementID] = AchievementsCheckers[achievementsChecker.Key]
                    .ValidateAchievement(CurrentApplicationData, SessionsData.ToList());
            }

            if (SessionsData.Count > 0)
                CurrentApplicationData.LastSession = SessionsData[0].EndSessionDate;
        }

        public void DeleteCurrentApp()
        {
            _launcherDatabase.DeleteApplicationData(CurrentApplicationData.AppGUID);
            ApplicationsData.Remove(CurrentApplicationData);

            if(ApplicationsData.Count > 0)
            {
                SelectApplication(ApplicationsData[0].AppGUID);
            }
            else
            {
                CurrentApplicationData = new ApplicationRuntimeData();
            }
        }

        public void UpdateCurrentApplication(string newAppName, string newAppPath, string newAppIconPath)
        {
            CurrentApplicationData.AppName = newAppName;
            CurrentApplicationData.AppIconPath = newAppIconPath;
            CurrentApplicationData.AppExecutablePath = newAppPath;

            _launcherDatabase.UpdateApplicationData(CurrentApplicationData);
        }

        public void StartCurrentApplication()
        {
            if(_currentProcess != null)
            {
                _currentProcess.Stop();
            }

            _currentProcess = new ApplicationProcess(CurrentApplicationData.AppGUID, 
                CurrentApplicationData.AppExecutablePath, ProcessSessionFeedback);

            ApplicationStarted?.Invoke();
        }

        public void StopCurrentApplication()
        {
            if (_currentProcess != null)
            {
                _currentProcess.Stop();
                _currentProcess = null;
            }
        }

        private void ProcessSessionFeedback(Guid applicationGuid, (DateTime processStart, DateTime processEnd) times)
        {
            _dispatcher.Invoke(() => 
            {
                AddNewSession(applicationGuid, times);
                ApplicationClosed?.Invoke();
            });
        }
    }
}
