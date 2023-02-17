using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using XS.Core2.FSO;
using XS.Core2.Strings;

namespace XS.Core2
{
    /// <summary>
    /// WebUtility : 基于System.Web的拓展类。
    /// </summary>
    public class WebUtils
    {

        /// <summary>
        /// 判断字符串是否为有效的URL地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsValidURL(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                return Regex.IsMatch(url.Trim(), @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*[^\.\,\)\(\s]$");
            }
            return false;
        }
        /// <summary>
        /// 检测指定的 Uri 是否有效
        /// </summary>
        /// <param name="url">指定的Url地址</param>
        /// <returns>bool</returns>
        public static int ValidateUrl(string url, out string err)
        {
            err = string.Empty;
            int iState;
            if (IsValidURL(url))
            {//连接到目标网页
                HttpWebRequest wreq = null;
                HttpWebResponse wresp = null;
                try
                {
                    wreq = (HttpWebRequest)WebRequest.Create(url);
                    wreq.Method = "GET";
                    wreq.UserAgent =
                        "Mozilla/5.0 (Windows; U; Windows NT 6.1; zh-CN; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8";
                    //wreq.Timeout = 5 * 1000;//超时时间5秒，默认100秒;
                    wreq.KeepAlive = false;
                    wresp = (HttpWebResponse)wreq.GetResponse();
                    ////采用流读取，并确定编码方式
                    //Stream s = wresp.GetResponseStream();
                    //StreamReader objReader = new StreamReader(s, System.Text.Encoding.GetEncoding(Charset));
                    //strHtml = objReader.ReadToEnd();

                    if (wresp.StatusCode == HttpStatusCode.OK)
                    {
                        //wresp.Close();
                        iState = 200;
                    }
                    else
                    {

                        //wresp.Close();
                        iState = Convert.ToInt32(wresp.StatusCode);
                    }
                }
                catch (Exception e)
                {
                    //Log.ErrorLog.ErrorFormat("调用URL出错:{0},{1}", url, e.Message);
                    err = e.Message;
                    iState = -2;
                }
                finally
                {
                    if (!Equals(wresp, null))
                    {
                        wresp.Close();
                        wresp = null;
                    }
                    if (!Equals(wreq, null))
                    {
                        wreq.Abort();
                        wreq = null;
                    }
                }

            }
            else
            {
                iState = -1;
            }


            return iState;
             

        }

        /// <summary>
        /// 提取网址
        /// </summary>
        /// <param name="sText">The s text.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static List<string> GetTextUrls(string sText)
        {
            List<string> Urls = new List<string>();
            //string sRule = @"(?i)www\.\w*\.(com.cn|gov.cn|net.cn|org.cn|top|com|net|org|cn|cc|info|me|tv|xyz|ws|tw|tel|mobi|asia|name|hk|eu|us|in|one|online|wiki|love|pw|club|ren|co|so|wang)";

            string sRule = @"(?<url>(http:|https:[/][/]|www.)([a-z]|[A-Z]|[0-9]|[/.]|[~])*)";

            MatchCollection mcsCollection = Regex.Matches(sText, sRule);

            foreach (Match mc in mcsCollection)
            {
                if (mc.Groups.Count > 0)
                {
                    string sUrl = mc.Groups[0].Value;
                    if (!string.IsNullOrEmpty(sUrl))
                    {
                        Urls.Add(sUrl.Trim());
                    }

                }
            }

            sRule = @"(?i)[a-z]*[0-9]*\.\w*\.(com.cn|gov.cn|net.cn|org.cn|top|com|net|org|cn|cc|info|me|tv|xyz|ws|tw|tel|mobi|asia|name|hk|eu|us|in|one|online|wiki|love|pw|club|ren|co|so|wang)";

            mcsCollection = Regex.Matches(sText, sRule);

            foreach (Match mc in mcsCollection)
            {
                if (mc.Groups.Count > 0)
                {
                    string sUrl = mc.Groups[0].Value;
                    if (!string.IsNullOrEmpty(sUrl))
                    {
                        if (!Urls.Contains(sUrl))
                            Urls.Add(sUrl.Trim());
                    }

                }
            }
            List<string> rz = new List<string>();
            foreach (var url in Urls)
            {

                if (!url.StartsWith("https://"))
                {
                    if (!url.StartsWith("http://"))
                        rz.Add(string.Concat("http://", url));
                    else
                    {
                        rz.Add(url);
                    }
                }
                else
                {
                    rz.Add(url);
                }
            }

            return rz;
        }
        /// <summary>
        /// 检测指定的 Uri 是否有效
        /// </summary>
        /// <param name="url">指定的Url地址</param>
        /// <returns>bool</returns>
        public static bool ValidateUrl(string url)
        {
            Uri newUri = new Uri(url);

            try
            {
                WebRequest req = WebRequest.Create(newUri);
                //req.Timeout				= 10000;
                WebResponse res = req.GetResponse();
                HttpWebResponse httpRes = (HttpWebResponse)res;

                if (httpRes.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>  
        /// 获取远程文件的大小
        /// </summary>
        /// <param name="sHttpUrl"></param>
        /// <returns></returns>
        public static long GetFileSize(string sHttpUrl)
        {
            long size = 0;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(sHttpUrl);
            HttpWebResponse myRes = (HttpWebResponse)myReq.GetResponse();
            size = myRes.ContentLength;
            myRes.Close();

            return size;
        }
         


        #region 获取指定页面的内容    
        static bool isLuan(string txt)
        {
            var bytes = Encoding.UTF8.GetBytes(txt);
            //239 191 189
            for (var i = 0; i < bytes.Length; i++)
            {
                if (i < bytes.Length - 3)
                    if (bytes[i] == 239 && bytes[i + 1] == 191 && bytes[i + 2] == 189)
                    {
                        return true;
                    }
            }
            return false;
        }
         
        /// <summary>
        /// 此方法可以解决某些页面的乱码问题
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>System.String.</returns>
        public static string GetHtml(string url)
        {

            string htmlCode;
            HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            webRequest.Timeout = 30000;
            webRequest.Method = "GET";
            webRequest.UserAgent = "Mozilla/4.0";
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");

            HttpWebResponse webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse();
            //获取目标网站的编码格式
            string contentype = webResponse.Headers["Content-Type"];
            Regex regex = new Regex("charset\\s*=\\s*[\\W]?\\s*([\\w-]+)", RegexOptions.IgnoreCase);
            if (webResponse.ContentEncoding.ToLower() == "gzip")//如果使用了GZip则先解压
            {
                using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                {
                   
                    using (Stream zipStream = new System.IO.Compression.GZipStream(streamReceive, System.IO.Compression.CompressionMode.Decompress))
                    {
                        //匹配编码格式
                        if (regex.IsMatch(contentype))
                        {
                            //“UTF8”不是支持的编码名
                            string ec = regex.Match(contentype).Groups[1].Value.Trim();
                            Encoding ending = Encoding.Default;
                            if (!string.IsNullOrEmpty(ec))
                            {
                                ec = ec.ToLower();
                                if (ec.IndexOf("utf8") > -1 || ec.IndexOf("utf-8") > -1)
                                    ending = Encoding.UTF8;
                            }
                            using (StreamReader sr = new System.IO.StreamReader(zipStream, ending))
                            {
                                htmlCode = sr.ReadToEnd();
                            }
                        }
                        else
                        {
                            
                            using (StreamReader sr = new System.IO.StreamReader(zipStream, Encoding.UTF8))
                            {
                                htmlCode = sr.ReadToEnd();
                            }
                            if (isLuan(htmlCode))
                            {
                                var data = new System.Net.WebClient { }.DownloadData(url); //再次执行，导致执行两次，以后优化
                                var r_gbk = new System.IO.StreamReader(new System.IO.MemoryStream(data), Encoding.Default);
                                htmlCode = r_gbk.ReadToEnd();

                            }
                           


                        }
                    }
                }
            }
            else
            {
                using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(streamReceive, Encoding.UTF8))
                    {
                        htmlCode = sr.ReadToEnd();
                    }
                    if (isLuan(htmlCode)) 
                    {
                        var data = new System.Net.WebClient { }.DownloadData(url); //再次执行，导致执行两次，以后优化
                        var r_gbk = new System.IO.StreamReader(new System.IO.MemoryStream(data), Encoding.Default);
                        htmlCode = r_gbk.ReadToEnd();

                    }
                }
            }
            return htmlCode;
        }

        /// <summary>
        /// 从指定的URL下载页面内容(使用WebRequest)
        /// </summary>
        /// <param name="url">指定URL</param>
        /// <returns></returns>
        public static string LoadURLString(string url)
        {
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //myWebRequest.ContentType = "";
            HttpWebResponse myWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
            Stream stream = myWebResponse.GetResponseStream();

            string strResult = "";
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.Default);//gb2312
            Char[] read = new Char[256];
            int count = sr.Read(read, 0, 256);
            int i = 0;
            while (count > 0)
            {
                i += Encoding.UTF8.GetByteCount(read, 0, 256);
                String str = new String(read, 0, count);
                strResult += str;
                count = sr.Read(read, 0, 256);
            }

            return strResult;
        }
        public static string LoadURLStringUTF8(string url)
        {
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse myWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
            Stream stream = myWebResponse.GetResponseStream();

            string strResult = "";
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.GetEncoding("utf-8"));
            Char[] read = new Char[256];
            int count = sr.Read(read, 0, 256);
            int i = 0;
            while (count > 0)
            {
                i += Encoding.UTF8.GetByteCount(read, 0, 256);
                String str = new String(read, 0, count);
                strResult += str;
                count = sr.Read(read, 0, 256);
            }

            return strResult;
        }


        /// <summary>
        /// 从指定的URL下载页面内容(使用WebClient)
        /// </summary>
        /// <param name="url">指定URL</param>
        /// <returns></returns>
        public static string LoadPageContent(string url)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            byte[] pageData = wc.DownloadData(url);
            return (Encoding.GetEncoding("gb2312").GetString(pageData));
        }

        /// <summary>
        /// 从指定的URL下载页面内容(使用WebClient)
        /// </summary>
        /// <param name="url">指定URL</param>
        /// <returns></returns>
        public static string LoadPageContent(string url, string postData)
        {
            WebClient wc = new WebClient();

            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");


            byte[] pageData = wc.UploadData(url, "POST", Encoding.Default.GetBytes(postData));


            return (Encoding.GetEncoding("gb2312").GetString(pageData));
        }
        #endregion


        #region 远程服务器下载文件
        /// <summary>
        /// 远程提取文件保存至服务器上(使用WebRequest)
        /// </summary>
        /// <param name="url">一个URI上的文件</param>
        /// <param name="saveurl">文件保存在服务器上的全路径</param>
        public static void RemoteGetFile(string url, string saveurl)
        {
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse myWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
            Stream stream = myWebResponse.GetResponseStream();

            //获得请求的文件大小
            int fileSize = (int)myWebResponse.ContentLength;

            int bufferSize = fileSize;
            byte[] buffer = new byte[bufferSize];
            FObject.WriteFile(saveurl, "temp");
            // 建立一个写入文件的流对象
            FileStream saveFile = File.Create(saveurl, bufferSize);
            int bytesRead;
            do
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                saveFile.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);

            saveFile.Flush();
            saveFile.Close();
            stream.Flush();
            stream.Close();

            myWebResponse.Close();
        }

        /// <summary>
        /// 远程提取一个文件保存至服务器上(使用WebClient)
        /// </summary>
        /// <param name="url">一个URI上的文件</param>
        /// <param name="saveurl">保存文件</param>
        public static void WebClientGetFile(string url, string saveurl)
        {
            WebClient wc = new WebClient();

            try
            {
                FObject.WriteFile(saveurl, "temp");
                wc.DownloadFile(url, saveurl);
            }
            catch
            { }

            wc.Dispose();
        }

        /// <summary>
        /// 远程提取一组文件保存至服务器上(使用WebClient)
        /// </summary>
        /// <param name="urls">一组URI上的文件</param>
        /// <param name="saveurls">保存文件</param>
        public static void WebClientGetFile(string[] urls, string[] saveurls)
        {
            WebClient wc = new WebClient();
            for (int i = 0; i < urls.Length; i++)
            {
                try
                {

                    wc.DownloadFile(urls[i], saveurls[i]);
                }
                catch
                { }
            }
            wc.Dispose();
        }
        #endregion

         

        /// <summary>
        /// HTMLs the decode.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>System.String.</returns>
        public static string HtmlDecode(string html)
        {
            return WebUtility.HtmlDecode(html);
        }

        /// <summary>
        /// HTMLs the encode.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>System.String.</returns>
        public static string HtmlEncode(string html)
        {
            return WebUtility.HtmlEncode(html);
        }

        /// <summary>
        /// URLs the decode.
        /// </summary>
        /// <param name="urldata">The urldata.</param>
        /// <returns>System.String.</returns>
        public static string UrlDecode(string urldata)
        {
            return WebUtility.UrlDecode(urldata);
        }

        /// <summary>
        /// URLs the encode.
        /// </summary>
        /// <param name="urldata">The urldata.</param>
        /// <returns>System.String.</returns>
        public static string UrlEncode(string urldata)
        {
            return WebUtility.UrlEncode(urldata);
        }
         

    }
}
