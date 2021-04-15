using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace DotNetCore_RobotFrameWork.Communicate.GenerateLib
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateCode();
        }
        static void GenerateCode()
        {
            StringBuilder pyClass = new StringBuilder();
            var ignoreMethods = new List<string>{ "GetType", "ToString", "Equals", "GetHashCode" };
            var header = @"import clr
import types
import sys, os
sys.path.append(os.path.join(os.path.dirname(__file__), '..', '..','SharedLib', 'netcoreapp3.0'))
clr.AddReference('DotNetCore_RobotFrameWork.Communicate.WebApp')
from DotNetCore_RobotFrameWork.Communicate.WebApp import KeyWords

class CommunicateLib(object):

    def __init__(self, connection_string):
        self.Communicate_Lib = KeyWords(connection_string)";

            pyClass.AppendFormat(header);

            pyClass.AppendLine(Environment.NewLine);
            string pathFile = @"..\SharedLib\netcoreapp3.0";
            //pathFile = @"..\..\..\..\..\CodedUILib";

            var absolutePath = new DirectoryInfo(pathFile).FullName;
            var dll = Assembly.LoadFile(Path.Combine(absolutePath, "DotNetCore_RobotFrameWork.Communicate.WebApp.dll"));

            Type methods = dll.GetType("DotNetCore_RobotFrameWork.Communicate.WebApp.KeyWords"); ;

            var methodHeader = @"    def {0}(self{1}): {2}{3} {4}self.Communicate_Lib.{0}({5})";
            foreach (var method in methods.GetMethods())
            {
                if (ignoreMethods.Contains(method.Name))
                    continue;

                string paramList = "", paramList1 = "";
                foreach (var param in method.GetParameters())
                {
                    var defaultValue = param.DefaultValue.ToString() == "" ? "''" : param.DefaultValue;
                    if (param.ParameterType.Name == "String" && param.DefaultValue.ToString() != "")
                        defaultValue = "'" + defaultValue + "'";
                    var p = param.HasDefaultValue ? param.Name + " = " + defaultValue : param.Name;
                    paramList = paramList + ", " + p;

                    var p2 = "";
                    if (param.ParameterType.Name == "Boolean")
                        p2 = param.Name;
                    else if (param.ParameterType.Name == "String")
                        p2 = "str(" + param.Name + ")";
                    else if (param.ParameterType.Name == "Int32")
                        p2 = "int(" + param.Name + ")";
                    else if (param.ParameterType.Name == "Double" || param.ParameterType.Name == "Single")
                        p2 = "float(" + param.Name + ")";

                    paramList1 = paramList1 + ", " + p2;
                }

                if (!string.IsNullOrEmpty(paramList))
                    paramList1 = paramList1.Substring(2, paramList1.Length - 2);

                var noReturnValue = method.ReturnParameter.ParameterType.Name;
                pyClass.AppendFormat(methodHeader, method.Name, paramList, Environment.NewLine, "       ", noReturnValue == "Void" ? "" : "return ", paramList1);
                pyClass.AppendLine(Environment.NewLine);
            }
            var fileName = @"..\Communicate\Libraries\CommunicateLib.py";
            File.WriteAllText(fileName, pyClass.ToString());
            Console.ReadLine();
        }
    }
}
