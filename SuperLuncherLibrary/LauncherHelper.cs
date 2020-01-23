using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperLauncher
{
    internal static class LauncherHelper
    {
        private const string APP_DIRECTORY_NAME = "SuperLauncher";
        private const string APP_ICONS_DIRECTORY_NAME = "Icons";

        public static string GetAppDataPath()
        {
            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), APP_DIRECTORY_NAME);
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            return appDataPath;
        }

        public static string GetAppIconsPath()
        {
            var appIconsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), APP_DIRECTORY_NAME, APP_ICONS_DIRECTORY_NAME);
            if (!Directory.Exists(appIconsPath))
            {
                Directory.CreateDirectory(appIconsPath);
            }

            return appIconsPath;
        }
    }
}
