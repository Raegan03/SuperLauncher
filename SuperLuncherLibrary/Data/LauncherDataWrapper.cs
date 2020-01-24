using System;
using Newtonsoft.Json;

namespace SuperLauncher.Data
{
    /// <summary>
    /// Wrapper for all serialization data to save them in one file which is easier to maintain
    /// </summary>
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
