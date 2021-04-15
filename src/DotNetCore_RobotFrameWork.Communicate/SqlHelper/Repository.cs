using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DotNetCore_RobotFrameWork.Communicate.SqlHelper
{
    public class Repository
    {
        private AppDbContext _dataContext;
        public Repository(string connectionString)
        {
            _dataContext = new AppDbContext(connectionString);
        }

        public string GetJsonData (string commandText, int rowIndex = -1, int rowCount = -1)
        {
            using (var command = _dataContext.CreateCommand())
            {
                command.CommandText = commandText;
                command.CommandType = CommandType.Text;
                var reader = command.ExecuteReader();
                var obj = GetData(reader, rowIndex, rowCount);
                return JsonConvert.SerializeObject(obj, Formatting.Indented);
            }
        }

        public object GetSingleValue(string commandText)
        {
            using (var command = _dataContext.CreateCommand())
            {
                command.CommandText = commandText;
                command.CommandType = CommandType.Text;
                var reader = command.ExecuteReader();
                if (reader.HasRows & reader.Read())
                    return reader[0] == DBNull.Value ? null : reader[0];

                return null;
            }
        }

        public int ExecuteNonQuery(string commandText)
        {
            using (var command = _dataContext.CreateCommand())
            {
                command.CommandText = commandText;
                command.CommandType = CommandType.Text;
                return command.ExecuteNonQuery();
            }
        }

        public string Compare2Results(string commandText1, string commandText2)
        {
            string sqlCommand = $"{commandText1} except {commandText2}";
            return GetJsonData(sqlCommand);
        }

        private IEnumerable<Dictionary<string, object>> GetData(SqlDataReader reader, int rowIndex = -1, int rowCount = -1)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();

            for (var i = 0; i < reader.FieldCount; i++)
            {
                cols.Add(reader.GetName(i));
            }

            if (rowIndex == -1 && rowCount == -1)
            {
                while (reader.Read())
                {
                    results.Add(GetRow(cols, reader));
                }
            }
            else if (rowIndex != -1 && rowCount == -1)
            {
                int index = 0;

                while (reader.Read())
                {
                    index++;
                    if (index < rowIndex)
                        continue;

                    results.Add(GetRow(cols, reader));
                }
            }
            else if (rowIndex == -1 && rowCount != -1)
            {
                int index = 0;

                while (reader.Read())
                {
                    index++;
                    if (index > rowCount)
                        break;

                    results.Add(GetRow(cols, reader));
                }
            }
            else
            {
                int index = 0;

                while (reader.Read())
                {
                    index++;
                    if (index < rowIndex)
                        continue;

                    if (index > rowCount)
                        break;

                    results.Add(GetRow(cols, reader));
                }
            }

            return results;
        }

        private Dictionary<string, object> GetRow(IEnumerable<string> cols, SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();

            foreach (var col in cols)
            {
                object obj = reader[col];
                result.Add(col, obj);
            }
            return result;
        }
    }
}
