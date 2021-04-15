using System.Collections.Generic;

namespace DotNetCore_RobotFrameWork.Communicate.TestData
{
    public class TestSuiteDataModel
    {
        public string TestSuiteId { get; set; }
        public List<TestDataModel> DataList { get; set; }
        public List<TestCaseDataModel> TestCaseDataList { get; set; }
    }
}
