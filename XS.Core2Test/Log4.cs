
using XS.Core2;
using static System.Net.Mime.MediaTypeNames;

namespace XS.Core2Test
{
    [TestClass]
    public class Log4
    {
        [TestMethod]
        public void TestLogInfo()
        {
            LogHelper.Write("fffffff");
            Console.WriteLine("testdddd");
        }
    }
}