using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore_RobotFrameWork.Communicate.WebApp.Tests
{
    public class DatabaseKeyWordsTest : DriverTestFixture
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void get_single_value_test()
        {
            var username = "mem001";
            var actual = KeyWordsInstance.get_single_value($"select firstName from dbo.Customer where username = '{username}'");
            var exptected = "David";
            Assert.AreEqual(actual, exptected);
        }

        [Test]
        public void get_json_data_test()
        {
            var actual = KeyWordsInstance.get_json_data($"select * from dbo.Customer");
            Assert.NotNull(actual);
        }
    }
}
