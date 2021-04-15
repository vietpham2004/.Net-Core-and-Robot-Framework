using DotNetCore_RobotFrameWork.Communicate.Common;
using OpenQA.Selenium;
using System;
using System.Linq;

namespace DotNetCore_RobotFrameWork.Communicate.WebApp
{
    public partial class KeyWords
    {
        const string TagNameRowHeader = "th";
        const string TagNameRow = "tr";
        const string TagNameCell = "td";

        public string get_data_in_cell(string locator, int row_index, string column_name, int timeout = -1, bool scroll = false)
        {
            try
            {
                return GetTextInCell(locator, row_index, column_name, timeout, scroll);
            }
            catch (Exception ex)
            {
                HandlingException(ex);
                return ex.Message;
            }
        }

        public bool verify_data_in_cell(string locator, string expected_text_or_key, int row_index, string column_name, bool ignore_case = true, int timeout = -1, bool scroll = false)
        {
            try
            {
                var textValue = GetTextData(expected_text_or_key, ignore_case);
                var actualValue = GetTextInCell(locator, row_index, column_name, timeout, scroll);
                return textValue.EqualsText(actualValue, ignore_case);
            }
            catch (Exception ex)
            {
                HandlingException(ex);
                return false;
            }
        }

        private string GetTextInCell(string locator, int row_index, string column_name, int timeout = -1, bool scroll = false)
        {
            var table = FindElement(locator, timeout, scroll);
            var columns = table.FindElements(By.TagName(TagNameRowHeader)).Select(f => f.Text).ToArray();
            var colIndex = Array.IndexOf(columns, column_name);
            var row = table.FindElements(By.TagName(TagNameRow)).Skip(row_index).FirstOrDefault();
            return row.FindElements(By.TagName(TagNameCell)).Skip(colIndex).Select(f => f.Text).FirstOrDefault();
        }
    }
}
