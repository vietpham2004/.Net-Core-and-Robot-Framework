using DotNetCore_RobotFrameWork.Communicate.Caching;

namespace DotNetCore_RobotFrameWork.Communicate.SqlHelper
{
    public class SqlDataHelper
    {
        private static Repository _instance;
        public static Repository Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Repository(AppSetting.Instance.ConnectionString);
                return _instance;
            }
        }
    }
}
