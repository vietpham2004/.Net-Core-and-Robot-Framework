using DotNetCore_RobotFrameWork.Communicate.Caching;
using DotNetCore_RobotFrameWork.Communicate.Common;
using DotNetCore_RobotFrameWork.Communicate.SqlHelper;
using System;

namespace DotNetCore_RobotFrameWork.Communicate.WebApp
{
    public partial class KeyWords
    {
        public void switch_connection_string(string connection_string)
        {
            AppSetting.Instance.ConnectionString = connection_string;
        }

        public int get_total_row(string commandText)
        {
            try
            {
                var count = SqlDataHelper.Instance.GetSingleValue(commandText);
                return count == null ? 0 : count.ToInt();
            }
            catch (Exception ex)
            {
                HandlingException(ex);
            }
            return 0;
        }

        public string get_json_data(string commandText)
        {
            try
            {
                return SqlDataHelper.Instance.GetJsonData(commandText);
            }
            catch (Exception ex)
            {
                HandlingException(ex);
            }
            return string.Empty;
        }

        public string get_single_value(string commandText)
        {
            try
            {
                var value = SqlDataHelper.Instance.GetSingleValue(commandText);
                return value == null ? string.Empty : value.ToString();
            }
            catch (Exception ex)
            {
                HandlingException(ex);
            }
            return string.Empty;
        }

        public void execute_sql_command(string commandText)
        {
            try
            {
                SqlDataHelper.Instance.ExecuteNonQuery(commandText);
            }
            catch (Exception ex)
            {
                HandlingException(ex);
            }
        }

        public string compare_2_results(string commandText1, string commandText2)
        {
            try
            {
                return SqlDataHelper.Instance.Compare2Results(commandText1, commandText2);
            }
            catch (Exception ex)
            {
                HandlingException(ex);
            }
            return string.Empty;
        }
    }
}
