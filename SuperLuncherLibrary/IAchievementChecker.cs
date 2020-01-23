using System.Collections.Generic;
using SuperLauncher.Data;

namespace SuperLauncher
{
    public interface IAchievementChecker
    {
        int AchievementID { get; }
        bool ValidateAchievement(ApplicationRuntimeData applicationData, List<SessionRuntimeData> sessionsData);
    }
}
