using DotNetCore_RobotFrameWork.Communicate.Common;
using DotNetCore_RobotFrameWork.Communicate.TestData;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using System;
using System.IO;
using System.Reflection;

namespace DotNetCore_RobotFrameWork.Communicate.WebApp
{
    public partial class KeyWords : WebAppBase
    {
        
        public KeyWords(string connectionString) : base(connectionString)
        {
            
        }

        #region application
        public void open_app(string browser_name = "Chrome", string url = "")
        {
            BrowserName browserNameEnum = Utility.ParseEnum<BrowserName>(browser_name);
            var pathExe = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (browserNameEnum == BrowserName.Chrome)
                _webDriver = new ChromeDriver(pathExe);
            else if (browserNameEnum == BrowserName.Edge)
                _webDriver = new EdgeDriver(pathExe);
            else if (browserNameEnum == BrowserName.Firefox)
                _webDriver = new FirefoxDriver(pathExe);
            if (!string.IsNullOrEmpty(url))
                _webDriver.Navigate().GoToUrl(url);
        }

        public void close_app()
        {
            _webDriver.Close();
            _webDriver.Quit();
            _webDriver.Dispose();
        }

        public void navigate(string url)
        {
            _webDriver.Navigate().GoToUrl(url);
        }

        #endregion

        #region Handle Element
        /// <summary>
        /// get text data from control via locator
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="timeout"></param>
        /// <param name="scroll"></param>
        /// <returns></returns>
        public string get_text(string locator, int timeout = -1, bool scroll = false)
        {
            try
            {
                var webElement = FindElement(locator, timeout, scroll);
                return webElement.Text;
            }
            catch (Exception ex)
            {
                HandlingException(ex);
                return FormatMessage(Msg_Default, ex.Message);
            }
        }

        /// <summary>
        /// input text data into control via locator
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="expected_text_or_key">expected text value or key value on data</param>
        /// <param name="timeout"></param>
        /// <param name="scroll"></param>
        /// <returns></returns>
        public string input_text(string locator, string expected_text_or_key, bool ignore_case = true, int timeout = -1, bool scroll = false)
        {
            try
            {
                var value = GetTextData(expected_text_or_key, ignore_case);
                var webElement = FindElement(locator, timeout, scroll);
                webElement.SendKeys(value);
                return string.Empty;
            }
            catch (Exception ex)
            {
                HandlingException(ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// perform click on control via locator
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="timeout"></param>
        /// <param name="scroll"></param>
        /// <returns></returns>
        public string click(string locator, int timeout = -1, bool scroll = false)
        {
            try
            {
                var webElement = FindElement(locator, timeout, scroll);
                webElement.Click();
                return string.Empty;
            }
            catch (Exception ex)
            {
                HandlingException(ex);
                return ex.Message;
            }
        }

        public bool equals_text(string locator, string expected_text_or_key, bool ignore_case = true, int timeout = -1, bool scroll = false)
        {
            try
            {
                var value = GetTextData(expected_text_or_key, ignore_case);
                var webElement = FindElement(locator, timeout, scroll);
                return webElement.Text.EqualsText(value, ignore_case);
            }
            catch (Exception ex)
            {
                HandlingException(ex);
                return false;
            }
        }

        public bool not_equals_text(string locator, string expected_text_or_key, bool ignore_case = true, int timeout = -1, bool scroll = false)
        {
            try
            {
                var value = GetTextData(expected_text_or_key, ignore_case);
                var webElement = FindElement(locator, timeout, scroll);
                return webElement.Text.NotEqualsText(value, ignore_case);
            }
            catch (Exception ex)
            {
                HandlingException(ex);
                return false;
            }
        }

        public bool contains_text(string locator, string expected_text_or_key, bool ignore_case = true, int timeout = -1, bool scroll = false)
        {
            try
            {
                var value = GetTextData(expected_text_or_key, ignore_case);
                var webElement = FindElement(locator, timeout, scroll);
                return webElement.Text.ContainsText(value, ignore_case);
            }
            catch (Exception ex)
            {
                HandlingException(ex);
                return false;
            }
        }

        public bool not_contains_text(string locator, string expected_text_or_key, bool ignore_case = true, int timeout = -1, bool scroll = false)
        {
            try
            {
                var value = GetTextData(expected_text_or_key, ignore_case);
                var webElement = FindElement(locator, timeout, scroll);
                return webElement.Text.ContainsText(value, ignore_case);
            }
            catch (Exception ex)
            {
                HandlingException(ex);
                return false;
            }
        }

        public void press_a_key(string locator, string key_name, bool ignore_case = true, int timeout = -1, bool scroll = false)
        {
            try
            {
                var value = GetTextData(key_name, ignore_case);
                var webElement = FindElement(locator, timeout, scroll);
                webElement.SendKeys(key_name);
            }
            catch (Exception ex)
            {
                HandlingException(ex);
            }
        }

        public bool has_visisble(string locator, int timeout = -1, bool scroll = false)
        {
            try
            {
                var webElement = FindElement(locator, timeout, scroll);
                if (webElement == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                HandlingException(ex);
            }
            return false;
        }

        public bool has_enable (string locator, int timeout = -1, bool scroll = false)
        {
            try
            {
                var webElement = FindElement(locator, timeout, scroll);
                if (webElement == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                HandlingException(ex);
            }
            return false;
        }

        public void scroll_to_view(string locator, int timeout = -1, bool scroll = false)
        {
            try
            {
                var webElement = FindElement(locator, timeout, scroll);
                Actions actions = new Actions(_webDriver);
                actions.MoveToElement(webElement);
                actions.Perform();
            }
            catch (Exception ex)
            {
                HandlingException(ex);
            }
        }
        public int count(string locator, int timeout = -1, bool scroll = false)
        {
            try
            {
                return CountElement(locator, timeout, scroll);
            }
            catch (Exception ex)
            {
                HandlingException(ex);
            }
            return 0;
        }

        #endregion

        #region Test data

        /// <summary>
        /// load test data to cached
        /// </summary>
        /// <param name="path_file"></param>
        /// <param name="sheet_name"></param>
        /// <param name="test_data_type: TestPlan, TestSuite, Share"></param>
        public void load_test_data(string path_file, string sheet_name, string test_data_type)
        {
            try
            {
                TestDataType type = Utility.ParseEnum<TestDataType>(test_data_type);
                if (type == TestDataType.TestPlan)
                    TestDataHelper.LoadTestPlanData(path_file, sheet_name);
                else if (type == TestDataType.TestSuite)
                    TestDataHelper.LoadTestSuiteData(path_file, sheet_name);
                else if (type == TestDataType.Share)
                    TestDataHelper.LoadShareData(path_file, sheet_name);
            }
            catch (Exception ex)
            {
                HandlingException(ex);
            }
        }

        public void start_test_case(string test_caseId)
        {
            try
            {
                TestDataHelper.StartTestCase(test_caseId);
            }
            catch (Exception ex)
            {
                HandlingException(ex);
            }
        }

        public string get_test_data_value_by_key(string key)
        {
            try
            {
                return TestDataHelper.GetValueByKey(key);
            }
            catch (Exception ex)
            {
                HandlingException(ex);
            }
            return string.Empty;
        }

        #endregion
    }
}
