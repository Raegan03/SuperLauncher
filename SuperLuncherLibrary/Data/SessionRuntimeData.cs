using System;
using System.ComponentModel;

namespace SuperLauncher.Data
{
    public class SessionRuntimeData : INotifyPropertyChanged
    {
        private Guid _sessionGuid;
        public Guid SessionGUID
        {
            get { return _sessionGuid; }
            set { _sessionGuid = value; NotifyPropertyChanged("SessionGuid"); }
        }

        private Guid _appGUID;
        public Guid AppGUID
        {
            get { return _appGUID; }
            set { _appGUID = value; NotifyPropertyChanged("AppGUID"); }
        }

        private DateTime _startSessionDate;
        public DateTime StartSessionDate
        {
            get { return _startSessionDate; }
            set { _startSessionDate = value; NotifyPropertyChanged("StartSessionDate"); }
        }

        private DateTime _endSessionDate;
        public DateTime EndSessionDate
        {
            get { return _endSessionDate; }
            set { _endSessionDate = value; NotifyPropertyChanged("EndSessionDate"); }
        }

        public double TotalDurationMinutes => (EndSessionDate - StartSessionDate).TotalMinutes;

        public SessionRuntimeData() { }

        public SessionRuntimeData(SessionSerializableData serializableData) 
        {
            SessionGUID = serializableData.SessionGUID;
            AppGUID = serializableData.AppGUID;
            StartSessionDate = serializableData.StartSessionDate;
            EndSessionDate = serializableData.EndSessionDate;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
