using System;
using System.Collections.ObjectModel;
using SuperLauncher.Data;

namespace SuperLauncher
{
    /// <summary>
    /// Main Super Launcher class
    /// Creates all others helper classes for GUI framwork (WPF) to render
    /// </summary>
    public class SuperLauncher
    {
        public SuperLauncherAppData CurrentSelectedApp => 
            _superLauncherContext.CurrentSelectedApp;

        public ObservableCollection<SuperLauncherAppData> SuperLauncherAppDatas => 
            _superLauncherContext.SuperLauncherAppDatas;

        public ObservableCollection<SuperLauncherSessionData> SuperLauncherSessionDatas =>
            _sessionManager.SuperLauncherSessionDatas;

        private SuperLauncherContext _superLauncherContext;
        private SuperLauncherSessionsManager _sessionManager;

        public SuperLauncher()
        {
            _superLauncherContext = new SuperLauncherContext();

            _sessionManager = new SuperLauncherSessionsManager();
        }

        public void AddNewSession(Guid applicationGuid)
            => _sessionManager.AddNewSession(applicationGuid);

        public void AddNewApplication(string applicationPath)
            => _superLauncherContext.AddNewApplication(applicationPath);

        public void SelectApplication(Guid applicationGuid)
            => _superLauncherContext.SelectApplication(applicationGuid);
    }
}
