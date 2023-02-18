using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XS.Core2
{
    public class LogHelper
    {
        private static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");

        private static object thisLock = new object();
        /// <summary>
        /// 将内容记录到info
        /// 客户端调用写法如下：LogHelp.WriteLog("123");  
        /// </summary>
        /// <param name="info"></param>
        /// <param name="realWriteLog"></param>
        public static void Write(string info, bool realWriteLog = true)
        {
            lock (thisLock)
            {
                if (realWriteLog)
                {
                    if (loginfo.IsInfoEnabled)
                    {
                        loginfo.Info(info);
                    }
                }
            }
        }


        /// <summary>
        /// 将内容记录到error
        /// 客户端调用写法如下：LogHelp.WriteLog("456",new Exception ("错误"));
        /// </summary>
        /// <param name="info"></param>
        /// <param name="se"></param>
        /// <param name="realWriteLog"></param>
        public static void Write(string info, Exception se, bool realWriteLog = true)
        {
            lock (thisLock)
            {
                if (realWriteLog)
                {
                    if (loginfo.IsInfoEnabled)
                    {
                        loginfo.Info(info, se);
                    }
                }
            }
        }
        /// <summary>
        /// 日志保留多少天
        /// </summary>
        /// <param name="logFilePath"></param>
        /// <param name="saveDays"></param>
        public static void SaveLogDays(string logFilePath, int saveDays)
        {
            lock (thisLock)
            {
                string[] logFileName = Directory.GetFiles(logFilePath);
                if (logFileName.Count() > saveDays)
                {
                    File.Delete(logFileName[0]);
                }
            }
        }
    }
}
