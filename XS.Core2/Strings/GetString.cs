using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web; 

namespace XS.Core2.Strings
{
   public class GetString
    {

      public static string NoHTML(string Htmlstring) //去除HTML标记   
      {   
          //删除脚本   
          Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);   
         //删除HTML   
         Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);   
        Htmlstring = Regex.Replace(Htmlstring, @"([/r/n])[/s]+", "", RegexOptions.IgnoreCase);   
         Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);   
          Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);   
  
          Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "/", RegexOptions.IgnoreCase);
          Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);   
          Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);   
          Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);   
          Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);   
          Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "/xa1", RegexOptions.IgnoreCase);   
          Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "/xa2", RegexOptions.IgnoreCase);   
          Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "/xa3", RegexOptions.IgnoreCase);   
          Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "/xa9", RegexOptions.IgnoreCase);   
          Htmlstring = Regex.Replace(Htmlstring, @"&#(/d+);", "", RegexOptions.IgnoreCase);   
  
          Htmlstring.Replace("<", "");   
          Htmlstring.Replace(">", "");   
          Htmlstring.Replace("/r/n", "");   
          //Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();   
  
          return Htmlstring;   
      }  


        /// <summary>
        /// 获得字符串中开始和结束字符串中间得值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="s">开始</param>
        /// <param name="e">结束</param>
        /// <returns></returns>
        public static string GetMidValue(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }

       public static string ClearHtml(string source)
       {
            string reg = @"[<].*?[>]";
            source = Regex.Replace(source, reg, "");

           return source;
       }

       /// <summary>
       /// 将一个文本框内的文本安回车换行来割分，放入数组
       /// </summary>
       /// <param name="sContent">要割分的内容</param>
       /// <returns>返回数组</returns>
       public static string[] GetArrByWrap(string sContent)
       {
           if (!string.IsNullOrEmpty(sContent))
           {
                //Regex re = new Regex("\r\n");
                Regex re = new Regex("\n");
                if (sContent.IndexOf("\r\n")>-1)
                {
                    re = new Regex("\r\n");
                }
                
                string[] aItems = re.Split(sContent);

                return aItems;
            }
           return null;
       }
        public static string[] GetArrByBR(string sContent)
        {
            Regex re = new Regex("<br />");
            string[] aItems = re.Split(sContent);

            return aItems;
        }
        public static string GetNewNameByDate(string OldFileName,DateTime dtDateTime)
        {
            string str = getFileType(OldFileName);
            string randomFileName = Path.GetRandomFileName().Replace(".", "");//某些情况下带点路径不兼用
            return string.Concat(dtDateTime.ToString("yyyyMMdd"), "/" + randomFileName + str);
        }
        public static string GetNewNameByDate(string OldFileName)
        {
            return GetNewNameByDate(OldFileName, DateTime.Now);
           //string str = getFileType(OldFileName);
           //string randomFileName = Path.GetRandomFileName().Replace(".","");//某些情况下带点路径不兼用
           //return string.Concat(DateTime.Now.ToString("yyyyMMdd") , "/" + randomFileName + str);
       }

       public static string GetNewNameByDate(string OldFileName,out string SmallFileName,out string MiddleFileName,out string BigFileName)
       {
           string str = getFileType(OldFileName);
           string randomFileName =string.Concat(DateTime.Now.ToString("yyyyMMdd"),"/", Path.GetRandomFileName().Replace(".", ""));//某些情况下带点路径不兼用

           SmallFileName = string.Concat(randomFileName, "-ebsmallimg", str);
           MiddleFileName = string.Concat(randomFileName, "-ebmiddleimg", str);
           BigFileName = string.Concat(randomFileName, "-ebbigimg", str);
           return string.Concat(randomFileName,"-ebbaseimg" , str);
       }

       public static string GetSmallImgUrl(string BigImgUrl)
       {
           string strEX = getFileType(BigImgUrl);
           return BigImgUrl.Replace(strEX, string.Concat("-small", strEX));
       }
       public static string GetMiddleImgUrl(string BigImgUrl)
       {
           string strEX = getFileType(BigImgUrl);
           return BigImgUrl.Replace(strEX, string.Concat("-midlle", strEX));
       }
       public static string GetBigImgUrl(string BigImgUrl)
       {
           string strEX = getFileType(BigImgUrl);
           return BigImgUrl.Replace(strEX, string.Concat("-big", strEX));
       }

        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;

            Byte[] bComments = Encoding.UTF8.GetBytes(p_SrcString);
            foreach (char c in Encoding.UTF8.GetChars(bComments))
            {    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
                if ((c > '\u0800' && c < '\u4e00') || (c > '\xAC00' && c < '\xD7A3'))
                {
                    //if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") || System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
                    //当截取的起始位置超出字段串长度时
                    if (p_StartIndex >= p_SrcString.Length)
                    {
                        return "";
                    }
                    else
                    {
                        return p_SrcString.Substring(p_StartIndex,
                                                       ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                    }
                }
            }


            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }



                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {

                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                            {
                                nFlag = 1;
                            }
                        }
                        else
                        {
                            nFlag = 0;
                        }

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                    {
                        nRealLength = p_Length + 1;
                    }

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);

                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }

        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
            //return GetSubStrings(p_SrcString, p_Length*2, p_TailString);
        }
        /// <summary>
        /// 格式化字节数字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FormatBytesStr(int bytes)
        {
            if (bytes > 1073741824)
            {
                return ((double)(bytes / 1073741824)).ToString("0") + "G";
            }
            if (bytes > 1048576)
            {
                return ((double)(bytes / 1048576)).ToString("0") + "M";
            }
            if (bytes > 1024)
            {
                return ((double)(bytes / 1024)).ToString("0") + "K";
            }
            return bytes.ToString() + "Bytes";
        }
        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!string.IsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                {
                    string[] tmp = { strContent };
                    return tmp;
                }
                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
            {
                return new string[0] { };
            }
        }
        /// <summary>
        /// 进行指定的替换(脏字过滤)
        /// </summary>
        public static string StrFilter(string str, string bantext)
        {
            string text1 = "";
            string text2 = "";
            string[] textArray1 = SplitString(bantext, "\r\n");
            for (int num1 = 0; num1 < textArray1.Length; num1++)
            {
                text1 = textArray1[num1].Substring(0, textArray1[num1].IndexOf("="));
                text2 = textArray1[num1].Substring(textArray1[num1].IndexOf("=") + 1);
                str = str.Replace(text1, text2);
            }
            return str;
        }

       public static string GetEmailHostName(string strEmail)
       {
           if (strEmail.IndexOf("@") < 0)
           {
               return "";
           }
           return strEmail.Substring(strEmail.LastIndexOf("@")).ToLower();
       }
       /// <summary>
       /// 返回 URL 字符串的编码结果
       /// </summary>
       /// <param name="str">字符串</param>
       /// <returns>解码结果</returns>
       public static string UrlDecode(string str)
       {
           return HttpUtility.UrlDecode(str);
       }
        /// <summary>
        /// 删除字符串尾部的回车/换行/空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RTrim(string str)
        {
            for (int i = str.Length; i >= 0; i--)
            {
                if (str[i].Equals(" ") || str[i].Equals("\r") || str[i].Equals("\n"))
                {
                    str.Remove(i, 1);
                }
            }
            return str;
        }
        /// <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(string content)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 过滤HTML中的不安全标签
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveUnsafeHtml(string content)
        {
            content = Regex.Replace(content, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"(script|frame|form|meta|behavior|style)([\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            return content;
        }

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns>字符长度</returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }
        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>
        public static int GetInArrayID(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (caseInsensetive)
                {
                    if (strSearch.ToLower() == stringArray[i].ToLower())
                    {
                        return i;
                    }
                }
                else
                {
                    if (strSearch == stringArray[i])
                    {
                        return i;
                    }
                }

            }
            return -1;
        }
        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>		
        public static int GetInArrayID(string strSearch, string[] stringArray)
        {
            return GetInArrayID(strSearch, stringArray, true);
        }



        /// <summary>
        /// 从HTML中获取文本,保留br,p,img
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        public static string GetTextFromHTML(string HTML)
        {
            System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"</?(?!br|/?p|img)[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return regEx.Replace(HTML, "");
        }
        /// <summary>
        /// 删除最后一个字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearLastChar(string str)
        {
            if (str == "")
                return "";
            else
                return str.Substring(0, str.Length - 1);
        }
        /// <summary>
        /// 获取一个URL中引用的文件名称（包括后缀符）
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <returns>string</returns>
        public static string GetFileName(string url)
        {
            //string[] Name = cConvert.SplitArray(url,'/');
            //return Name[Name.Length - 1];
            
            return System.IO.Path.GetFileName(url);
        }
       /// <summary>
       /// 获取文件名称，不带后缀
       /// </summary>
       /// <param name="url"></param>
       /// <returns></returns>
        static public string GetFileNameNoEx(string url)
        {
            string FileName = GetFileName(url);
            int Index = FileName.LastIndexOf(".");
            return FileName.Substring(0, Index);
        }
       
        /// <summary>
        /// 检测某一字符串的第一个字符是否与指定的
        /// 字符一致，否则在该字符串前加上这个字符
        /// </summary>
        /// <param name="Strings">字符串</param>
        /// <param name="Str">字符</param>
        /// <returns>返回 string</returns>
        public static string AddFirst(string Strings, string Str)
        {
            string strResult = "";
            if (Strings.StartsWith(Str))
            {
                strResult = Strings;
            }
            else
            {
                strResult = String.Concat(Str, Strings);
            }
            return strResult;
        }


        /// <summary>
        /// 检测某一字符串的最后一个字符是否与指定的
        /// 字符一致，否则在该字符串末尾加上这个字符
        /// </summary>
        /// <param name="Strings">字符串</param>
        /// <param name="Str">字符</param>
        /// <returns>返回 string</returns>
        public static string AddLast(string Strings, string Str)
        {
            string strResult = "";
            if (Strings.EndsWith(Str))
            {
                strResult = Strings;
            }
            else
            {
                strResult = String.Concat(Strings, Str);
            }
            return strResult;
        }

        /// <summary>
        /// 检测某一字符串的第一个字符是否与指定的
        /// 字符一致，相同则去掉这个字符
        /// </summary>
        /// <param name="Strings">字符串</param>
        /// <param name="Str">字符</param>
        /// <returns>返回 string</returns>
        public static string DelFirst(string Strings, string Str)
        {
            string strResult = "";
            if (Strings.Length == 0) throw new Exception("原始字符串长度为零");

            if (Strings.StartsWith(Str))
            {
                strResult = Strings.Substring(Str.Length, Strings.Length - 1);
            }
            else
            {
                strResult = Strings;
            }

            return strResult;
        }

        /// <summary>
        /// 检测某一字符串的最后一个字符是否与指定的
        /// 字符一致，相同则去掉这个字符
        /// </summary>
        /// <param name="Strings">字符串</param>
        /// <param name="Str">字符</param>
        /// <returns>返回 string</returns>
        public static string DelLast(string Strings, string Str)
        {
            string strResult = "";

            if (Strings.EndsWith(Str))
            {
                strResult = Strings.Substring(0, Strings.Length - Str.Length);
            }
            else
            {
                strResult = Strings;
            }

            return strResult;
        }
         
        /// <summary>
        /// 根据起始位置截取字符
        /// </summary>
        /// <param name="strContent">源字符</param>
        /// <param name="strStartTag">开始标记</param>
        /// <param name="strEndTag">结束标记</param>
        /// <returns></returns>
        public static string CutMiddleStr(string strContent, string strStartTag, string strEndTag)
        {
            int iStart = strContent.IndexOf(strStartTag) + strStartTag.Length;
            int iEnd = strContent.LastIndexOf(strEndTag);
            string strValue = string.Empty;
            if (iStart < iEnd && iEnd < strContent.Length)
                strValue = strContent.Substring(iStart, iEnd - iStart);

            return strValue;

        }
        /// <summary>
        /// 正则替换
        /// </summary>
        /// <param name="strWebPageHtml">需要替换的源码</param>
        /// <param name="strRegex">替换正则</param>
        /// <returns>被替换的的代码</returns>
        public static string RegexReplace(string strWebPageHtml, string strRegex, string strNew)
        {
            
            strWebPageHtml = Regex.Replace(strWebPageHtml, strRegex, strNew, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            return strWebPageHtml;
        }

        public static String PregReplace(String input, string[] pattern, string[] replacements)
        {
            if (replacements.Length != pattern.Length) throw new ArgumentException("Replacement and Pattern Arrays must be balanced");
            for (var i = 0; i < pattern.Length; i++)
            { input = Regex.Replace(input, pattern[i], replacements[i]); }
            return input;
        } 


        /// <summary>
        /// 执行正则提取出值
        /// </summary>
        /// <param name="RegexString">正则表达式</param>
        /// <param name="RemoteStr">HtmlCode源代码</param>
        /// <returns></returns>
        public static string RegexFind(string RegexString, string RemoteStr)
        {
            string MatchVale = "";
            Regex r = new Regex(RegexString, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match m = r.Match(RemoteStr);
            if (m.Success)
            {
                MatchVale = m.Value;
            }
            return MatchVale;
        }
        
        /// <summary>
        /// 获得数字形式的随机字符串
        /// </summary>
        /// <returns>数字形式的随机字符串</returns>
        public static string MakeFileName()
        {
            int mikecat_intNum;
            long mikecat_lngNum;
            string mikecat_strNum = System.DateTime.Now.ToString();
            mikecat_strNum = mikecat_strNum.Replace(":", "");
            mikecat_strNum = mikecat_strNum.Replace("-", "");
            mikecat_strNum = mikecat_strNum.Replace(" ", "");
            mikecat_lngNum = long.Parse(mikecat_strNum);
            System.Random mikecat_ran = new Random();
            mikecat_intNum = mikecat_ran.Next(1, 99999);
            mikecat_ran = null;
            mikecat_lngNum += mikecat_intNum;
            return mikecat_lngNum.ToString();
        }
        /// <summary>
        /// 获取服务器本机的MAC地址
        /// </summary>
        /// <returns></returns>
        public static string GetMAC_Address()
        {
            string strResult = "";

            //ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            //ManagementObjectCollection queryCollection = query.Get();
            //foreach (ManagementObject mo in queryCollection)
            //{
            //    if (mo["IPEnabled"].ToString() == "True") strResult = mo["MacAddress"].ToString();
            //}

            return strResult;
        }
        /// <summary>
        /// 判断客户端操作系统和浏览器的配置
        /// </summary>
        /// <param name="Info">客户端返回的头信息(Request.UserAgent)</param>
        /// <param name="Type">获取类型：1为操作系统， 2为浏览器</param>
        /// <returns></returns>
        public static string GetInfo(string Info, int Type)
        {

            string GetInfo = "";
            switch (Type)
            {
                case 1:
                    if (Instr(Info, @"NT 5.1") > 0)
                    {
                        GetInfo = "操作系统：Windows XP";
                    }
                    else if (Instr(Info, @"Tel") > 0)
                    {
                        GetInfo = "操作系统：Telport";
                    }
                    else if (Instr(Info, @"webzip") > 0)
                    {
                        GetInfo = "操作系统：操作系统：webzip";
                    }
                    else if (Instr(Info, @"flashget") > 0)
                    {
                        GetInfo = "操作系统：flashget";
                    }
                    else if (Instr(Info, @"offline") > 0)
                    {
                        GetInfo = "操作系统：offline";
                    }
                    else if (Instr(Info, @"NT 5") > 0)
                    {
                        GetInfo = "操作系统：Windows 2000";
                    }
                    else if (Instr(Info, @"NT 4") > 0)
                    {
                        GetInfo = "操作系统：Windows NT4";
                    }
                    else if (Instr(Info, @"98") > 0)
                    {
                        GetInfo = "操作系统：Windows 98";
                    }
                    else if (Instr(Info, @"95") > 0)
                    {
                        GetInfo = "操作系统：Windows 95";
                    }
                    else
                    {
                        GetInfo = "操作系统：未知";
                    }
                    break;
                case 2:
                    if (Instr(Info, @"NetCaptor 6.5.0") > 0)
                    {
                        GetInfo = "浏 览 器：NetCaptor 6.5.0";
                    }
                    else if (Instr(Info, @"MyIe 3.1") > 0)
                    {
                        GetInfo = "浏 览 器：MyIe 3.1";
                    }
                    else if (Instr(Info, @"NetCaptor 6.5.0RC1") > 0)
                    {
                        GetInfo = "浏 览 器：NetCaptor 6.5.0RC1";
                    }
                    else if (Instr(Info, @"NetCaptor 6.5.PB1") > 0)
                    {
                        GetInfo = "浏 览 器：NetCaptor 6.5.PB1";
                    }
                    else if (Instr(Info, @"MSIE 6.0b") > 0)
                    {
                        GetInfo = "浏 览 器：Internet Explorer 6.0b";
                    }
                    else if (Instr(Info, @"MSIE 6.0") > 0)
                    {
                        GetInfo = "浏 览 器：Internet Explorer 6.0";
                    }
                    else if (Instr(Info, @"MSIE 5.5") > 0)
                    {
                        GetInfo = "浏 览 器：Internet Explorer 5.5";
                    }
                    else if (Instr(Info, @"MSIE 5.01") > 0)
                    {
                        GetInfo = "浏 览 器：Internet Explorer 5.01";
                    }
                    else if (Instr(Info, @"MSIE 5.0") > 0)
                    {
                        GetInfo = "浏 览 器：Internet Explorer 5.0";
                    }
                    else if (Instr(Info, @"MSIE 4.0") > 0)
                    {
                        GetInfo = "浏 览 器：Internet Explorer 4.0";
                    }
                    else
                    {
                        GetInfo = "浏 览 器：未知";
                    }
                    break;
            }
            return GetInfo;
        }
        private static int Instr(string strA, string strB)
        {
            if (string.Compare(strA, strA.Replace(strB, "")) > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
          
        /// <summary>
        /// 获取一条SQL语句中的所参数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public static ArrayList OracleParame(string sql)
        {
            ArrayList list = new ArrayList();
            Regex r = new Regex(@":(?<x>[0-9a-zA-Z]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection mc = r.Matches(sql);
            for (int i = 0; i < mc.Count; i++)
            {
                list.Add(mc[i].Result("$1"));
            }

            return list;
        }
        /// <summary>
        /// 获取代码中所有图片的以HTTP开头的URL地址
        /// </summary>
        /// <param name="sourceString">代码内容</param>
        /// <returns>ArrayList</returns>
        public static ArrayList GetImgFileUrl(string sourceString)
        {
            ArrayList imgArray = new ArrayList();

            Regex r = new Regex("<IMG(.*?)src=('|\"|)(http://.*?)('|\"| |>)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            MatchCollection mc = r.Matches(sourceString);
            for (int i = 0; i < mc.Count; i++)
            {
                if (!imgArray.Contains(mc[i].Result("$3")))
                {
                    imgArray.Add(mc[i].Result("$3"));
                }
            }

            return imgArray;
        }
        /// <summary>
        /// 获取代码中所有图片的可以不是以HTTP开头的URL地址
        /// </summary>
        /// <param name="sourceString">代码内容</param>
        /// <returns>ArrayList</returns>
        public static List<string> GetImgUrl(string sourceString)
        {
            List<string> imgArray = new List<string>();

            Regex r = new Regex("<IMG(.*?)src=('|\"|)(.*?)('|\"| |>)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            MatchCollection mc = r.Matches(sourceString);
            for (int i = 0; i < mc.Count; i++)
            {
                if (!imgArray.Contains(mc[i].Result("$3")))
                {
                    imgArray.Add(mc[i].Result("$3"));
                }
            }

            return imgArray;
        }
        /// <summary>
        /// 获取代码中所有文件的以HTTP开头的URL地址
        /// </summary>
        /// <param name="sourceString">代码内容</param>
        /// <returns>ArrayList</returns>
        public static Hashtable getFileUrlPath(string sourceString)
        {
            Hashtable url = new Hashtable();

            Regex r = new Regex(" (src|href|background|value)=('|\"|)(http://.*?)('|\"| |>)",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            MatchCollection mc = r.Matches(sourceString);
            for (int i = 0; i < mc.Count; i++)
            {
                if (!url.ContainsValue(mc[i].Result("$3")))
                {
                    url.Add(i, mc[i].Result("$3"));
                }
            }

            return url;
        }
        /// <summary>
        /// 获取文件的后缀
        /// </summary>
        /// <param name="filenameOrurl">文件名称或地址(包括文件名)</param>
        /// <returns></returns>
        public static string getFileType(string filenameOrurl)
        {
            int iListIndex = filenameOrurl.LastIndexOf(".");
            if (iListIndex > -1)
            {
               return filenameOrurl.Substring(iListIndex, filenameOrurl.Length - iListIndex);

                 
            }
            return string.Empty;
        }
        public static string getFileType(string filenameOrurl, bool IsKeepDot)
        {
            int num = 0;
            if (!IsKeepDot)
            {
                num = 1;
            }
            int startIndex = filenameOrurl.LastIndexOf(".") + num;
            return filenameOrurl.Substring(startIndex, filenameOrurl.Length - startIndex);
        }
       public static List<string> StringToList(string str,char split)
       {
           string[] aS = str.Split(split);
           List<string> lst = new List<string>();
           foreach (string s in aS)
           {
               lst.Add(s);
           }
           return lst;
       }
        /// <summary>
        /// 输出由同一字符组成的指定长度的字符串
        /// </summary>
        /// <param name="Char">输出字符，如：A</param>
        /// <param name="i">指定长度</param>
        /// <returns></returns>
        public static string Strings(char Char, int i)
        {
            string strResult = null;

            for (int j = 0; j < i; j++)
            {
                strResult += Char;
            }
            return strResult;
        }


        /// <summary>
        /// 返回字符串的真实长度，一个汉字字符相当于两个单位长度
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns></returns>
        public static int Len(string str)
        {
            int intResult = 0;

            foreach (char Char in str)
            {
                if ((int)Char > 127)
                    intResult += 2;
                else
                    intResult++;
            }
            return intResult;
        }


        /// <summary>
        /// 以日期为标准获得一个绝对的名称
        /// </summary>
        /// <returns>返回 String</returns>
        public static string MakeName()
        {
            /*
            string y = DateTime.Now.Year.ToString();
            string m = DateTime.Now.Month.ToString();
            string d = DateTime.Now.Day.ToString();
            string h = DateTime.Now.Hour.ToString();
            string n = DateTime.Now.Minute.ToString();
            string s = DateTime.Now.Second.ToString();
            return y + m + d + h + n + s;
            */

            return DateTime.Now.ToString("yyMMddHHmmss");
        }


        /// <summary>
        /// 返回字符串的真实长度，一个汉字字符相当于两个单位长度(使用Encoding类)
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns></returns>
        public static int getLen(string str)
        {
            int intResult = 0;
            Encoding gb2312 = Encoding.GetEncoding("gb2312");
            byte[] bytes = gb2312.GetBytes(str);
            intResult = bytes.Length;
            return intResult;
        }
       static public string NoHtml(string html)
        {
            string StrNohtml = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            StrNohtml = System.Text.RegularExpressions.Regex.Replace(StrNohtml, "&[^;]+;", "");
            return StrNohtml;
        }
     static  public string NoUbb(string str)
       {
           str = RegexReplace(str, @"\<.*\>|\[.*\]", "");
           str = RegexReplace(str, @"\[[a-z][^\]]*\]|\[\/[a-z]+\]", "");
           str = str.Replace("/</", "");
           str = str.Replace("/>/", "");
           str = str.Replace("/\r?\n/", "");
           str = str.Replace("<br>", "");
           str = str.Replace("( )|(&nbsp)", "");
           str = RegexReplace(str, @"(\<).*?(\>)", "");
           return str;

       }


        /// <summary>
        /// 按照字符串的实际长度截取指定长度的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="Length">指定长度</param>
        /// <returns></returns>
        public static string CutLen(string str, int Length)
        {
            int i = 0, j = 0;

            foreach (char Char in str)
            {
                if ((int)Char > 127)
                    i += 2;
                else
                    i++;

                if (i > Length)
                {
                    str = str.Substring(0, j - 2) ;
                    break;
                }
                j++;
            }
            return str;
        }
        /// <summary>
        /// 按照字符串的实际长度截取指定长度的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="Length">指定长度</param>
        /// <returns></returns>
        public static string CutLenEnd(string str, int Length)
        {
            int i = 0, j = 0;

            foreach (char Char in str)
            {
                if ((int)Char > 127)
                    i += 2;
                else
                    i++;

                if (i > Length)
                {
                    str = str.Substring(0, j - 2) + "...";
                    break;
                }
                j++;
            }
            return str;
        }
        public static string CutLenCen(string str, int Length)
        {
            int ilen = str.Length;
            if (ilen > Length)
            {
                double numl = ilen / 2;
                int iMidStar = ((int)Math.Round(numl)) / 2;
                int iMidEnd = iMidStar + Length;
                string strtem = str.Substring(iMidStar, iMidEnd);
                str = str.Replace(strtem, "...");
            }
            return str;
        }

        public static string SubStr(string str, int length)
        {

            if (str == null)
                str = "";
            if (length <= 0)
                return str;
            if (Encoding.Default.GetByteCount(str) <= length)
                return str;
            else
            {
                byte[] txtBytes = Encoding.Default.GetBytes(str);
                byte[] newBytes = new byte[length];

                for (int i = 0; i < length; i++)
                    newBytes[i] = txtBytes[i];

                return Encoding.Default.GetString(newBytes) + "...";
            }
        }
        /// <summary>
        /// 截取中间部分字符串！
        /// </summary>
        /// <param name="str">总的字符串</param>
        /// <param name="starLen">开始截取字符串长度</param>
        /// <param name="endLen">截止到的字符串长度</param>
        /// <param name="sumlen">总字符串的总长度</param>
        /// <returns>字符串</returns>
        public static string subStringURL(string str, int starLen, int endLen, int sumlen)
        {
            int i = 0;

            foreach (char Char in str)
            {
                if ((int)Char > 127)
                    i += 2;
                else
                    i++;

                if (i > endLen)
                {
                    str = str.Substring(starLen, endLen - 2) + "...";
                    break;
                }
                endLen++;
            }
            return str;
        }


        /// <summary>
        /// 获取指定长度的纯数字随机数字串
        /// </summary>
        /// <param name="intLong">数字串长度</param>
        /// <returns>字符串</returns>
        public static string RandomNUM(int intLong)
        {
            string strResult = "";

            Random r = new Random();
            for (int i = 0; i < intLong; i++)
            {
                strResult = strResult + r.Next(10);
            }

            return strResult;
        }

        /// <summary>
        /// 获取一个由26个小写字母组成的指定长度的随即字符串
        /// </summary>
        /// <param name="intLong">指定长度</param>
        /// <returns></returns>
        public static string RandomSTR(int intLong)
        {
            string strResult = "";
            string[] array = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

            Random r = new Random();

            for (int i = 0; i < intLong; i++)
            {
                strResult += array[r.Next(26)];
            }

            return strResult;
        }

        /// <summary>
        /// 获取一个由数字和26个小写字母组成的指定长度的随即字符串
        /// </summary>
        /// <param name="intLong">指定长度</param>
        /// <returns></returns>
        public static string RandomNUMSTR(int intLong)
        {
            string strResult = "";
            string[] array = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

            Random r = new Random();

            for (int i = 0; i < intLong; i++)
            {
                strResult += array[r.Next(36)];
            }

            return strResult;
        }


        public static string GetStrNumber(string str)
        {
            Regex rgc = new Regex("(\\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection mcc = rgc.Matches(str);
            string strNumber = string.Empty;
            foreach (Match match in mcc)
            {
                strNumber = strNumber + match.Value;
            }
            return strNumber;
        }




        /// <summary>
        /// 从包含中英文的字符串中截取固定长度的一段，strInput为传入字符串，intLen为截取长度（一个汉字占两个位）。
        /// </summary>
        /// <param name="strInput">为传入字符串</param>
        /// <param name="intLen">为截取长度</param>
        /// <returns></returns>
        public string cutString(string strInput, int intLen)
        {
            strInput = strInput.Trim();
            byte[] myByte = System.Text.Encoding.Default.GetBytes(strInput);
            if (myByte.Length > intLen)
            {
                //截取操作
                string resultStr = "";
                for (int i = 0; i < strInput.Length; i++)
                {
                    byte[] tempByte = System.Text.Encoding.Default.GetBytes(resultStr);
                    if (tempByte.Length < intLen - 4)
                    {
                        resultStr += strInput.Substring(i, 1);
                    }
                    else
                    {
                        break;
                    }
                }
                return resultStr + " ...";
            }
            else
            {
                return strInput;
            }
        }
        
        /// <summary>
        /// 获取一条SQL语句中的所参数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public static List<string> SqlParame(string sql)
        {
            List<string> list = new List<string>();

            string pattern = @"select(\s+top\s\d+)?\s+(?<fields>.+?)\s+from";
            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match m = reg.Match(sql);
            if (m.Success)
            {
                string Fields = m.Groups["fields"].Value.Trim();
                if (Fields.IndexOf(",") > 0)
                {
                     string[] Fld = Fields.Split(',');
                     foreach (string _fld in Fld)
                     {
                         list.Add(_fld);
                     }
                }
                else
                {
                    list.Add(Fields); 
                }
            }


            //Regex r = new Regex(@"@(?<x>[0-9a-zA-Z]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            //MatchCollection mc = r.Matches(sql);
            //for (int i = 0; i < mc.Count; i++)
            //{
            //    list.Add(mc[i].Result("$1"));
            //}

            return list;
        }



        #region ubb处理

        #endregion

       /// <summary>
       /// 电话 处理 中间4位 ****
       /// </summary>
       /// <param name="strPhone">电话号码</param>
       /// <returns></returns>
        public static string GetHidePhoneUName(string strPhone)
        {
            if (Validate.IsMobile(strPhone) && strPhone.Length == 11)
            {
                return string.Concat(strPhone.Substring(0, 3), "****", strPhone.Substring(7));
            }
            else
            {
                return strPhone;
            }
        }

    }
}
