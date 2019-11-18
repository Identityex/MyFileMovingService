using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyFileMovingService.Logic
{
    //Main Class For Doing all the Logic
    public class MainLogic : Service1
    {
        public static string folderPath = @"G:\root\Completed_Downloads\";
        public static string gamePath = @"G:\Games\";
        public static string videoPath = @"G:\Videos\";
        public static string testFilePath = @"G:\Testing\";
        public static string zippedPath = @"G:\zipped\";
        public static string fileFullPath;
        public static string directoryPath;
        public static string fileName;
        public static string fileExt;
        private static string destinationPath;

        //Main function for Moving Files and other things
        public static void testingAndMoving(string pathName)
        {

            if (File.Exists(pathName))
            {
                fileName = Path.GetFileName(pathName);
                fileExt = Path.GetExtension(pathName);
                fileFullPath = Path.GetFullPath(pathName);
                directoryPath = Path.GetDirectoryName(pathName);

                destinationPath = findAndMoveTo(fileExt);

                MyServiceLogger.Log("Moved " + fileName + "to " + destinationPath + " At: " + DateTime.Now.TimeOfDay.ToString());

            }
            else
            {
                MyServiceLogger.Log("File Does Not Exist " + DateTime.Now.TimeOfDay.ToString());
            }
        }

        //Function to check file extension and move a file elsewhere depending on it's extension Name
        protected static string findAndMoveTo(string fileExtension)
        {
            bool attemptingMoveFile = true;
            string returnString = "";
            string destinationPath = "";
            FileInfo FI = new FileInfo(fileFullPath);
            while (attemptingMoveFile)
            {
                if (!isFileLocked(FI))
                {
                    try
                    {
                        switch (fileExtension)
                        {
                            case ".txt":
                                destinationPath = testFilePath;
                                returnString = testFilePath;
                                checkAndCreateDirectory(destinationPath);
                                moveAndDelete(fileFullPath, destinationPath + fileName);
                                attemptingMoveFile = false;
                                break;

                            case ".iso":
                                destinationPath = gamePath;
                                returnString = gamePath;
                                checkAndCreateDirectory(destinationPath);
                                moveAndDelete(fileFullPath, destinationPath + fileName);
                                attemptingMoveFile = false;
                                break;

                            case ".wbfs":
                                returnString = gamePath;
                                destinationPath = gamePath;
                                checkAndCreateDirectory(destinationPath);
                                moveAndDelete(fileFullPath, destinationPath + fileName);
                                attemptingMoveFile = false;
                                break;

                            case ".mp4":
                                returnString = videoPath;
                                destinationPath = videoPath;
                                checkAndCreateDirectory(destinationPath);
                                moveAndDelete(fileFullPath, destinationPath + fileName);
                                attemptingMoveFile = false;
                                break;

                            case ".flv":
                                returnString = videoPath;
                                destinationPath = videoPath;
                                checkAndCreateDirectory(destinationPath);
                                moveAndDelete(fileFullPath, destinationPath + fileName);
                                attemptingMoveFile = false;
                                break;

                            case ".7z":
                                unZip(fileFullPath, directoryPath);
                                checkAndCreateDirectory(destinationPath);
                                returnString = "File was Zipped Ergo Unzipped";
                                attemptingMoveFile = false;
                                break;

                            default:
                                destinationPath = testFilePath;
                                returnString = "Case Went to Default";
                                checkAndCreateDirectory(destinationPath);
                                moveAndDelete(fileFullPath, destinationPath + fileName);
                                attemptingMoveFile = false;
                                break;
                        }
                        return returnString;
                    }
                    catch (Exception e)
                    {
                        MyServiceLogger.Log(e.ToString() + DateTime.Now.ToString());
                        return returnString;
                    }
                }
                else
                {
                    return returnString;
                }
            }
            return returnString;
        }

        //Check if Directory exists if not create
        protected static void checkAndCreateDirectory(String directoryPath)
        {
            if(!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        //Move and Delete 
        protected static void moveAndDelete(String filePath, String destPath)
        {
            if (File.Exists(filePath))
            {
                File.Move(filePath, destPath);
                File.Delete(filePath);
            }
        }

        public static bool isFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch(IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }

        // Function to unzip a file if zipped Using 7-Zip (CALL-TO)
        protected static void unZip(string filePath, string fileDirectory)
        {
            UnzipperOfFiles unzipMyFile = new UnzipperOfFiles();
            unzipMyFile.Unzip(filePath, fileDirectory);
        }
    }
}
