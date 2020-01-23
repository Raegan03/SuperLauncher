using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace SuperLauncher.Data
{
    public class SuperLauncherSessionData : INotifyPropertyChanged
    {
        [JsonProperty("app_guid")]
        private Guid _appGUID;
        public Guid AppGUID
        {
            get { return _appGUID; }
            set { _appGUID = value; NotifyPropertyChanged("_appGUID"); }
        }

        [JsonProperty("session_start_date")]
        private DateTime _startSessionDate;
        public DateTime StartSessionDate
        {
            get { return _startSessionDate; }
            set { _startSessionDate = value; NotifyPropertyChanged("_startSessionDate"); }
        }

        [JsonProperty("session_end_date")]
        private DateTime _endSessionDate;
        public DateTime EndSessionDate
        {
            get { return _endSessionDate; }
            set { _endSessionDate = value; NotifyPropertyChanged("_endSessionDate"); }
        }

        public double MinutesPassed => (EndSessionDate - StartSessionDate).TotalMinutes;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
