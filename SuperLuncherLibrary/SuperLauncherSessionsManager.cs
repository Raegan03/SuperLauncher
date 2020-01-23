using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperLauncher.Data;
using Newtonsoft.Json;

namespace SuperLauncher
{
    public class SuperLauncherSessionsManager
    {
        private const string APP_DIRECTORY_NAME = "SuperLauncher";
        private const string SESSION_DATA_FILENAME = "SessionData.data";

        public ObservableCollection<SuperLauncherSessionData> SuperLauncherSessionDatas { get; private set; }

        private readonly string AppDataDirectoryPath;

        public SuperLauncherSessionsManager()
        {
            AppDataDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), APP_DIRECTORY_NAME);
            if (!Directory.Exists(AppDataDirectoryPath))
            {
                Directory.CreateDirectory(AppDataDirectoryPath);
            }

            var sessionDatasPath = Path.Combine(AppDataDirectoryPath, SESSION_DATA_FILENAME);

            if (File.Exists(sessionDatasPath))
            {
                var jsonData = File.ReadAllText(sessionDatasPath);
                var array = JsonConvert.DeserializeObject<SuperLauncherSessionData[]>(jsonData);
                SuperLauncherSessionDatas = new ObservableCollection<SuperLauncherSessionData>(array);
            }
            else
            {
                SuperLauncherSessionDatas = new ObservableCollection<SuperLauncherSessionData>();

                var jsonData = JsonConvert.SerializeObject(SuperLauncherSessionDatas.ToArray());
                File.WriteAllText(sessionDatasPath, jsonData);
            }
        }

        public void AddNewSession(Guid appGuid)
        {
            var newSession = new SuperLauncherSessionData
            {
                AppGUID = appGuid,
                StartSessionDate = DateTime.Now,
                EndSessionDate = DateTime.Now
            };

            SuperLauncherSessionDatas.Add(newSession);

            SaveData();
        }

        private void SaveData()
        {
            var appDatasPath = Path.Combine(AppDataDirectoryPath, SESSION_DATA_FILENAME);

            var appsJsonData = JsonConvert.SerializeObject(SuperLauncherSessionDatas.ToArray());
            File.WriteAllText(appDatasPath, appsJsonData);
        }
    }
}
