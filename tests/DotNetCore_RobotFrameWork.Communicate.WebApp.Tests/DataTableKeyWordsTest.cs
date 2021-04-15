using NUnit.Framework;

namespace DotNetCore_RobotFrameWork.Communicate.WebApp.Tests
{
    public class DataTableKeyWordsTest : DriverTestFixture
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void get_data_in_cell_test()
        {
            KeyWordsInstance.navigate("https://selenium.dev/documentation/en/getting_started_with_webdriver/third_party_drivers_and_plugins/");
            var actual = KeyWordsInstance.get_data_in_cell("XPath:://*[@id='body-inner']/table", 4, "Browser");
            var exptected = "Microsoft Edge Driver";
            Assert.AreEqual(actual, exptected);
        }

        [Test]
        public void verify_data_in_cell_test()
        {
            KeyWordsInstance.navigate("https://selenium.dev/documentation/en/getting_started_with_webdriver/third_party_drivers_and_plugins/");
            var exptected = "Microsoft Edge Driver";
            var actual = KeyWordsInstance.verify_data_in_cell("XPath:://*[@id='body-inner']/table", exptected, 4, "Browser");
            Assert.AreEqual(actual, true);
        }
    }
}