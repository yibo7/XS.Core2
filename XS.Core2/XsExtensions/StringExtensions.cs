using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XS.Core2.Strings;
using System.Web;
using System.Net;
using XS.Core2.Encrypts;

namespace XS.Core2.XsExtensions
{
    public static class StringExtensions
    {
        public static string ToBase64(this string strSoure)
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(strSoure); // 将字符串转换为byte数组
            return  Convert.ToBase64String(bytesToEncode);
        }

        public static string AESDecode(this string strSoure, string decryptKey)
        {
            return AES.Decode(strSoure, decryptKey);
        }
        public static string AESEncode(this string strSoure, string decryptKey)
        {
            return AES.Encode(strSoure, decryptKey);
        }

        public static string SHA256(this string strSoure)
        {
            return Md5Helper.SHA256(strSoure);
        }
        public static string Md5(this string strSoure)
        {
            return Md5Helper.MD5(strSoure);
        }
        /// <summary>
        /// 通过换行符号将字符串分割成数组
        /// </summary>
        /// <param name="strSoure"></param>
        /// <returns></returns>
        public static string[] SplitByWrap(this string strSoure)
        {
            return GetString.GetArrByWrap(strSoure);
        }

        /// <summary>
        /// 用指定字符标记分割成数组
        /// </summary>
        /// <param name="strSoure"></param>
        /// <param name="strSplit">指定标记</param>
        /// <returns></returns>
        public static string[] SplitByTag(this string strSoure, string strSplit)
        {
            return GetString.SplitString(strSoure, strSplit);
        }
        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string UrlEncode(this string strSoure)
        {
            return WebUtility.UrlEncode(strSoure);
        }
        /// <summary>
        /// 从开始与结束标签截取字符
        /// </summary>
        /// <param name="strSoure"></param>
        /// <param name="start">开始标记</param>
        /// <param name="end">结束标记</param>
        /// <returns></returns>
        public static string CutStr(this string strSoure, string start, string end)
        {
            Regex rg = new Regex("(?<=(" + start + "))[.\\s\\S]*?(?=(" + end + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(strSoure).Value;
        }
        /// <summary>
        /// 截取过长的字符
        /// </summary>
        /// <param name="strSoure"></param>
        /// <param name="length">要截取的长度</param> 
        /// <returns></returns>
        public static string CutStrLen(this string strSoure, int length)
        {
            return GetString.GetSubString(strSoure, length,"...");
        }
        /// <summary>
        /// 截取过长的字符
        /// </summary>
        /// <param name="strSoure"></param>
        /// <param name="length">要截取的长度</param>
        /// <param name="p_TailString">结尾符</param>
        /// <returns></returns>
        public static string CutStrLen(this string strSoure, int length, string p_TailString)
        {
            return GetString.GetSubString(strSoure, length, p_TailString);
        }

        public static dynamic ToJsonDynamic(this string str)
        {
            // 定义一个正则表达式
            string pattern = @"^(\\{.*\\}|\\[.*\\])$";
            // 使用 Regex 类进行匹配
            bool isJson = Regex.IsMatch(str, pattern);
            if (isJson)
            {
                try
                {
                    return JsonConvert.DeserializeObject<dynamic>(str);
                }
                catch (Exception ex)
                {
                    throw new Exception("字符串转成dynamic失败：" + ex.Message);
                }

            }
            else
            {
                throw new Exception("不是正确的json字符");
            }

        }
        /// <summary>
        /// 将字符转换到对象
        /// </summary>
        /// <typeparam name="T">指定的对象类型</typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T ToJson<T>(this string s) where T : class
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("s");
            }

            if (s == "null")
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(s);
        }

        public static JToken ToJsonJToken(this string str)
        {
            try
            {
                return JToken.Parse(str);
            }
            catch (JsonReaderException ex)
            {
                throw new Exception("转成json失败：" + ex.Message);
            }

        }
        //public static bool IsNullOrEmpty(this string str)
        //{
        //     return String.IsNullOrEmpty(str);
        //}
        
    }
}
