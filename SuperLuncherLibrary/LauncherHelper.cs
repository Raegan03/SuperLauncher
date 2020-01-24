using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperLauncher
{
    /// <summary>
    /// Additional helper class providing curcial paths for objects in library
    /// </summary>
    internal static class LauncherHelper
    {
        private const string APP_DIRECTORY_NAME = "SuperLauncher";
        private const string APP_ICONS_DIRECTORY_NAME = "Icons";

        /// <summary>
        /// Returns path to local application directory 
        /// </summary>
        /// <returns></returns>
        public static string GetAppDataPath()
        {
            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), APP_DIRECTORY_NAME);
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            return appDataPath;
        }

        /// <summary>
        /// Returns path to Icons folder in local application directory 
        /// </summary>
        /// <returns></returns>
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
