using System;
using Newtonsoft.Json;

namespace SuperLauncher.Data
{
    /// <summary>
    /// App Data class
    /// Keeps app data, is serializable
    /// </summary>
    [Serializable]
    public class ApplicationSerializableData
    {
        [JsonProperty("app_guid")]
        public Guid AppGUID { get; set; }

        [JsonProperty("app_name")]
        public string AppName { get; set; }

        [JsonProperty("app_exe_path")]
        public string AppExecutablePath { get; set; }

        [JsonProperty("app_icon_path")]
        public string AppIconPath { get; set; }


        public ApplicationSerializableData() { }

        public ApplicationSerializableData(ApplicationRuntimeData runtimeData)
        {
            AppGUID = runtimeData.AppGUID;
            AppName = runtimeData.AppName;
            AppExecutablePath = runtimeData.AppExecutablePath;
            AppIconPath = runtimeData.AppIconPath;
        }
    }
}
