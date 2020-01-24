using System.Linq;
using System.Collections.Generic;
using SuperLauncher.Data;

namespace SuperLauncher
{
    /// <summary>
    /// First achievement checker
    /// </summary>
    public class FirstAchievementChecker : IAchievementChecker
    {
        public int AchievementID => 1;

        /// <summary>
        /// Validate if there is any session for application
        /// </summary>
        /// <param name="applicationData">Current application data</param>
        /// <param name="sessionsData">Current application sessions</param>
        /// <returns>True if achievement is unlocked and false if it isn't</returns>
        public bool ValidateAchievement(ApplicationRuntimeData applicationData, List<SessionRuntimeData> sessionsData)
        {
            return sessionsData.Count > 0;
        }
    }

    /// <summary>
    /// Second achievement checker
    /// </summary>
    public class SecondAchievementChecker : IAchievementChecker
    {
        public int AchievementID => 2;

        /// <summary>
        /// Validate if there is any session longer then 10 minutes
        /// </summary>
        /// <param name="applicationData">Current application data</param>
        /// <param name="sessionsData">Current application sessions</param>
        /// <returns>True if achievement is unlocked and false if it isn't</returns>
        public bool ValidateAchievement(ApplicationRuntimeData applicationData, List<SessionRuntimeData> sessionsData)
        {
            return sessionsData.Count(x => x.TotalDurationMinutes >= 10) >= 1;
        }
    }

    /// <summary>
    /// Third achievement checker
    /// </summary>
    public class ThirdAchievementChecker : IAchievementChecker
    {
        public int AchievementID => 3;

        /// <summary>
        /// Validate if there is at least 5 sessions for application
        /// </summary>
        /// <param name="applicationData">Current application data</param>
        /// <param name="sessionsData">Current application sessions</param>
        /// <returns>True if achievement is unlocked and false if it isn't</returns>
        public bool ValidateAchievement(ApplicationRuntimeData applicationData, List<SessionRuntimeData> sessionsData)
        {
            return sessionsData.Count >= 5;
        }
    }

    /// <summary>
    /// Four achievement checker
    /// </summary>
    public class FourAchievementChecker : IAchievementChecker
    {
        public int AchievementID => 4;

        /// <summary>
        /// Validate if all sessions have duration longer then 30 minutes
        /// </summary>
        /// <param name="applicationData">Current application data</param>
        /// <param name="sessionsData">Current application sessions</param>
        /// <returns>True if achievement is unlocked and false if it isn't</returns>
        public bool ValidateAchievement(ApplicationRuntimeData applicationData, List<SessionRuntimeData> sessionsData)
        {
            return sessionsData.Sum(x => x.TotalDurationMinutes) >= 30;
        }
    }
}
