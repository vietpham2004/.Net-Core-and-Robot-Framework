using NUnit.Framework;
using System.IO;

namespace DotNetCore_RobotFrameWork.Communicate.WebApp.Tests
{
    public abstract class DriverTestFixture
    {
        protected KeyWords _wrapperkeyWords;

        protected KeyWords KeyWordsInstance
        {
            get { return _wrapperkeyWords; }
            set { _wrapperkeyWords = value; }
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            var database = Path.Combine(GetAbsolutePath("LocalDB"), "CommunicateTestDB.mdf");
            var connString = $"Data Source = (LocalDB)\\MSSQLLocalDB;AttachDbFilename={database};Integrated Security = True; Connect Timeout = 300";

            _wrapperkeyWords = new KeyWords(connString);
            _wrapperkeyWords.open_app();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _wrapperkeyWords.close_app();
        }

        protected string GetAbsolutePath(string folderName)
        {
            string dbPathFile = $@"..\..\..\{folderName}\";
            return new DirectoryInfo(dbPathFile).FullName;
        }
    }
}
