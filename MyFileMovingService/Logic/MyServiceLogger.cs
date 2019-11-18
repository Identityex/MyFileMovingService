using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyFileMovingService.Logic
{
    class MyServiceLogger 
    {
        
        protected static String logFilePath = @"G:\MyLog.txt";
        public static void Log(string message)
        {
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath);
            }
            File.AppendAllText(logFilePath, string.Format("{0}{1}", message, Environment.NewLine));

        }
    
    }
}
