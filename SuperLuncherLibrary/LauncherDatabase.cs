using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SuperLauncher.Data;

namespace SuperLauncher
{
    internal class LauncherDatabase
    {
        private const string APP_DATA_FILENAME = "AppData.data";

        public IReadOnlyList<ApplicationSerializableData> ApplicationsData => 
            _applicationsData;

        private List<ApplicationSerializableData> _applicationsData;

        private readonly string AppDataPath;

        public LauncherDatabase()
        {
            AppDataPath = Path.Combine(LauncherHelper.GetAppDataPath(), APP_DATA_FILENAME);

            LauncherDataWrapper wrapper;
            if (File.Exists(AppDataPath))
            {
                var jsonData = File.ReadAllText(AppDataPath);
                wrapper = JsonConvert.DeserializeObject<LauncherDataWrapper>(jsonData);
            }
            else
            {
                wrapper = new LauncherDataWrapper();
            }

            _applicationsData = wrapper.ApplicationsData.ToList();
        }

        public void UpdateApplicationData(ApplicationRuntimeData applicationData)
        {
            var appData = _applicationsData.Find(x => x.AppGUID == applicationData.AppGUID);

            if (appData != null)
            {
                var index = _applicationsData.IndexOf(appData);
                _applicationsData[index] = new ApplicationSerializableData(applicationData);
            }
            else
            {
                _applicationsData.Add(new ApplicationSerializableData(applicationData));
            }

            SaveAppData();
        }

        private void SaveAppData()
        {
            var wrapper = new LauncherDataWrapper
            {
                ApplicationsData = _applicationsData.ToArray()
            };

            var jsonData = JsonConvert.SerializeObject(wrapper);
            File.WriteAllText(AppDataPath, jsonData);
        }
    }
}
