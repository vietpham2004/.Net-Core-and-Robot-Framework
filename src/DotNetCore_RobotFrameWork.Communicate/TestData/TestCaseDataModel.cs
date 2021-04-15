using System;
using System.Collections.Generic;

namespace DotNetCore_RobotFrameWork.Communicate.TestData
{
    public class TestCaseDataModel : ICloneable
    {
        public string TestCaseId { get; set; }
        //public string Name { get; set; }

        public List<TestDataModel> DataList { get; set; }
        public List<TestDataExtraModel> DataExtendList { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
