using System;
using System.IO;

namespace DotNetCore_RobotFrameWork.Communicate.Logging
{
    public static class Logger
    {
        const string DEFAULT_PATH_FILE = @".";
        private static string loggerName;
        private static object objLock = new object();
        private static string pathFile;
        public static void Init(string loggerNameValue)
        {
            lock (objLock)
            {
                loggerName = loggerNameValue;
                pathFile = new DirectoryInfo(DEFAULT_PATH_FILE).FullName;
                pathFile = Path.Combine(pathFile, "Logs");
                if (!Directory.Exists(pathFile))
                    Directory.CreateDirectory(pathFile);
            }
        }
        public static void Log(string message)
        {
            WriteLog(string.Format("[{0} - {3:yyyy-MM-dd HH:mm:ss zzz}] {1} - {2}", loggerName, "Info", message, DateTime.Now));
        }
        public static void Log(Exception exception)
        {
            WriteLog(string.Format("[{0} - {5:yyyy-MM-dd HH:mm:ss zzz}] {1} - {2}: {3} {4} --// {6}", loggerName, "Error",
                exception.GetType().FullName, exception.Message, exception.StackTrace, DateTime.Now, exception.InnerException != null ? exception.InnerException.Message + " ^ " + exception.InnerException.StackTrace : string.Empty));
        }

        private static void WriteLog(string message)
        {
            lock (objLock)
            {
                string fileName = string.Format("Log-{0}{1}", DateTime.Today.ToString("yyyy-MM-dd"), ".txt");
                var file = Path.Combine(pathFile, fileName);
                if (!File.Exists(file))
                {
                    File.Create(file).Dispose();
                }
                using (StreamWriter sw = File.AppendText(file))
                {
                    sw.WriteLine(message);
                    sw.Flush();
                    sw.Close();
                }
            }
        }
    }
}
