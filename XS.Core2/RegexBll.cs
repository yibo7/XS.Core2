using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace XS.Core2
{
    public class RegexBll
    {

        static private readonly RegexOptions opt = RegexOptions.IgnoreCase | RegexOptions.Compiled;

        /// <summary>
        /// 通过正则表达式查找字符集
        /// </summary>
        /// <param name="RegexString">要执行的正则表达式.</param>
        /// <param name="Soure">要查询的文本.</param>
        /// <param name="Index">每行结果都有可能出现多个列值，这里指定您要获取的列位置，默认获取第1个，也可以是第0或其他哦.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        static public  List<string> RegexFinds(string RegexString, string Soure,int Index=0)
        {

            List<string> lst = new List<string>();
           Regex r = new Regex(RegexString, opt);

            MatchCollection mc = r.Matches(Soure);
             
            for (int i = 0; i < mc.Count; i++)
            {
               string sv = mc[i].Result(string.Concat("$", Index));
                //string sv = mc[i].Groups[Index].Value;
                lst.Add(sv);
            }
            return lst;
        }
        static public List<string> RegexFinds(string RegexString, string Soure, string colname)
        {

            List<string> lst = new List<string>();
            Regex r = new Regex(RegexString, opt);

            MatchCollection mc = r.Matches(Soure);

            for (int i = 0; i < mc.Count; i++)
            {
                string sv = mc[i].Groups[colname].Value;
                //string sv = mc[i].Groups[Index].Value;
                lst.Add(sv);
            }
            return lst;
        }
        /// <summary>
        /// 通过正则表达式查找字符(非集合，基于命名的分组构造)
        /// </summary>
        /// <param name="RegexString">要执行的正则表达式</param>
        /// <param name="Soure">要查找的文本.</param>
        /// <param name="colname">基于命名的分组构造,如(http|ftp|https://)[^\.]*\.(?<domain>[^/|?]*).这里输入domain</param>
        /// <returns>System.String.</returns>
        static public string RegexFind(string RegexString, string Soure, string colname)
        {
            string MatchVale = "";
            Regex r = new Regex(RegexString, opt);
            Match m = r.Match(Soure);
            if (m.Success)
            {

                MatchVale = m.Groups[colname].Value;
            }
            return MatchVale;
        }
        /// <summary>
        /// 通过正则表达式查找字符(非集合)
        /// </summary>
        /// <param name="RegexString">要执行的正则表达式.</param>
        /// <param name="Soure">要查询的文本.</param>
        /// <param name="iIndex">结果有可能出现多个列值，这里指定您要获取的列位置，默认获取第1个，也可以是第0或其他哦..</param>
        /// <returns>System.String.</returns>
        static public  string RegexFind(string RegexString, string Soure,int iIndex = 0)
        {
            string MatchVale = "";
            Regex r = new Regex(RegexString, opt);
            Match m = r.Match(Soure);
            if (m.Success)
            {
                MatchVale = m.Result(string.Concat("$", iIndex)); 
            }
            return MatchVale;
        }
        /// <summary>
        /// 正则替换
        /// </summary>
        /// <param name="strWebPageHtml">需要替换的源码</param>
        /// <param name="strRegex">替换正则</param>
        /// <returns>被替换的的代码</returns>
        static public  string RegexReplace(string strWebPageHtml, string strRegex, string strNew)
        {

            strWebPageHtml = Regex.Replace(strWebPageHtml, strRegex, strNew, opt);

            return strWebPageHtml;
        }

        #region 应用实例

        public static string GetOneStartEnd(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }
        public static List<string> GetMoreStartEnd(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection mc = rg.Matches(str);
            List<string> lst = new List<string>();
            for (int i = 0; i < mc.Count; i++)
            {
                string sv = mc[i].Value;
                //string sv = mc[i].Groups[Index].Value;
                lst.Add(sv);
            }

            return lst;
        }
        #endregion


    }

     
}