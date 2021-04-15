using System.IO;
using System.Text;

namespace DotNetCore_RobotFrameWork.Communicate.IO
{
    public static class ReadFileHelper
    {
        public static string ReadFile(string pathFile)
        {
            return File.ReadAllText(pathFile, Encoding.UTF8);
        }
    }
}
