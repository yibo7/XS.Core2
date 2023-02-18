
using XS.Core;
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
        
        public static string GetHtmlByJson(string url, string json = "")
        {
            var result = string.Empty;

            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                request.ContentType = "application/json";
                request.Method = "POST";
                //request.Headers.Add("nethash:b11fa2f2");
                //request.Headers.Add("version:''");

                //request.CookieContainer = _cookie;

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();

                    var response = (HttpWebResponse)request.GetResponse();

                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                    }
                }


            }
            catch (Exception uex)
            {
                // 出错处理
                return uex.Message;
            }

            return result;
        }
        [TestMethod]
        public void TestTrx()
        {

            for (int i = 23410355; i < 23410565; i++)
            {
                RunTimeWatch rt = new RunTimeWatch();
                rt.start();
                string s = GetHtmlByJson("http://47.242.72.203:8090/wallet/getblockbynum", "{ \"num\":23410355}");
                Console.WriteLine(rt.elapsed());
                //Console.WriteLine(s);
            }



        }

        [TestMethod]
        public void PostTime()
        {
            string s = SqlDateTimeInt.NewOrderNumberSleep();
            Console.WriteLine(s);

        }
        [TestMethod]
        public void Post()
        {
            string sUrl = "http://appapi.beimai.com/Api/Ask/AddQuestionAndAnswer";
            string args = "{\"title\":\"ddffdsfsdfsfsfs\",\"content\":\"dddddd\",\"answercontent\":\"ccccc\",\"isapproved\":\"0\"} ";
            string sPostData = string.Concat(sUrl, "?p=", UrlEncode(args));
            string s = Post(sPostData);
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
        string Post(string url)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.Headers.Add("BMsessionid", "-1");
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取内容  
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
         
        [TestMethod]
        public void GetHtml()
        {
             
            string url = "https://www.baidu.com";
            string s = WebUtils.LoadURLStringGBK(url);
            
            Console.WriteLine(s);

        }

    }
}