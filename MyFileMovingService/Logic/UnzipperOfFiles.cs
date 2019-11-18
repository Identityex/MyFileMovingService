using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.IO.Compression;

namespace MyFileMovingService.Logic
{
    
    class UnzipperOfFiles : MainLogic
    {
        private static string zipFilePath = @"C:\Program Files\7-Zip\7zG.exe";
        Process zipProcess = new Process();
        

        public void Unzip(String pathName, string targetPath)
        {
            zipProcess.StartInfo.FileName = zipFilePath;
            zipProcess.StartInfo.Arguments = @" e " + pathName + @" -o" + folderPath;
            zipProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            zipProcess.Start();
            MyServiceLogger.Log("Unziped File" + Path.GetFileName(pathName) + " At: " + DateTime.Now.TimeOfDay.ToString());
        }

    }
}
