using XS.Core2;

namespace XS.Core2Test
{
    [TestClass]
    public class Log4Test
    {
        [TestMethod]
        public void TestLogInfo()
        {
            LogHelper.Error<Log4Test>("8888888");
            Console.WriteLine("testdddd");
        }
    }
}