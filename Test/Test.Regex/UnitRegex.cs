using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace Test.DRegex
{
    [TestClass]
    public class UnitRegex
    {
        [TestMethod]
        public void TestMethod1()
        {
            var match = Regex.Match("Trace: 2020-01-19 23:58:17.5724 [110]", "\\[([0-9]{1,6})\\]");
            var match1 = Regex.Match("Trace: 2020-01-19 23:58:17.5724 [110]", "\\[[0-9]{1,6}\\]");
        }
    }
}
