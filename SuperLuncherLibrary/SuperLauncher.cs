using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using SuperLauncher.Data;

namespace SuperLauncher
{
    /// <summary>
    /// Main Super Launcher class
    /// Creates all others helper classes for GUI framwork (WPF) to render
    /// </summary>
    public class SuperLauncher
    {
        public string CurrentAppName => CurrentApplicationData.AppName;

        public ObservableCollection<SuperLauncherSessionData> SuperLauncherSessionDatas =>
            _sessionManager.SuperLauncherSessionDatas;

        private SuperLauncherSessionsManager _sessionManager;

        public bool IsApplicationsListEmpty => ApplicationsData.Count == 0;

        public ApplicationRuntimeData CurrentApplicationData { get; private set; }
        public ObservableCollection<ApplicationRuntimeData> ApplicationsData { get; private set; }

        private LauncherDatabase _launcherDatabase;
        private ApplicationProcess _currentProcess;

        public SuperLauncher()
        {
            _launcherDatabase = new LauncherDatabase();

            ApplicationsData = new ObservableCollection<ApplicationRuntimeData>();
            foreach (var applicationData in _launcherDatabase.ApplicationsData)
            {
                var runtimeData = new ApplicationRuntimeData(applicationData);

                ApplicationsData.Add(runtimeData);
            }

            CurrentApplicationData = new ApplicationRuntimeData();
            if (ApplicationsData.Count > 0)
            {
                SelectApplication(ApplicationsData[0].AppGUID);
            }

            _sessionManager = new SuperLauncherSessionsManager();
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

        public void AddNewSession(Guid applicationGuid)
            => _sessionManager.AddNewSession(applicationGuid);


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
        }

        public void StartCurrentApplication()
        {
            if(_currentProcess != null)
            {
                _currentProcess.Stop();
            }

            _currentProcess = new ApplicationProcess(CurrentApplicationData.AppGUID, 
                CurrentApplicationData.AppExecutablePath, ProcessSessionFeedback);
        }

        private void ProcessSessionFeedback(Guid applicationGuid, (DateTime processStart, DateTime processEnd) times)
        {
            Console.WriteLine(times.processStart + " " + times.processEnd);
            //AddNewSession(applicationGuid);
        }
    }
}
