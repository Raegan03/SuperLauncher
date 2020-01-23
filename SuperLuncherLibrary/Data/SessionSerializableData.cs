using System;
using Newtonsoft.Json;

namespace SuperLauncher.Data
{
    [Serializable]
    public class SessionSerializableData
    {
        [JsonProperty("session_guid")]
        public Guid SessionGUID { get; set; }

        [JsonProperty("app_guid")]
        public Guid AppGUID { get; set; }

        [JsonProperty("start_session_date")]
        public DateTime StartSessionDate { get; set; }

        [JsonProperty("end_session_date")]
        public DateTime EndSessionDate { get; set; }

        public SessionSerializableData() { }

        public SessionSerializableData(SessionRuntimeData runtimeData)
        {
            SessionGUID = runtimeData.SessionGUID;
            AppGUID = runtimeData.AppGUID;
            StartSessionDate = runtimeData.StartSessionDate;
            EndSessionDate = runtimeData.EndSessionDate;
        }
    }
}
