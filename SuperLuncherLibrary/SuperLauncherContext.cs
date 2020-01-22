using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;
using System.Drawing;
using Newtonsoft.Json;
using SuperLauncher.Data;

namespace SuperLauncher
{
    /// <summary>
    /// Super Launcher context class
    /// loads and manage all app and categories related data 
    /// </summary>
    public class SuperLauncherContext
    {
        private const string APP_DIRECTORY_NAME = "SuperLauncher";
        private const string APP_ICONS_DIRECTORY_NAME = "Icons";

        private const string APP_DATA_FILENAME = "AppData.data";

        public SuperLauncherAppData CurrentSelectedApp { get; private set; }

        public ObservableCollection<SuperLauncherAppData> SuperLauncherAppDatas { get; private set; }

        private readonly string AppDataDirectoryPath;
        private readonly string AppIconsDirectoryPath;

        public SuperLauncherContext()
        {
            AppDataDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), APP_DIRECTORY_NAME);
            if(!Directory.Exists(AppDataDirectoryPath))
            {
                Directory.CreateDirectory(AppDataDirectoryPath);
            }

            AppIconsDirectoryPath = Path.Combine(AppDataDirectoryPath, APP_ICONS_DIRECTORY_NAME);
            if (!Directory.Exists(AppIconsDirectoryPath))
            {
                Directory.CreateDirectory(AppIconsDirectoryPath);
            }

            var appDatasPath = Path.Combine(AppDataDirectoryPath, APP_DATA_FILENAME);

            if (File.Exists(appDatasPath))
            {
                var jsonData = File.ReadAllText(appDatasPath);
                var array = JsonConvert.DeserializeObject<SuperLauncherAppData[]>(jsonData);
                SuperLauncherAppDatas = new ObservableCollection<SuperLauncherAppData>(array);
            }
            else
            {
                SuperLauncherAppDatas = new ObservableCollection<SuperLauncherAppData>();

                var jsonData = JsonConvert.SerializeObject(SuperLauncherAppDatas.ToArray());
                File.WriteAllText(appDatasPath, jsonData);
            }
        }

        public void AddNewApplication(string applicationPath)
        {
            var appData = new SuperLauncherAppData
            {
                AppGUID = Guid.NewGuid(),
                AppName = Path.GetFileNameWithoutExtension(applicationPath),
                AppExecutablePath = applicationPath,
            };

            using (var icon = Icon.ExtractAssociatedIcon(applicationPath))
            {
                using (var bitmap = icon.ToBitmap())
                {
                    var iconPath = Path.Combine(AppIconsDirectoryPath, appData.AppGUID + ".png");
                    appData.AppIconPath = iconPath;
                    using (var stream = new StreamWriter(iconPath))
                    {
                        bitmap.Save(stream.BaseStream, System.Drawing.Imaging.ImageFormat.Png);
                        stream.Close();
                    }
                }
            }

            SuperLauncherAppDatas.Add(appData);
            CurrentSelectedApp = appData;

            SaveData();
        }

        public void RemoveApplication(Guid applicationGuid)
        { }

        private void SaveData()
        {
            var appDatasPath = Path.Combine(AppDataDirectoryPath, APP_DATA_FILENAME);

            var appsJsonData = JsonConvert.SerializeObject(SuperLauncherAppDatas.ToArray());
            File.WriteAllText(appDatasPath, appsJsonData);
        }
    }
}
