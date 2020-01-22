using System;
using System.IO;
using System.Collections.Generic;
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
        private const string APP_CATEGORIES_DATA_FILENAME = "CategoriespData.data";

        public SuperLauncherAppData CurrentSelectedApp { get; private set; }
        public IReadOnlyList<SuperLauncherAppData> SuperLauncherAppDatas =>
            _superLauncherAppDatas;

        public IReadOnlyList<SuperLauncherAppCategoryData> SuperLauncherAppCategoryDatas =>
            _superLauncherAppCategoryDatas;

        private List<SuperLauncherAppData> _superLauncherAppDatas;
        private List<SuperLauncherAppCategoryData> _superLauncherAppCategoryDatas;

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
            var appCategoriesDatasPath = Path.Combine(AppDataDirectoryPath, APP_CATEGORIES_DATA_FILENAME);

            if (File.Exists(appDatasPath))
            {
                var jsonData = File.ReadAllText(appDatasPath);
                _superLauncherAppDatas = JsonConvert.DeserializeObject<List<SuperLauncherAppData>>(jsonData);
            }
            else
            {
                _superLauncherAppDatas = new List<SuperLauncherAppData>();

                var jsonData = JsonConvert.SerializeObject(_superLauncherAppDatas);
                File.WriteAllText(appDatasPath, jsonData);
            }

            if (File.Exists(appCategoriesDatasPath))
            {
                var jsonData = File.ReadAllText(appCategoriesDatasPath);
                _superLauncherAppCategoryDatas = JsonConvert.DeserializeObject<List<SuperLauncherAppCategoryData>>(jsonData);
            }
            else
            {
                _superLauncherAppCategoryDatas = new List<SuperLauncherAppCategoryData>();

                var jsonData = JsonConvert.SerializeObject(_superLauncherAppDatas);
                File.WriteAllText(appCategoriesDatasPath, jsonData);
            }
        }
    }
}
