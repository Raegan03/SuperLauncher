using System;
using Newtonsoft.Json;

namespace SuperLauncher.Data
{
    [Serializable]
    internal class LauncherDataWrapper
    {
        [JsonProperty("applications_data")]
        internal ApplicationSerializableData[] ApplicationsData { get; set; }

        public LauncherDataWrapper()
        {
            ApplicationsData = new ApplicationSerializableData[0];
        }
    }
}
