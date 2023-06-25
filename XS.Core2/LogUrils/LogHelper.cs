using log4net.Config;
using log4net;
using System.Reflection;
using System.Xml;

namespace XS.Core2
{
    static public class LogHelper
    {
        static LogHelper()
        {
            string ConfigPath = "conf/log4net.config";
            if (File.Exists(ConfigPath))
            {
                XmlDocument log4netConfig = new XmlDocument();
                log4netConfig.Load(File.OpenRead(ConfigPath));

                var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

                XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
            }
            else
            {
                throw new Exception("调用了日志操作方法，但没有配置log4配置文件，请在项目目录下创建conf/log4net.config");
            }
                

        }
        //private static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
         
        public static void Info<T>(string info)
        {
            // 获取一个日志记录器
            ILog logger = LogManager.GetLogger(typeof(T));
            logger.Info(info);

        }
        public static void Error<T>(string err)
        { 
            ILog logger = LogManager.GetLogger(typeof(T));
            logger.Error(err);

        }
        public static void Debug<T>(string msg)
        { 
            ILog logger = LogManager.GetLogger(typeof(T));
            logger.Debug(msg);
        }

        [Obsolete("此方法即可弃用，请使用Info(string info)替换")]
        public static void Write(string info)
        {
            ILog logger = LogManager.GetLogger("loginfo");
            logger.Info(info);
        }

    }
}
