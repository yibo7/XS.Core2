
using XS.Core2;

namespace XSCoreTest
{
    [TestClass]
    public class UtilsTests
    { 
        [TestMethod]
        public void TestSortedDict()
        {

            Dictionary<string, string> dic= new Dictionary<string, string>();
            dic.Add("c", "2");
            dic.Add("e", "4");
            dic.Add("A", "1");           
            dic.Add("d", "3");
            dic.Add("B", "5");
            dic.Add("1d", "3");
            dic.Add("ab", "f3");


            var sb = XsUtils.SortParams(dic);

            // Append key at the end
            //sb.Append("&key=").Append("2015ebsite");

            Console.WriteLine(sb);

        }

        

    }
}