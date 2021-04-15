using DotNetCore_RobotFrameWork.Communicate.Caching;
using DotNetCore_RobotFrameWork.Communicate.Common;
using DotNetCore_RobotFrameWork.Communicate.Logging;
using DotNetCore_RobotFrameWork.Communicate.TestData;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetCore_RobotFrameWork.Communicate.WebApp
{
    public class WebAppBase
    {
        #region Messages

        protected const string Msg_Default = "None";
        protected const string Msg_Element_Not_Exist_Or_Invisible = "The element '{0}' not exist or invisible.";
        protected const string Msg_ElementShouldContains = "The expected value '{0}' and actual value '{1}' does not match.";
        protected const string Msg_Element_Not_Enable = "The element '{0}' not enable.";
        protected const string Msg_Element_Not_Visible = "The element '{0}' not visiable.";

        #endregion

        protected IWebDriver _webDriver;

        public WebAppBase(string connectionString)
        {
            try
            {
                LoadSetting(connectionString);
                Logger.Init("Communicate.Log");
                //todo _webDriver
            }
            catch (Exception ex)
            {
                HandlingException(ex);
            }
        }

        protected void LoadSetting(string connectionString)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            AppSetting.Instance.ConnectionString = connectionString;
        }

        protected void HandlingException(Exception ex, string messasge = "")
        {
            //write log to tracking
            WriteLog(messasge, ex);

            if (ex is NullReferenceException)
                throw new NullReferenceException(string.IsNullOrEmpty(messasge) ? ex.Message : messasge);
            else
                throw ex;
        }

        protected string GetTextData(string key, bool ignore_case = true)
        {
            return TestDataHelper.GetValueByKey(key, ignore_case);
        }
        protected void WriteLog(string message, Exception exception)
        {
            
            if (exception == null)
            {
                Console.WriteLine(message);
                Logger.Log(message);
            }
            else
            {
                LogError(exception);
            }
        }

        protected void LogError(Exception exception)
        {
            Console.WriteLine(exception.Message);
            Logger.Log(exception.Message);
            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                Console.WriteLine(exception.StackTrace);
                Logger.Log(exception.StackTrace);
            }
            //inner exception
            if(exception.InnerException != null)
            {
                Console.WriteLine(exception.InnerException.Message);
                Logger.Log(exception.InnerException.Message);
            }
        }
        protected string FormatMessage(string customMessage, string defaultMessage)
        {
            if (customMessage == Msg_Default)
                return defaultMessage;
            return customMessage;
        }

        protected IWebElement FindElement(string locator, int timeout = -1, bool scroll = false)
        {
            var models = ParseLocator(locator);
            if (models == null)
                return null;

            if (models != null && models.Count == 1)
                return FindElementWithSingleStrategy(models.FirstOrDefault().By, timeout, scroll);

            return FindElementWithMultipleStrategy(models.Select(f => f.By).ToList(), timeout, scroll);
        }

        protected int CountElement(string locator, int timeout = -1, bool scroll = false)
        {
            var models = ParseLocator(locator);
            if (models == null)
                return 0;

            if (models != null && models.Count == 1)
                return CountElementWithSingleStrategy(models.FirstOrDefault().By, timeout, scroll);

            return CountElementWithMultipleStrategy(models.Select(f => f.By).ToList(), timeout, scroll);
        }

        protected IWebElement FindElementWithSingleStrategy(By by, int timeout = -1, bool scroll = false)
        {
            if (timeout != -1)
                _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeout);

            var element = _webDriver.FindElement(by);
            if (element != null)
                return element;

            if (scroll)
            {
                var jscommand = $"window.scroll(0, {element.Location.Y});";
                _webDriver.ExecuteJavaScript(jscommand);

                WebDriverWait wait = new WebDriverWait(_webDriver, _webDriver.Manage().Timeouts().ImplicitWait);
                return wait.Until((condition) =>
                {
                    element = condition.FindElement(by);
                    if (element.Displayed &&
                        element.Enabled &&
                        element.GetAttribute("aria-disabled") == null)
                    {
                        return element;
                    }
                    return null;
                });
            }

            return null;
        }

        protected int CountElementWithSingleStrategy(By by, int timeout = -1, bool scroll = false)
        {
            if (timeout != -1)
                _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeout);

            var elements = _webDriver.FindElements(by);
            if (elements != null)
                return elements.Count;

            if (scroll)
            {
                var jscommand = $"window.scroll(0, {elements.FirstOrDefault().Location.Y});";
                _webDriver.ExecuteJavaScript(jscommand);

                WebDriverWait wait = new WebDriverWait(_webDriver, _webDriver.Manage().Timeouts().ImplicitWait);
                return wait.Until((condition) =>
                {
                    elements = condition.FindElements(by);
                    if (elements.Any(f => f.Displayed &&
                            f.Enabled &&
                            f.GetAttribute("aria-disabled") == null))
                    {
                        return elements.Count;
                    }
                    return 0;
                });
            }

            return 0;
        }

        protected IWebElement FindElementWithMultipleStrategy(List<By> bies, int timeout = -1, bool scroll = false)
        {
            if (bies == null)
                return null;

            foreach (var by in bies)
            {
                try
                {
                    return FindElementWithSingleStrategy(by, timeout, scroll);
                }
                catch (NoSuchElementException)
                {
                    continue;
                }
            }

            return null;
        }

        protected int CountElementWithMultipleStrategy(List<By> bies, int timeout = -1, bool scroll = false)
        {
            if (bies == null)
                return 0;

            foreach (var by in bies)
            {
                try
                {
                    return CountElementWithSingleStrategy(by, timeout, scroll);
                }
                catch (NoSuchElementException)
                {
                    continue;
                }
            }

            return 0;
        }

        protected IList<LocatorModel> ParseLocator(string locator)
        {
            var locators = new List<LocatorModel>();
            var arrLocators = locator.Split(Constants.LocatorDelimiter);
            foreach (var locatorTemp in arrLocators)
            {
                var arr = locatorTemp.Split(Constants.StrategyDelimiter);
                var model = new LocatorModel();
                if (arr.Length == 2)
                {
                    model.Strategy = Utility.ParseEnum<FindStrategy>(arr[0]);
                    model.Locator = arr[1];
                }
                else if (arr.Length > 2)
                {
                    model.Strategy = Utility.ParseEnum<FindStrategy>(arr[0]);
                    model.Locator = locatorTemp.Replace($"{arr[0]}{Constants.StrategyDelimiter}", "");
                }
                else
                {
                    model.Strategy = FindStrategy.Id; //default
                    model.Locator = arr[0];
                }
                model.By = GetBy(model.Strategy, model.Locator);
                locators.Add(model);
            }

            return locators;
        }

        protected By GetBy(FindStrategy strategy, string locator)
        {
            if (strategy == FindStrategy.ClassName)
                return By.ClassName(locator);
            else if (strategy == FindStrategy.Css)
                return By.CssSelector(locator);
            else if (strategy == FindStrategy.Tag)
                return By.TagName(locator);
            else if (strategy == FindStrategy.Id)
                return By.Id(locator);
            else if (strategy == FindStrategy.Link)
                return By.LinkText(locator);
            else if (strategy == FindStrategy.Name)
                return By.Name(locator);
            else if (strategy == FindStrategy.Partial)
                return By.PartialLinkText(locator);
            else if (strategy == FindStrategy.XPath)
                return By.XPath(locator);

            throw new Exception($"Locator {locator} does not support.");
        }

        protected string GetKeys(string keys_name)
        {
            string keys = "";
            var arr = keys_name.Split('+');
            foreach (var key in arr)
                keys += $"{GetKey(key)}+";
            return keys.TrimEnd('+');
        }

        protected string GetKey(string key_name)
        {
            switch(key_name)
            {
                case "Enter":
                    return Keys.Enter;
                case "NumberPad0":
                    return Keys.NumberPad0;
                case "NumberPad1":
                    return Keys.NumberPad1;
                case "NumberPad2":
                    return Keys.NumberPad2;
                case "NumberPad3":
                    return Keys.NumberPad3;
                case "NumberPad4":
                    return Keys.NumberPad4;
                case "NumberPad5":
                    return Keys.NumberPad5;
                case "NumberPad6":
                    return Keys.NumberPad6;
                case "NumberPad7":
                    return Keys.NumberPad7;
                case "NumberPad8":
                    return Keys.NumberPad8;
                case "NumberPad9":
                    return Keys.NumberPad9;
                case "Multiply":
                    return Keys.Multiply;
                case "Add":
                    return Keys.Add;
                case "Separator":
                    return Keys.Separator;
                case "Equal":
                    return Keys.Equal;
                case "Subtract":
                    return Keys.Subtract;
                case "Divide":
                    return Keys.Divide;
                case "F7":
                    return Keys.F7;
                case "F8":
                    return Keys.F8;
                case "F9":
                    return Keys.F9;
                case "F10":
                    return Keys.F10;
                case "F11":
                    return Keys.F11;
                case "F12":
                    return Keys.F12;
                case "Decimal":
                    return Keys.Decimal;
                case "Meta":
                    return Keys.Meta;
                case "Semicolon":
                    return Keys.Semicolon;
                case "Insert":
                    return Keys.Insert;
                case "Cancel":
                    return Keys.Cancel;
                case "Help":
                    return Keys.Help;
                case "Backspace":
                    return Keys.Backspace;
                case "Tab":
                    return Keys.Tab;
                case "Clear":
                    return Keys.Clear;
                case "Return":
                    return Keys.Return;
                case "Shift":
                    return Keys.Shift;
                case "LeftShift":
                    return Keys.LeftShift;
                case "Control":
                    return Keys.Control;
                case "LeftControl":
                    return Keys.LeftControl;
                case "Alt":
                    return Keys.Alt;
                case "LeftAlt":
                    return Keys.LeftAlt;
                case "Delete":
                    return Keys.Delete;
                case "Pause":
                    return Keys.Pause;
                case "Space":
                    return Keys.Space;
                case "PageUp":
                    return Keys.PageUp;
                case "PageDown":
                    return Keys.PageDown;
                case "End":
                    return Keys.End;
                case "Home":
                    return Keys.Home;
                case "Left":
                    return Keys.Left;
                case "ArrowLeft":
                    return Keys.ArrowLeft;
                case "Up":
                    return Keys.Up;
                case "ArrowUp":
                    return Keys.ArrowUp;
                case "Right":
                    return Keys.Right;
                case "ArrowRight":
                    return Keys.ArrowRight;
                case "Down":
                    return Keys.Down;
                case "ArrowDown":
                    return Keys.ArrowDown;
                case "Escape":
                    return Keys.Escape;
                case "Command":
                    return Keys.Command;
                default:
                    return Keys.Null;
            }
        }

    }
}
