using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperLauncher
{
    internal class ApplicationProcess
    {
    }

    //TODO
    //Process process = new Process();
    //process.StartInfo.FileName = openFileDialog.FileName;
    //            process.Start();
    //            RuntimeMonitor rtm = new RuntimeMonitor(5000);
    //process.WaitForExit();
    //            rtm.Close();
    //public class RuntimeMonitor
    //{
    //    System.Timers.Timer timer;

    //    // Define other methods and classes here
    //    public RuntimeMonitor(double intervalMs) // in milliseconds
    //    {
    //        timer = new System.Timers.Timer();
    //        timer.Interval = intervalMs;
    //        timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_func);
    //        timer.AutoReset = true;
    //        timer.Start();
    //    }

    //    void timer_func(object source, object e)
    //    {
    //        Console.WriteLine("Yes");
    //    }

    //    public void Close()
    //    {
    //        timer.Stop();
    //    }
    //}
}
