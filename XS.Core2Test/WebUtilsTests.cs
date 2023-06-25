
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using XS.Core2;
namespace XSCoreTest
{
    [TestClass]
    public class WebUtilsTests
    {
          
        [TestMethod]
        public void PostTime()
        {
            string s = SqlDateTimeInt.NewOrderNumberSleep();
            Console.WriteLine(s);

        } 
        public string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }
            return (sb.ToString());
        } 
         
        [TestMethod]
        public void GetHtml()
        {
             
            string url = "https://www.aitanqin.com";
            string s = WebUtils.GetHtml(url);
            
            Console.WriteLine(s);

        }
        [TestMethod]
        public void ValidateUrl()
        {
            string url = "https://www.aitanqin.com/Search.html?k=微光乐团&site=1";

            string errinfo = "---";
            int code = WebUtils.ValidateUrl(url, out errinfo);
            Console.WriteLine(string.Format("状态码：{0},错误信息:{1}", code, errinfo));
        }

    }
}