using System;
using System.ComponentModel;

namespace SuperLauncher.Data
{
    /// <summary>
    /// Runtime version of application data used to save state of application while Launcher is running
    /// </summary>
    public class ApplicationRuntimeData : INotifyPropertyChanged
    {
        /// <summary>
        /// App indetifier
        /// </summary>
        private Guid _appGUID;
        public Guid AppGUID
        {
            get { return _appGUID; }
            set { _appGUID = value; NotifyPropertyChanged("AppGUID"); }
        }

        /// <summary>
        /// App name, based on executable file name but can be changed
        /// </summary>
        private string _appName;
        public string AppName
        {
            get { return _appName; }
            set { _appName = value; NotifyPropertyChanged("AppName"); }
        }

        /// <summary>
        /// App exe path, use by process
        /// </summary>
        private string _appExecutablePath;
        public string AppExecutablePath
        {
            get { return _appExecutablePath; }
            set { _appExecutablePath = value; NotifyPropertyChanged("AppExecutablePath"); }
        }

        /// <summary>
        /// App icon path, use by MainView
        /// </summary>
        private string _appIconPath;
        public string AppIconPath
        {
            get { return _appIconPath; }
            set { _appIconPath = value; NotifyPropertyChanged("AppIconPath"); }
        }

        /// <summary>
        /// If app is current app selected is equal true
        /// </summary>
        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; NotifyPropertyChanged("Selected"); }
        }

        /// <summary>
        /// Date of last session copied from sessions list
        /// </summary>
        private DateTime _lastSession;
        public DateTime LastSession
        {
            get { return _lastSession; }
            set { _lastSession = value; NotifyPropertyChanged("LastSession"); }
        }


        public ApplicationRuntimeData() { }

        /// <summary>
        /// Contructor made for transitioning from serializable to runtime data easier and safer
        /// </summary>
        /// <param name="serializableData">Serialization data to copy from</param>
        public ApplicationRuntimeData(ApplicationSerializableData serializableData)
        {
            _appGUID = serializableData.AppGUID;
            _appName = serializableData.AppName;
            _appExecutablePath = serializableData.AppExecutablePath;
            _appIconPath = serializableData.AppIconPath;
        }

        /// <summary>
        /// WPF Notification handler, use for ItemsControl to react on data changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
