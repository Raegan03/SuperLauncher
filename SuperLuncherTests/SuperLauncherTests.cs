using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperLauncher;

namespace SuperLauncherTests
{
    [TestClass]
    public class SuperLauncherTests
    {
        [TestMethod]
        public void Test_SuperLaucher_Init()
        {
            var superLauncher = new SuperLauncher.SuperLauncher(null);

            Assert.AreEqual(superLauncher.AchievementsCheckers.Count, superLauncher.CurrentApplicationAchievements.Count);
            Assert.IsNotNull(superLauncher.ApplicationsData);
            Assert.IsNotNull(superLauncher.SessionsData);

            if(superLauncher.ApplicationsData.Count > 0)
            {
                Assert.IsFalse(superLauncher.IsApplicationsListEmpty);
                Assert.AreEqual(superLauncher.CurrentApplicationData.AppGUID, superLauncher.ApplicationsData[0].AppGUID);

                if(superLauncher.SessionsData.Count > 0)
                {
                    foreach (var session in superLauncher.SessionsData)
                    {
                        Assert.AreEqual(superLauncher.CurrentApplicationData.AppGUID, session.AppGUID);
                    }
                }
            }
            else
            {
                Assert.IsTrue(superLauncher.IsApplicationsListEmpty);
                Assert.IsNotNull(superLauncher.CurrentApplicationData);
            }
        }
    }
}
