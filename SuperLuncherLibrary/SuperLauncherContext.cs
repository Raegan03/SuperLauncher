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

            foreach (var item in SuperLauncherAppDatas)
            {
                if (CurrentSelectedApp != null)
                    item.Selected = false;

                if(item.Selected)
                {
                    CurrentSelectedApp = item;
                }
            }

            if(CurrentSelectedApp == null && SuperLauncherAppDatas.Count > 0)
            {
                SuperLauncherAppDatas[0].Selected = true;
                CurrentSelectedApp = SuperLauncherAppDatas[0];
            }

            SaveData();
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

            foreach (var item in SuperLauncherAppDatas)
            {
                item.Selected = false;
            }
            SuperLauncherAppDatas.Add(appData);

            appData.Selected = true;
            CurrentSelectedApp = appData;

            SaveData();
        }

        public void RemoveApplication(Guid applicationGuid)
        { }

        public void SelectApplication(Guid applicationGuid)
        {
            var app = SuperLauncherAppDatas.FirstOrDefault(x => x.AppGUID == applicationGuid);
            if(app != null)
            {
                foreach (var item in SuperLauncherAppDatas)
                {
                    item.Selected = false;
                }

                CurrentSelectedApp = app;
                CurrentSelectedApp.Selected = true;

                SuperLauncherAppDatas.Add(CurrentSelectedApp);
                SuperLauncherAppDatas.RemoveAt(SuperLauncherAppDatas.Count - 1);
            }
        }

        private void SaveData()
        {
            var appDatasPath = Path.Combine(AppDataDirectoryPath, APP_DATA_FILENAME);

            var appsJsonData = JsonConvert.SerializeObject(SuperLauncherAppDatas.ToArray());
            File.WriteAllText(appDatasPath, appsJsonData);
        }
    }
}
