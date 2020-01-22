using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace SuperLauncher.Data
{
    /// <summary>
    /// App Data class
    /// Keeps app data, is serializable
    /// </summary>
    [Serializable]
    public class SuperLauncherAppData : INotifyPropertyChanged
    {
        [JsonProperty("app_guid")]
        private Guid _appGUID;
        public Guid AppGUID
        {
            get { return _appGUID; }
            set { _appGUID = value; NotifyPropertyChanged("_appGUID"); }
        }

        [JsonProperty("app_name")]
        private string _appName;
        public string AppName
        {
            get { return _appName; }
            set { _appName = value; NotifyPropertyChanged("_appName"); }
        }

        [JsonProperty("app_exe_path")]
        private string _appExecutablePath;
        public string AppExecutablePath
        {
            get { return _appExecutablePath; }
            set { _appExecutablePath = value; NotifyPropertyChanged("_appExecutablePath"); }
        }

        [JsonProperty("app_icon_path")]
        private string _appIconPath;
        public string AppIconPath
        {
            get { return _appIconPath; }
            set { _appIconPath = value; NotifyPropertyChanged("_appIconPath"); }
        }

        [JsonProperty("selected")]
        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; NotifyPropertyChanged("_selected"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
