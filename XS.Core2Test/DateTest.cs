using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XS.Core2; 

namespace XS.Core2Test
{ 
    [TestClass]
    public class DateTest
    {
        [TestMethod]
        public void PostTime()
        {
            long d_span = DateUtils.DateDiff("day", DateTime.Parse("2022/1/19"), DateTime.Now.Date);
            Console.WriteLine($"返回的天数:{d_span}");

             
        }
    }
}
