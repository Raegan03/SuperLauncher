using System;
using System.ComponentModel;

namespace SuperLauncher.Data
{
    public class SessionRuntimeData : INotifyPropertyChanged
    {
        /// <summary>
        /// Session unique identifier
        /// </summary>
        private Guid _sessionGuid;
        public Guid SessionGUID
        {
            get { return _sessionGuid; }
            set { _sessionGuid = value; NotifyPropertyChanged("SessionGuid"); }
        }

        /// <summary>
        /// Identifier of app that session belongs to
        /// </summary>
        private Guid _appGUID;
        public Guid AppGUID
        {
            get { return _appGUID; }
            set { _appGUID = value; NotifyPropertyChanged("AppGUID"); }
        }

        /// <summary>
        /// Session ID used to show session index in WPF window
        /// </summary>
        private int _sessionId;
        public int SessionID
        {
            get { return _sessionId; }
            set { _sessionId = value; NotifyPropertyChanged("SessionID"); }
        }

        /// <summary>
        /// Process start date 
        /// </summary>
        private DateTime _startSessionDate;
        public DateTime StartSessionDate
        {
            get { return _startSessionDate; }
            set { _startSessionDate = value; NotifyPropertyChanged("StartSessionDate"); }
        }

        /// <summary>
        /// Process end date
        /// </summary>
        private DateTime _endSessionDate;
        public DateTime EndSessionDate
        {
            get { return _endSessionDate; }
            set { _endSessionDate = value; NotifyPropertyChanged("EndSessionDate"); }
        }

        /// <summary>
        /// Duration of session calculated from start and end date, in minutes rounded 
        /// </summary>
        public int TotalDurationMinutes => (int)(EndSessionDate - StartSessionDate).TotalMinutes;

        public SessionRuntimeData() { }

        /// <summary>
        /// Contructor made for transitioning from runtime to serializable data easier and safer
        /// </summary>
        /// <param name="serializableData">Runtime data to copy from</param>
        public SessionRuntimeData(SessionSerializableData serializableData) 
        {
            SessionGUID = serializableData.SessionGUID;
            AppGUID = serializableData.AppGUID;
            StartSessionDate = serializableData.StartSessionDate;
            EndSessionDate = serializableData.EndSessionDate;
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
