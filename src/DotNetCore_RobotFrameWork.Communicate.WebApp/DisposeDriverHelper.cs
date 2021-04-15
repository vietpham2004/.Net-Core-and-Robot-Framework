using DotNetCore_RobotFrameWork.Communicate.Caching;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DotNetCore_RobotFrameWork.Communicate.WebApp
{
    public partial class KeyWords
    {
        private readonly List<string> _processesToCheck =
        new List<string>
        {
            "opera",
            "chrome",
            "firefox",
            "ie",
            "gecko",
            "phantomjs",
            "edge",
            "microsoftwebdriver",
            "webdriver"
        };

        public void dispose_web_driver()
        {
            _webDriver?.Dispose();
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                try
                {
                    Debug.WriteLine(process.ProcessName);
                    if (process.StartTime > AppSetting.Instance.TestRunStartTime)
                    {
                        var shouldKill = false;
                        foreach (var processName in _processesToCheck)
                        {
                            if (process.ProcessName.ToLower().Contains(processName))
                            {
                                shouldKill = true;
                                break;
                            }
                        }
                        if (shouldKill)
                        {
                            process.Kill();
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }
    }
}
