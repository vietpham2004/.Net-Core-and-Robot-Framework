using System;

namespace DotNetCore_RobotFrameWork.Communicate.Caching
{
    public class AppSetting
    {
        public string ConnectionString { set; get; }
        public DateTime TestRunStartTime { get; set; }

        private static AppSetting _instance;
        /// <summary>
        /// Instance
        /// </summary>
        public static AppSetting Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AppSetting();
                return _instance;
            }
        }
    }
}
