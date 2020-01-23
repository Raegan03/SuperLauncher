using System;
using System.ComponentModel;

namespace SuperLauncher.Data
{
    /// <summary>
    /// App Data class
    /// Keeps app data, is serializable
    /// </summary>
    public class ApplicationRuntimeData : INotifyPropertyChanged
    {
        private Guid _appGUID;
        public Guid AppGUID
        {
            get { return _appGUID; }
            set { _appGUID = value; NotifyPropertyChanged("AppGUID"); }
        }

        private string _appName;
        public string AppName
        {
            get { return _appName; }
            set { _appName = value; NotifyPropertyChanged("AppName"); }
        }

        private string _appExecutablePath;
        public string AppExecutablePath
        {
            get { return _appExecutablePath; }
            set { _appExecutablePath = value; NotifyPropertyChanged("AppExecutablePath"); }
        }

        private string _appIconPath;
        public string AppIconPath
        {
            get { return _appIconPath; }
            set { _appIconPath = value; NotifyPropertyChanged("AppIconPath"); }
        }

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; NotifyPropertyChanged("Selected"); }
        }

        private DateTime _lastSession;
        public DateTime LastSession
        {
            get { return _lastSession; }
            set { _lastSession = value; NotifyPropertyChanged("LastSession"); }
        }


        public ApplicationRuntimeData() { }

        public ApplicationRuntimeData(ApplicationSerializableData serializableData)
        {
            _appGUID = serializableData.AppGUID;
            _appName = serializableData.AppName;
            _appExecutablePath = serializableData.AppExecutablePath;
            _appIconPath = serializableData.AppIconPath;
        }

        public void ChangeData(ApplicationRuntimeData newData)
        {
            AppGUID = newData.AppGUID;
            AppName = newData.AppName;
            AppExecutablePath = newData.AppExecutablePath;
            AppIconPath = newData.AppIconPath;
            Selected = newData.Selected;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
