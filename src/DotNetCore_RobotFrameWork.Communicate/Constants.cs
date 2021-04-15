namespace DotNetCore_RobotFrameWork.Communicate
{
    public class Constants
    {
        public const string StrategyDelimiter = "::";
        public const string LocatorDelimiter = "||"; //or operator
    }

    public enum TestDataType
    {
        //None = 0,
        TestPlan = 1,
        Share = 2,
        TestSuite = 3,
        TestCase = 4,
        Extend = 5
    }

    public enum FindStrategy
    {
        Id = 1, //Element id. 	id:example
        Name = 2, //name attribute. 	name:example
        Identifier = 3, //Either id or name. 	identifier:example
        ClassName = 4, //Element class. 	class:example
        Tag = 5, //Tag name. 	tag:div
        XPath = 6, //XPath expression. 	xpath://div[@id="example"]
        Css = 7, //CSS selector. 	css:div#example
        Dom = 8, //DOM expression. 	dom:document.images[5]
        Link = 9, //Exact text a link has. 	link:The example
        Partial = 10, //link 	Partial link text. 	partial link:he ex
        Sizzle = 11, //Sizzle selector provided by jQuery. 	sizzle:div.example
        Jquery = 12, //Same as the above. 	jquery:div.example
    }

    public enum FormatType
    {
        Text = 0,
        Currency = 1,
        Numeric = 2,
        NumericNoDigit = 3
    }

    public enum BrowserName
    {
        Firefox = 1,
        Edge = 2,
        Chrome = 3,
    }
}
