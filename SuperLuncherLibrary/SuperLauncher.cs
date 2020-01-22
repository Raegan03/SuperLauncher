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

        private SuperLauncherContext _superLauncherContext;

        public SuperLauncher()
        {
            _superLauncherContext = new SuperLauncherContext();
        }

        public void AddNewApplication(string applicationPath)
            => _superLauncherContext.AddNewApplication(applicationPath);

        public void SelectApplication(Guid applicationGuid)
            => _superLauncherContext.SelectApplication(applicationGuid);
    }
}
