using System.Linq;
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

    internal class SecondAchievementChecker : IAchievementChecker
    {
        public int AchievementID => 2;

        public bool ValidateAchievement(ApplicationRuntimeData applicationData, List<SessionRuntimeData> sessionsData)
        {
            return sessionsData.Count(x => x.TotalDurationMinutes >= 10) >= 1;
        }
    }

    internal class ThirdAchievementChecker : IAchievementChecker
    {
        public int AchievementID => 3;

        public bool ValidateAchievement(ApplicationRuntimeData applicationData, List<SessionRuntimeData> sessionsData)
        {
            return sessionsData.Count >= 5;
        }
    }

    internal class FourAchievementChecker : IAchievementChecker
    {
        public int AchievementID => 4;

        public bool ValidateAchievement(ApplicationRuntimeData applicationData, List<SessionRuntimeData> sessionsData)
        {
            return sessionsData.Sum(x => x.TotalDurationMinutes) >= 30;
        }
    }
}
