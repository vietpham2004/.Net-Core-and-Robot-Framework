using OpenQA.Selenium;

namespace DotNetCore_RobotFrameWork.Communicate.WebApp
{
    public static class Extensions
    {
        /// <summary>
		/// Execute javascript command
		/// </summary>
		/// <param name="webDriver"></param>
		/// <param name="javaScriptCommand"></param>
		/// <returns></returns>
		public static object ExecuteJavaScript(this IWebDriver webDriver, string javaScriptCommand)
        {
            return ((IJavaScriptExecutor)webDriver).ExecuteScript(javaScriptCommand);
        }
    }
}
