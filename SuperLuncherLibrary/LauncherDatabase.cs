using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SuperLauncher.Data;

namespace SuperLauncher
{
    /// <summary>
    /// Launcher data base class
    /// On creation it's trying to load all data stored on PC and if there is no data it creates folders for it
    /// </summary>
    internal class LauncherDatabase
    {
        private const string APP_DATA_FILENAME = "AppData.data";

        public IReadOnlyList<ApplicationSerializableData> ApplicationsData => 
            _applicationsData;
        public IReadOnlyList<SessionSerializableData> SessionsData =>
            _sessionsData;

        private List<ApplicationSerializableData> _applicationsData;
        private List<SessionSerializableData> _sessionsData;

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
            _sessionsData = wrapper.SessionsData.ToList();
        }

        /// <summary>
        /// Returns sessions list associated with applicationGuid
        /// </summary>
        /// <param name="applicationGuid">GUID of current application</param>
        /// <returns></returns>
        public List<SessionSerializableData> GetApplicationSessions(Guid applicationGuid)
        {
            return _sessionsData.Where(x => x.AppGUID == applicationGuid).ToList();
        }

        /// <summary>
        /// Updates session data if exists and add if is new, saves it on drive
        /// </summary>
        /// <param name="sessionData">Session data to be updated/added</param>
        public void UpdateSessionData(SessionRuntimeData sessionData)
        {
            var sessData = _sessionsData.Find(x => x.SessionGUID == sessionData.SessionGUID);

            if (sessData != null)
            {
                var index = _sessionsData.IndexOf(sessData);
                _sessionsData[index] = new SessionSerializableData(sessionData);
            }
            else
            {
                _sessionsData.Add(new SessionSerializableData(sessionData));
            }

            SaveAppData();
        }

        /// <summary>
        /// Removes all data associated with application (app data and all sessions)
        /// </summary>
        /// <param name="applicationGuid">GUID of application that should be deleted</param>
        public void DeleteApplicationData(Guid applicationGuid)
        {
            var sessions = _sessionsData.Where(x => x.AppGUID == applicationGuid).ToList();
            foreach (var session in sessions)
            {
                _sessionsData.Remove(session);
            }

            _applicationsData.RemoveAll(x => x.AppGUID == applicationGuid);

            SaveAppData();
        }

        /// <summary>
        /// Updates application data if exists and add if is new, saves it on drive
        /// </summary>
        /// <param name="applicationData">Application data to be updated</param>
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

        /// <summary>
        /// Saves application and sessions data on drive in local app location
        /// </summary>
        private void SaveAppData()
        {
            var wrapper = new LauncherDataWrapper
            {
                ApplicationsData = _applicationsData.ToArray(),
                SessionsData = _sessionsData.ToArray()
            };

            var jsonData = JsonConvert.SerializeObject(wrapper);
            File.WriteAllText(AppDataPath, jsonData);
        }
    }
}
