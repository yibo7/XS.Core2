using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace XS.Core2.Strings
{
   public class Validate
    {
        // 判断字符串是否包含汉字
        public static bool ContainsChinese(string str)
        {
            Regex regex = new Regex("[\u4e00-\u9fa5]");
            return regex.IsMatch(str);
        }
        public static bool StringIsNullOrWhitespace(string value)
       {
           return ((value == null) || (value.Trim().Length == 0));
       }
        /// <summary>
        /// 判断文件流是否为UTF8字符集
        /// </summary>
        /// <param name="sbInputStream">文件流</param>
        /// <returns>判断结果</returns>
        private static bool IsUTF8(FileStream sbInputStream)
        {
            int i;
            byte cOctets;  // octets to go in this UTF-8 encoded character 
            byte chr;
            bool bAllAscii = true;
            long iLen = sbInputStream.Length;

            cOctets = 0;
            for (i = 0; i < iLen; i++)
            {
                chr = (byte)sbInputStream.ReadByte();

                if ((chr & 0x80) != 0) bAllAscii = false;

                if (cOctets == 0)
                {
                    if (chr >= 0x80)
                    {
                        do
                        {
                            chr <<= 1;
                            cOctets++;
                        }
                        while ((chr & 0x80) != 0);

                        cOctets--;
                        if (cOctets == 0) return false;
                    }
                }
                else
                {
                    if ((chr & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    cOctets--;
                }
            }

            if (cOctets > 0)
            {
                return false;
            }

            if (bAllAscii)
            {
                return false;
            }

            return true;

        }
        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");

        }


        public static bool IsIPSect(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");

        }
       /// <summary>
       /// 验证手机号是否正确
       /// </summary>
       /// <param name="mobile"></param>
       /// <returns></returns>
        public static bool IsMobile(string mobile)
        {
            return Regex.IsMatch(mobile, @"^0{0,1}(13[0-9]|15[0-9]|18[0-9]|14[0-9])[0-9]{8}$");

        }
        /// <summary>
        /// 返回指定IP是否在指定的IP数组所限定的范围内, IP数组内的IP地址可以使用*表示该IP段任意, 例如192.168.1.*
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="iparray"></param>
        /// <returns></returns>
        public static bool InIPArray(string ip, string[] iparray)
        {

            string[] userip = GetString.SplitString(ip, @".");
            for (int ipIndex = 0; ipIndex < iparray.Length; ipIndex++)
            {
                string[] tmpip = GetString.SplitString(iparray[ipIndex], @".");
                int r = 0;
                for (int i = 0; i < tmpip.Length; i++)
                {
                    if (tmpip[i] == "*")
                    {
                        return true;
                    }

                    if (userip.Length > i)
                    {
                        if (tmpip[i] == userip[i])
                        {
                            r++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                }
                if (r == 4)
                {
                    return true;
                }


            }
            return false;

        }

        /// <summary>
        /// 检测是否有危险的可能用于链接的字符串
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeUserInfoString(string str)
        {
            return !Regex.IsMatch(str, @"^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|^Guest");
        }
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {

            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
        /// <summary>
        /// 判断是否为base64字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBase64String(string str)
        {
            //A-Z, a-z, 0-9, +, /, =
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }
        /// <summary>
        /// 检测是否是正确的Url
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool IsURL(string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }
        /// <summary>
        /// 判断文件名是否为浏览器可以直接显示的图片文件名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否可以直接显示</returns>
        public static bool IsImgFilename(string filename)
        {
            filename = filename.Trim();
            if (filename.EndsWith(".") || filename.IndexOf(".") == -1)
            {
                return false;
            }
            string extname = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
            return (extname == "jpg" || extname == "jpeg" || extname == "png" || extname == "bmp" || extname == "gif");
        }
        /// <summary>
        /// 是否为数值串列表，各数值间用","间隔
        /// </summary>
        /// <param name="numList"></param>
        /// <returns></returns>
        public static bool IsNumericList(string numList)
        {
            if (numList == "")
                return false;
            foreach (string num in numList.Split(','))
            {
                if (!IsNumeric(num))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 检查颜色值是否为3/6位的合法颜色
        /// </summary>
        /// <param name="color">待检查的颜色</param>
        /// <returns></returns>
        public static bool CheckColorValue(string color)
        {
            if (string.IsNullOrEmpty(color))
            {
                return false;
            }

            color = color.Trim().Trim('#');

            if (color.Length != 3 && color.Length != 6)
            {
                return false;
            }
            //不包含0-9  a-f以外的字符
            if (!Regex.IsMatch(color, "[^0-9a-f]", RegexOptions.IgnoreCase))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 验证是否为正整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt(string str)
        {

            return Regex.IsMatch(str, @"^[0-9]*$");
        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNumeric(object obj)
        {
            return obj is int;
        }

        /// <summary>
        /// 判断是否英文字母或数字的C#正则表达式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public bool IsNatural_Number(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
            return reg1.IsMatch(str);
        }
        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray, string strsplit)
        {

            return InArray(str, GetString.SplitString(stringarray, strsplit), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray, string strsplit, bool caseInsensetive)
        {
            return InArray(str, GetString.SplitString(stringarray, strsplit), caseInsensetive);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            return GetString.GetInArrayID(strSearch, stringArray, caseInsensetive) >= 0;
        }


        
        /// <summary>
        /// 是否为有效域
        /// </summary>
        /// <param name="host">域名</param>
        /// <returns></returns>
        public static bool IsValidDomain(string host)
        {
            Regex r = new Regex(@"^\d+$");
            if (host.IndexOf(".") == -1)
            {
                return false;
            }
            return r.IsMatch(host.Replace(".", string.Empty)) ? false : true;
        }
        /// <summary>
        /// 检测值是否有效，为 null 或 "" 均为无效
        /// </summary>
        /// <param name="obj">要检测的值</param>
        /// <returns></returns>
        public static bool CheckValiable(object obj)
        {
            if (Object.Equals(obj, null) || Object.Equals(obj, string.Empty))
                return false;
            else
                return true;
        }
        /// <summary>
        /// 检测一个字符串，是否存在于一个以固定分割符分割的字符串中
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="Strings">固定分割符分割的字符串</param>
        /// <param name="Str">分割符</param>
        /// <returns></returns>
        public static bool InArray(string str, string Strings, char Str)
        {
            bool blResult = false;

            string[] array = cConvert.SplitArray(Strings, Str);
            for (int i = 0; i < array.Length; i++)
            {
                if (str == array[i])
                {
                    blResult = true;
                    break;
                }
            }

            return blResult;
        }
        /// <summary>
        /// 检测一个字符串，是否存在于一个以固定分割符分割的字符串中,不区分大小写
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="array">字符串数组</param>
        /// <returns></returns>
        public static bool InArray(string str, string[] array)
        {
            bool blResult = false;
            
            for (int i = 0; i < array.Length; i++)
            {
                if (str.ToUpper().Trim() == array[i].ToUpper().Trim())
                {
                    blResult = true;
                    break;
                }
            }

            return blResult;
        }


        
        /// <summary>
        /// 现在网上比较大型的论坛都得备案，一旦有什么不太合理的信息，都可能受到有关部门的那啥。。。所以在信息过滤显得有点地位了。下面向大家介绍一个简单的信息硬过滤的办法。其实就是自动匹配
        /// </summary>
        /// <param name="strArr">要过虑的字符集</param>
        /// <param name="strContent">要过虑的内容</param>
        /// <returns></returns>
        public static bool CheckWords(string[] strArr, string strContent)
        {
            System.Text.RegularExpressions.Regex re;
            bool blIsBad = false;
            for (int i = 0; i < strArr.Length - 1; i++) //一个循环检查是否含有预定的字符串
            {
                re = new System.Text.RegularExpressions.Regex(strArr[i]);
                if (re.Match(strContent).Success)
                {
                    blIsBad = true;
                    break;
                    // Response.Write("捕捉到一预定信息：");
                    // Response.Write(BadWords[i]);
                    // Response.Write("<br>");
                }
            }
            return blIsBad;
        }
        /// <summary>
        /// 判断字符串是否为有效的邮件地址
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^.+\@(\[?)[a-zA-Z0-9\-\.]+\.([a-zA-Z]{2,3}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// 判断字符串是否为有效的URL地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsValidURL(string url)
        {
            return Regex.IsMatch(url, @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*[^\.\,\)\(\s]$");
        }

        /// <summary>
        /// 判断字符串是否为Int类型的
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsValidInt(string val)
        {
            return Regex.IsMatch(val, @"^[1-9]\d*\.?[0]*$");
        }

        /// <summary>
        /// 检测字符串是否全为正整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNum(string str)
        {
            bool blResult = true;//默认状态下是数字

            if (str == "")
                blResult = false;
            else
            {
                foreach (char Char in str)
                {
                    if (!char.IsNumber(Char))
                    {
                        blResult = false;
                        break;
                    }
                }
                if (blResult)
                {
                    if (int.Parse(str) == 0)
                        blResult = false;
                }
            }
            return blResult;
        }

        /// <summary>
        /// 检测字符串是否全为数字型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDouble(string str)
        {
            bool blResult = true;//默认状态下是数字

            if (str == "")
                blResult = false;
            else
            {
                foreach (char Char in str)
                {
                    if (!char.IsNumber(Char) && Char.ToString() != "-")
                    {
                        blResult = false;
                        break;
                    }
                }
            }
            return blResult;
        }
        /// <summary>
        /// 判断输入的字符串是否完全匹配正则
        /// </summary>
        /// <param name="RegexExpression">正则表达式</param>
        /// <param name="str">待判断的字符串</param>
        /// <returns></returns>
        public static bool IsValiable(string RegexExpression, string str)
        {
            bool blResult = false;

            Regex rep = new Regex(RegexExpression, RegexOptions.IgnoreCase);

            //blResult = rep.IsMatch(str);
            Match mc = rep.Match(str);

            if (mc.Success)
            {
                if (mc.Value == str) blResult = true;
            }


            return blResult;
        }

		
    }
}
