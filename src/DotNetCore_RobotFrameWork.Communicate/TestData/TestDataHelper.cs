using DotNetCore_RobotFrameWork.Communicate.Caching;
using DotNetCore_RobotFrameWork.Communicate.Common;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNetCore_RobotFrameWork.Communicate.TestData
{
    public static class TestDataHelper
    {
        public static TestSuiteDataModel TestSuitData { get; set; }
        public static List<TestDataModel> TestPlanData { get; set; }
        public static List<TestDataModel> SharedData { get; set; }
        public static TestCaseDataModel CurrentTestCaseData { get; set; }

        public static AppSetting AppSettingCached { get; set; }

        private const string ExtraDataKey = "ExtraData";
        private const string TestCaseNameKey = "TestName";
        private const string TestCaseIdKey = "TestCaseId";
        private const string TestSuiteIdKey = "TestSuiteId";

        public static void LoadTestPlanData(string fileName, string sheetName)
        {
            TestPlanData = new List<TestDataModel>();
            LoadDataBySheetName(fileName, sheetName, TestDataType.TestPlan);
        }
        public static void LoadShareData(string fileName, string sheetName)
        {
            SharedData = new List<TestDataModel>();
            LoadDataBySheetName(fileName, sheetName, TestDataType.Share);
        }

        public static void LoadTestSuiteData(string fileName, string sheetName)
        {
            TestSuitData = new TestSuiteDataModel();
            LoadDataBySheetName(fileName, sheetName, TestDataType.TestSuite);
        }

        public static void StartTestCase(string testCaseId)
        {
            CurrentTestCaseData = TestSuitData.TestCaseDataList.Where(f => f.TestCaseId == testCaseId).Select(f => f).FirstOrDefault();
        }

        private static void LoadDataBySheetName(string fileName, string sheetName, TestDataType type)
        {
            FileStream stream = null;
            IExcelDataReader excelReader = null;
            try
            {
                FileInfo fileInfo = new FileInfo(fileName);
                var ext = fileInfo.Extension;
                stream = File.Open(fileName, FileMode.Open, FileAccess.Read);

                if (ext.Equals(".xls", StringComparison.CurrentCultureIgnoreCase))
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                else if (ext.Equals(".csv", StringComparison.CurrentCultureIgnoreCase))
                    excelReader = ExcelReaderFactory.CreateCsvReader(stream);
                else
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                
                var rows = excelReader.AsDataSet().Tables[sheetName].Rows;
                //==test plan
                if (type == TestDataType.TestPlan)
                {
                    for (int i = 0; i < rows.Count; i++)
                    {
                        var key = rows[i].ItemArray.GetValue(0).ToString();
                        var value = rows[i].ItemArray.GetValue(1).ToString();
                        TestPlanData.Add(new TestDataModel { Key = key, Value = value });
                    }
                    return;
                }
                else if (type == TestDataType.Share) //==share data --> skip the 1st row for header
                {
                    for (int i = 1; i < rows.Count; i++)
                    {
                        var key = rows[i].ItemArray.GetValue(0).ToString();
                        var value = rows[i].ItemArray.GetValue(1).ToString();
                        SharedData.Add(new TestDataModel { Key = key, Value = value });
                    }
                    return;
                }
                else if (type == TestDataType.TestSuite)//==test suite
                {
                    TestCaseDataModel testCaseData = null;
                    TestDataExtraModel extraData = null;
                    for (int i = 0; i < rows.Count; i++)
                    {
                        extraData = null;

                        var key = rows[i].ItemArray.GetValue(0).ToString();
                        if (string.IsNullOrEmpty(key))
                            continue;

                        var value = rows[i].ItemArray.GetValue(1) == null ? string.Empty : rows[i].ItemArray.GetValue(1).ToString();

                        if (key.EqualsText(TestCaseNameKey, true))
                        {
                            if (testCaseData == null) //==begin a test case
                                testCaseData = new TestCaseDataModel();
                            else//next test case
                            {
                                if (TestSuitData.TestCaseDataList == null)
                                    TestSuitData.TestCaseDataList = new List<TestCaseDataModel>();

                                TestSuitData.TestCaseDataList.Add(testCaseData.Clone() as TestCaseDataModel);
                                testCaseData = new TestCaseDataModel();
                                continue;
                            }
                        }
                        else if (key.Contains(ExtraDataKey))
                        {
                            if (string.IsNullOrEmpty(value))
                                continue;

                            string extraSheetName = value.Split('!').ToList().FirstOrDefault();
                            extraData = new TestDataExtraModel { Key = key, DataExtend = excelReader.AsDataSet().Tables[extraSheetName] };

                            if (testCaseData.DataExtendList == null)
                                testCaseData.DataExtendList = new List<TestDataExtraModel>();

                            testCaseData.DataExtendList.Add(extraData.Clone() as TestDataExtraModel);
                            extraData = null;
                            continue;
                        }

                        if (testCaseData == null && string.IsNullOrEmpty(TestSuitData.TestSuiteId)) //==test suite info
                        {
                            if (key.EqualsText(TestSuiteIdKey, true))
                                TestSuitData.TestSuiteId = value;

                            if (TestSuitData.DataList == null)
                                TestSuitData.DataList = new List<TestDataModel>();

                            TestSuitData.DataList.Add(new TestDataModel { Key = key, Value = value });
                        }
                        else if (testCaseData != null)
                        {
                            if (testCaseData.DataList == null)
                                testCaseData.DataList = new List<TestDataModel>();

                            if (key.EqualsText(TestCaseIdKey, true))
                                testCaseData.TestCaseId = value;

                            testCaseData.DataList.Add(new TestDataModel { Key = key, Value = value });
                        }
                    }
                    //the latest test case
                    if (testCaseData != null && testCaseData.TestCaseId != string.Empty)
                    {
                        if (TestSuitData.TestCaseDataList == null)
                            TestSuitData.TestCaseDataList = new List<TestCaseDataModel>();
                        TestSuitData.TestCaseDataList.Add(testCaseData);
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                excelReader.Close();
                stream.Close();
            }
        }

        public static string GetValueByKey(string key, string testDataType, bool ignoreCase = true)
        {
            if (key == testDataType) return key;
            TestDataType type = Utility.ParseEnum<TestDataType>(testDataType);

            if (type == TestDataType.Share)
                return SharedData.Where(f => f.Key.EqualsText(key, ignoreCase)).Select(f => f.Value).FirstOrDefault();
            else if (type == TestDataType.TestPlan)
                return TestPlanData.Where(f => f.Key.EqualsText(key, ignoreCase)).Select(f => f.Value).FirstOrDefault();
            else if (type == TestDataType.TestSuite)
                return TestSuitData.DataList.Where(f => f.Key.EqualsText(key, ignoreCase)).Select(f => f.Value).FirstOrDefault();
            else if (type == TestDataType.TestCase)
                return CurrentTestCaseData.DataList.Where(f => f.Key.EqualsText(key, ignoreCase)).Select(f => f.Value).FirstOrDefault();

            return key;
        }

        public static string GetValueByKey(string key, bool ignoreCase = true)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            var arr = key.Split('.').ToList();
            var keyTmp = key;
            var testDataType = key;
            if (arr.Count == 2)
            {
                testDataType = arr[0];
                key = arr[1];
                if (!Utility.CheckExistEnum<TestDataType>(testDataType))
                    key = testDataType = keyTmp;
            }

            return GetValueByKey(key, testDataType, ignoreCase);
        }
        
    }
}
