using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MyFileMovingService.Logic;
using System.IO;
using System.Timers;

namespace MyFileMovingService
{
    public partial class Service1 : ServiceBase
    {
        MainLogic ML = new MainLogic();
        public static Thread childThread;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            fileSystemWatcher1.EnableRaisingEvents = true;
            MyServiceLogger.Log("Started Moving Service At: " + DateTime.Now.TimeOfDay.ToString());
        }

        protected override void OnStop()
        {
            fileSystemWatcher1.EnableRaisingEvents = false;
            MyServiceLogger.Log("Stopped Service At: " + DateTime.Now.TimeOfDay.ToString());
        }

        private void fileDropped(object sender, System.IO.FileSystemEventArgs e)
        {
            TimeSpan timeSpan = new TimeSpan(0,15,0);
            MyServiceLogger.Log(e.Name + " Dropped At :" + DateTime.Now.TimeOfDay.ToString());
            childThread = new Thread(() => MainLogic.testingAndMoving(e.FullPath));
            if(!MainLogic.isFileLocked(new FileInfo(e.FullPath)))
            {
                childThread.Start();
            }
            else
            {
                childThread.Suspend();
                System.Timers.Timer ticker = new System.Timers.Timer(900000);
                ticker.Elapsed += delegate { restartThread(childThread, ticker); };
                ticker.Enabled = true;
                ticker.Start();
            }
        }
        
        public static void restartThread(Thread myThread, System.Timers.Timer timer)
        {
            myThread.Resume();
            timer.Close();
        }

        private void fileCreated(object sender, System.IO.FileSystemEventArgs e)
        {
            MyServiceLogger.Log(e.Name + " Dropped At :" + DateTime.Now.TimeOfDay.ToString());
            childThread = new Thread(() => MainLogic.testingAndMoving(e.FullPath));
            if (!MainLogic.isFileLocked(new FileInfo(e.FullPath)))
            {
                childThread.Start();
            }
            else
            {
                childThread.Suspend();
                System.Timers.Timer ticker = new System.Timers.Timer(900000);
                ticker.Elapsed += delegate { restartThread(childThread, ticker); };
                ticker.Enabled = true;
                ticker.Start();
            }
        }
    }
}
