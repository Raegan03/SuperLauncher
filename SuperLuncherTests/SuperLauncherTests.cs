using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperLauncher;
using SuperLauncher.Data;

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

        [TestMethod]
        public void Test_SuperLaucher_Achievements()
        {
            var first = new FirstAchievementChecker();
            var second = new SecondAchievementChecker();
            var third = new ThirdAchievementChecker();
            var four = new FourAchievementChecker();

            List<SessionRuntimeData> sessions = new List<SessionRuntimeData>();

            Assert.IsFalse(first.ValidateAchievement(null, sessions));
            Assert.IsFalse(second.ValidateAchievement(null, sessions));
            Assert.IsFalse(third.ValidateAchievement(null, sessions));
            Assert.IsFalse(four.ValidateAchievement(null, sessions));

            sessions.Add(new SessionRuntimeData()
            {
                StartSessionDate = DateTime.Now,
                EndSessionDate = DateTime.Now.AddMinutes(1)
            });

            sessions.Add(new SessionRuntimeData()
            {
                StartSessionDate = DateTime.Now,
                EndSessionDate = DateTime.Now.AddMinutes(2)
            });

            sessions.Add(new SessionRuntimeData()
            {
                StartSessionDate = DateTime.Now,
                EndSessionDate = DateTime.Now.AddMinutes(5)
            });

            sessions.Add(new SessionRuntimeData()
            {
                StartSessionDate = DateTime.Now,
                EndSessionDate = DateTime.Now.AddMinutes(5)
            });

            sessions.Add(new SessionRuntimeData()
            {
                StartSessionDate = DateTime.Now,
                EndSessionDate = DateTime.Now.AddMinutes(2)
            });

            Assert.IsTrue(first.ValidateAchievement(null, sessions));
            Assert.IsFalse(second.ValidateAchievement(null, sessions));
            Assert.IsTrue(third.ValidateAchievement(null, sessions));
            Assert.IsFalse(four.ValidateAchievement(null, sessions));

            sessions.Add(new SessionRuntimeData()
            {
                StartSessionDate = DateTime.Now,
                EndSessionDate = DateTime.Now.AddMinutes(10)
            });

            sessions.Add(new SessionRuntimeData()
            {
                StartSessionDate = DateTime.Now,
                EndSessionDate = DateTime.Now.AddMinutes(29)
            });

            Assert.IsTrue(first.ValidateAchievement(null, sessions));
            Assert.IsTrue(second.ValidateAchievement(null, sessions));
            Assert.IsTrue(third.ValidateAchievement(null, sessions));
            Assert.IsTrue(four.ValidateAchievement(null, sessions));
        }

        [TestMethod]
        public void Test_SuperLaucher_SessionTest()
        {
            var session = new SessionSerializableData()
            {
                StartSessionDate = DateTime.Now,
                EndSessionDate = DateTime.Now.AddMinutes(37)
            };

            Assert.AreEqual((int)(session.EndSessionDate - session.StartSessionDate).TotalMinutes, 37);

            var runtimeSession = new SessionRuntimeData(session);

            Assert.AreEqual(runtimeSession.StartSessionDate, session.StartSessionDate);
            Assert.AreEqual(runtimeSession.EndSessionDate, session.EndSessionDate);

            Assert.AreEqual(runtimeSession.TotalDurationMinutes, 37);
        }

        [TestMethod]
        public void Test_SuperLaucher_ApplicationTest()
        {
            var app = new ApplicationSerializableData()
            {
                AppGUID = Guid.NewGuid(),
                AppExecutablePath = "path_exe",
                AppIconPath = "icon_path_exe",
                AppName = "name"
            };


            var runtimeApp = new ApplicationRuntimeData(app);

            Assert.AreEqual(runtimeApp.AppGUID, app.AppGUID);
            Assert.AreEqual(runtimeApp.AppExecutablePath, app.AppExecutablePath);
            Assert.AreEqual(runtimeApp.AppIconPath, app.AppIconPath);
            Assert.AreEqual(runtimeApp.AppName, app.AppName);
        }
    }
}
