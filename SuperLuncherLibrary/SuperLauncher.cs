using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperLauncher
{
    /// <summary>
    /// Main Super Launcher class
    /// Creates all others helper classes for GUI framwork (WPF) to render
    /// </summary>
    public class SuperLauncher
    {
        public SuperLauncherContext SuperLauncherContext { get; private set; }

        public SuperLauncher()
        {
            SuperLauncherContext = new SuperLauncherContext();
        }
    }
}
