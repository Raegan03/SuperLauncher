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
    /// Main Library class
    /// Contains referances to most important parts of Launcher
    /// Manage all runtime data and processes
    /// Only class directly connected with WPF which used it for getting and modifing data
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

        /// <summary>
        /// Creates database which loads data from saved files, prepares session and achievements
        /// Tries to select current application if there is any loaded
        /// </summary>
        /// <param name="dispatcher">Allows to start WPF connected code from different Thread on UI Thread</param>
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

        /// <summary>
        /// Based on executable application path it creates application data and saves icon for application
        /// Application is also automaticaly selected as current
        /// </summary>
        /// <param name="applicationPath">Path to application user wants to add</param>
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

        /// <summary>
        /// Adds new session under current application
        /// Called on process exit callback
        /// Revalidate achievements as they are all depended on sessions
        /// </summary>
        /// <param name="applicationGuid">Current application guid</param>
        /// <param name="times">Start and End process time</param>
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

        /// <summary>
        /// Selects application as current
        /// Reloads sessions data and revalidates achievements
        /// </summary>
        /// <param name="applicationGuid">Guid of application user wants to select</param>
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

        /// <summary>
        /// Delets current application and tries to select new
        /// If there is no other application we aren't selecting any
        /// </summary>
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


        /// <summary>
        /// Updates current application with new data passed by user
        /// </summary>
        /// <param name="newAppName">New name for application</param>
        /// <param name="newAppPath">New path to application executable</param>
        /// <param name="newAppIconPath">New path to icon associated with application</param>
        public void UpdateCurrentApplication(string newAppName, string newAppPath, string newAppIconPath)
        {
            CurrentApplicationData.AppName = newAppName;
            CurrentApplicationData.AppIconPath = newAppIconPath;
            CurrentApplicationData.AppExecutablePath = newAppPath;

            _launcherDatabase.UpdateApplicationData(CurrentApplicationData);
        }

        /// <summary>
        /// Starts currently selected application
        /// </summary>
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

        /// <summary>
        /// Stops application process if it's exists
        /// </summary>
        public void StopCurrentApplication()
        {
            if (_currentProcess != null)
            {
                _currentProcess.Stop();
                _currentProcess = null;
            }
        }

        /// <summary>
        /// Application exited process callback
        /// Fired when process is closed
        /// </summary>
        /// <param name="applicationGuid">Close application guid</param>
        /// <param name="times">Start and End time of process</param>
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
