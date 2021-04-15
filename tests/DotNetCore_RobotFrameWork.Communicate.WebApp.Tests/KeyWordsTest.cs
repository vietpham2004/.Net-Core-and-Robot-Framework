using NUnit.Framework;
using System.IO;

namespace DotNetCore_RobotFrameWork.Communicate.WebApp.Tests
{
    public class KeyWordsTest : DriverTestFixture
    {
        [SetUp]
        public void Setup()
        {
            var testDataPath = Path.Combine(GetAbsolutePath("TestData"), "TestDataDemo.xlsx");
            KeyWordsInstance.load_test_data(testDataPath, "TestSuiteDemo", "TestSuite");
        }

        [Test]
        public void input_text_test()
        {
            KeyWordsInstance.navigate("https://www.google.com.vn/");
            KeyWordsInstance.start_test_case("12345");
            var actual = KeyWordsInstance.input_text("Name::q", "TestCase.SearchText");
            KeyWordsInstance.click("Name::btnK");
            Assert.IsEmpty(actual);
        }
    }
}