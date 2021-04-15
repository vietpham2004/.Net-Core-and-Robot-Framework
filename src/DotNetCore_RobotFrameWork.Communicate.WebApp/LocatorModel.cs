using OpenQA.Selenium;

namespace DotNetCore_RobotFrameWork.Communicate.WebApp
{
    public class LocatorModel
    {
        public FindStrategy Strategy { get; set; }
        public string Locator { get; set; }

        public By By { get; set; }

    }
}
