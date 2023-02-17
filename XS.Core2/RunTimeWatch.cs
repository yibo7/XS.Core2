using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XS.Core2
{

    /// <summary>
    /// 检测一段代码的运行时间
    /// </summary>
    public class RunTimeWatch
    {
        private int mintStart;

        /// <summary>
        /// 开始检测
        /// </summary>
        public void start()
        {
            mintStart = Environment.TickCount;
        }

        /// <summary>
        /// 结束检测-返回时间
        /// </summary>
        /// <returns></returns>
        public string elapsed()
        {
            return DateUtils.MillisecondToTime(Environment.TickCount - mintStart);
        }
        /// <summary>
        /// 结束检测-毫秒
        /// </summary>
        /// <returns></returns>
        public int endmillisecond()
        {
            return Environment.TickCount - mintStart;
        }


    }
}
