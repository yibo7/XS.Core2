
using XS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using XS.Core2;

namespace XS.Core2Test
{
    [TestClass]
    public class RegexTests
    {
        
        string clearContent(string strInput, bool isTags)
        {

            // 1、替换掉所有网址
            //string input = "爱的罗曼斯（完全版－根据原谱） CAT sound效果器整理 www.catsound.cn";
            string pattern = @"\b(?:(?:https?|ftp|file)://|www\.)[^\s]+";
            strInput = RegexBll.RegexReplace(strInput, pattern, "");

            // 2、替换掉所有广告语
            strInput = strInput.Replace("sound效果器整理", "");
            strInput = strInput.Replace("电吉他吧整合", "");
            strInput = strInput.Replace("()", "");
            strInput = strInput.Replace("（）", "");

            //3、用C#正则表达式替换掉括号及括号内的内容，注意，括号内的内容为数字时才作替换 
            //string input = "Smiths (The) - Reel Around The Fountain (2)"; 
            pattern = @"\(\d+\)";
            strInput = Regex.Replace(strInput, pattern, "");

            //4、 替换文本中的数字为空，但数字后面带有首字的不要替换
            //string input = "​花儿乐队​​62​,流行指弹吉他曲190首打包下载";
            pattern = @"(?<![首\d])(\d+)(?![\d首])";
            strInput = Regex.Replace(strInput, pattern, "");

            if (isTags) // 如果是标签还要做这些处理
            {
                strInput = strInput.Replace(")", ",）");
                strInput = strInput.Replace("(", ",（");
                #region 将括号替换成,号
                string replacement = "  $1  ";
                pattern = @"\(([^)]*)\)";
                strInput = Regex.Replace(strInput, pattern, replacement);

                pattern = @"（([^）]*)）";
                strInput = Regex.Replace(strInput, pattern, replacement);

                pattern = @"【([^】]*)】";
                strInput = Regex.Replace(strInput, pattern, replacement);
                #endregion

                strInput = strInput.Trim();
                strInput = strInput.Replace("  ", ",");

                //
                pattern = @",\s*-\s*|-\s*,";
                strInput = Regex.Replace(strInput, pattern, ",");

                strInput = strInput.Replace("-", ",");

                strInput = strInput.Replace("、", ",");

                strInput = strInput.Replace("，", ",");

                strInput = strInput.Replace("。", ",");

                strInput = strInput.Replace(",,", ",");

                // 去掉前面的逗号
                strInput = Regex.Replace(strInput, "^,", "");

                // 去掉后面的逗号
                strInput = Regex.Replace(strInput, ",$", "");



            }
            //else
            //{
            //    strInput = strInput.Replace("（", "【");
            //    strInput = strInput.Replace("）", "】");

            //    strInput = strInput.Replace("(", "【");
            //    strInput = strInput.Replace(")", "】");
            //}

            // 去掉前面的逗号
            strInput = Regex.Replace(strInput, "^-", "");

            // 去掉后面的逗号
            strInput = Regex.Replace(strInput, "-$", "");

            return strInput;
        }
        [TestMethod]
        public void RplaceAll()
        {
            string text = clearContent("【犬夜叉】-时代を越える想い（木吉他独奏）", true);

            Console.WriteLine(text);
        }

        [TestMethod]
        public void RplaceKhToSpace()
        {
            //string text = "【妖精的的旋律】-Lilium、Lilium（指弹版本二）";
            string text = "-Smiths (The) - Reel Around The Fountain- ";
            string replacement = "  $1  "; 
            string pattern = @"\(([^)]*)\)";            
            text = Regex.Replace(text, pattern, replacement);

            pattern = @"（([^）]*)）";
            text = Regex.Replace(text, pattern, replacement);

            pattern = @"【([^】]*)】";
            text = Regex.Replace(text, pattern, replacement);

            text = text.Trim();
            text = text.Replace("  ",",");

            pattern = @",\s*-\s*|-\s*,";
            text = Regex.Replace(text, pattern, ",");

            text = text.Replace("-", ",");

            text = text.Replace("、", ",");

            text = text.Replace("，", ",");

            text = text.Replace("。", ",");

            text = text.Replace(",,", ",");

            Console.WriteLine(text);


        }
        [TestMethod]
        public void RegexRplaceUrl()
        {
            // 替换掉所有网址
            string input = "爱的罗曼斯（完全版－根据原谱） CAT sound效果器整理 www.catsound.cn";
            string pattern = @"\b(?:(?:https?|ftp|file)://|www\.)[^\s]+";

            string newstr = RegexBll.RegexReplace(input, pattern, "");
            Console.WriteLine(newstr);


        }
        [TestMethod]
        public void ReplaceNumber()
        {
            // 替换数字，但数字后面带有首字的不要替换
            string input = "​花儿乐队​​62​,流行指弹吉他曲190首打包下载";
            string pattern = @"(?<![首\d])(\d+)(?![\d首])"; 

            string output = Regex.Replace(input, pattern, "");
            Console.WriteLine(output);


        }
        [TestMethod]
        public void ReplaceKh()
        {
            //用C#正则表达式替换掉括号及括号内的内容，注意，括号内的内容为数字时才作替换 

            string input = "Smiths (The) - Reel Around The Fountain (2)";

            // 定义一个匹配括号内内容的正则表达式
            string pattern = @"\(\d+\)";
            string output = Regex.Replace(input, pattern, "");
            Console.WriteLine(output);


        }
        [TestMethod]
        public void RegexFindAll()
        {
             
            // 定义一个待匹配的字符串
            string input = "Smiths (The) - Reel Around The Fountain (2)";

            // 定义一个匹配括号内内容的正则表达式
            string pattern = @"\((.*?)\)";

            List<string> sv = RegexBll.RegexFinds(pattern, input);
            foreach (var v in sv)
            {
                Console.WriteLine(v);

            }
        }
        [TestMethod]
        public void RegexFinds()
        {

            string s = XS.Core2.WebUtils.LoadURLString("http://www.beimai.com");

            List<string> sv = XS.Core2.RegexBll.RegexFinds(@"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?", s, 0);
            foreach (var v in sv)
            {
                Console.WriteLine(v);

            }


        }
        [TestMethod]
        public void RegexFindOne()
        {
            //List<string> lst = XS.Core.RegexBll.GetFriendLinks("http://www.beimai.com");
            //foreach (var v in lst)
            //{
            //    Console.WriteLine(v);
            //}

            // string s = XS.Core.WebUtils.LoadURLString("http://seo.chinaz.com/?q=beimai.com");

            //string v = XS.Core.RegexBll.RegexFind("baiduapp/([0-9]*).gif", s, 1);
            // Console.WriteLine(v);


        }
    }
}