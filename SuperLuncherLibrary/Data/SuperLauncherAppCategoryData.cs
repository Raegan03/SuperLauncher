using System;
using Newtonsoft.Json;

namespace SuperLauncher.Data
{
    /// <summary>
    /// Category Data class
    /// Keeps category data, is serializable
    /// </summary>
    [Serializable]
    public class SuperLauncherAppCategoryData
    {
        [JsonProperty("category_guid")]
        public Guid CategoryGUID { get; set; }

        [JsonProperty("category_name")]
        public string CategoryName { get; set; }
    }
}
