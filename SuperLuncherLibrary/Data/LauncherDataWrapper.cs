using System;
using Newtonsoft.Json;

namespace SuperLauncher.Data
{
    [Serializable]
    internal class LauncherDataWrapper
    {
        [JsonProperty("applications_data")]
        internal ApplicationSerializableData[] ApplicationsData { get; set; }

        [JsonProperty("sessions_data")]
        internal SessionSerializableData[] SessionsData { get; set; }

        public LauncherDataWrapper()
        {
            ApplicationsData = new ApplicationSerializableData[0];
            SessionsData = new SessionSerializableData[0];
        }
    }
}
