﻿using Newtonsoft.Json.Linq;
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
        //// 保留多少个字符
        //public static string Cut(this string value, int maxLength)
        //{
        //    return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        //}
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
            return Md5Helper.SHA_256(strSoure);
        }
        public static string Md5(this string strSoure,string privateKey="")
        {
            return Md5Helper.Md5Hash($"{strSoure}{privateKey}");
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
        public static string CutStrLen(this string value, int maxLength)
        {
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
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
            try
            {
                return JsonConvert.DeserializeObject<dynamic>(str);
            }
            catch (Exception ex)
            {
                throw new Exception("字符串转成dynamic失败：" + ex.Message);
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
       
        public static int ToInt(this string str,int defaultV=0)
        {
            return XsUtils.StrToInt(str, defaultV);
        }
 
        public static long ToLong(this string str, long defaultV=0)
        {
            return XsUtils.StrToLong(str, defaultV);
        }
        public static float ToFloat(this string str, float defaultV=0)
        {
            return XsUtils.StrToFloat(str, defaultV);
        }
        public static decimal ToDecimal(this string str, decimal defaultV = 0)
        {
            return XsUtils.StrToDecimal(str, defaultV);
        }

        public static bool ToBool(this string str, bool defaultV = false)
        {
            return XsUtils.StrToBool(str, defaultV);
        }

        public static DateTime StrToDate(this string str)
        {
            return XsUtils.StrToDate(str);
        }
        //public static string Concat(this string str,string newstr)
        //{
        //    return string.Concat(str, newstr);
        //}
        

        //public static bool IsNullOrEmpty(this string str)
        //{
        //     return String.IsNullOrEmpty(str);
        //}

    }
}
