using System.Collections.Generic;
using SuperLauncher.Data;

namespace SuperLauncher
{
    internal class FirstAchievementChecker : IAchievementChecker
    {
        public int AchievementID => 1;

        public bool ValidateAchievement(ApplicationRuntimeData applicationData, List<SessionRuntimeData> sessionsData)
        {
            return sessionsData.Count > 0;
        }
    }
}
