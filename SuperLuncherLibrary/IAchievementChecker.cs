using System.Collections.Generic;
using SuperLauncher.Data;

namespace SuperLauncher
{
    /// <summary>
    /// Interface used to create same logic for all checkers
    /// </summary>
    public interface IAchievementChecker
    {
        int AchievementID { get; }
        bool ValidateAchievement(ApplicationRuntimeData applicationData, List<SessionRuntimeData> sessionsData);
    }
}
