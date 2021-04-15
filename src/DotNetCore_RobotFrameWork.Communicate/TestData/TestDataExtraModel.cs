using System;
using System.Data;

namespace DotNetCore_RobotFrameWork.Communicate.TestData
{
    public class TestDataExtraModel : ICloneable
    {
        public string Key { get; set; }
        public DataTable DataExtend { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
