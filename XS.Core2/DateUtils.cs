using System;
using System.Text;

namespace XS.Core2
{
    /// <summary>
    /// 时间日期的常用方法
    /// </summary>
    public class DateUtils
    {  
        /// <summary>
        /// 获取当前时间的毫秒
        /// </summary>
        /// <returns></returns>
        public long CurrentTimeMillis()
        {
            long currentTicks = DateTime.Now.Ticks;
            DateTime dtFrom = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long currentMillis = (currentTicks - dtFrom.Ticks) / 10000;
            return currentMillis;
        }

        /// <summary>
        /// 转换一个double型数字串为时间，起始 0 为 1970-01-01 08:00:00
        /// 原理就是，每过一秒就在这个数字串上累加一
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>
        public static DateTime ConvertIntDateTime(double d)
        {
            DateTime time = DateTime.MinValue;

            DateTime startTime = DateTime.Parse("1970-01-01 08:00:00");

            time = startTime.AddSeconds(d);

            return time;
        }
        
        /// <summary>
         /// 转换时间为一个double型数字串，起始 0 为 1970-01-01 08:00:00
         /// 原理就是，每过一秒就在这个数字串上累加一
         /// </summary>
         /// <param name="time">时间</param>
         /// <returns>double</returns>
        public static double ConvertDateTimeInt(DateTime time)
        {
            double intResult = 0;

            DateTime startTime = DateTime.Parse("1970-01-01 08:00:00");

            intResult = (time - startTime).TotalSeconds;

            return intResult;
        }
        ///  <summary>
        ///  返回跨两个指定日期的日期和时间边界数
        ///  System.DateTime  date1  =  System.DateTime.Now;
        ///System.DateTime  date2  =  System.DateTime.Now.AddYears(2);
        ///WriteLine("Days  :  "  +  TimeHelper.DateDiff("type",date1,date2).ToString());
        ///Interval  值范围：
        ///  second,minute,hour,day,week,month,quarter,year
        ///  s,n,h,d,w,m,q,y
        ///  </summary>
        ///  <param  name="Interval">间隔类型，s秒，n分钟,h小时,d天，w周,m月，q季度,y年</param>
        ///  <param  name="StartDate">开始时间</param>
        ///  <param  name="EndDate">结束时间</param>
        ///  <returns>时间边界数,默认和类型错误时为0</returns>
        public static long DateDiff(string Interval, DateTime StartDate, DateTime EndDate)
        {
            long _datediffvalue = 0;
            System.TimeSpan TS = new System.TimeSpan(EndDate.Ticks - StartDate.Ticks); 
            switch (Interval)
            { 
                case "mm":
                    _datediffvalue = (long)TS.Milliseconds;
                    break;
                case "second":                        //秒
                case "s":
                    _datediffvalue = (long)TS.TotalSeconds;
                    break;
                case "minute":                        //分钟
                case "n":
                    _datediffvalue = (long)TS.TotalMinutes;
                    break;
                case "hour":                                //小时
                case "h":
                    _datediffvalue = (long)TS.TotalHours;
                    break;
                case "day":                                //天
                case "d":
                    _datediffvalue = (long)TS.Days;
                    break;
                case "week":                        //周
                case "w":
                    _datediffvalue = (long)(TS.Days / 7);
                    break;
                case "month":                        //月
                case "m":
                    _datediffvalue = (long)(TS.Days / 30);
                    break;
                case "quarter":                //季度
                case "q":
                    _datediffvalue = (long)((TS.Days / 30) / 3);
                    break;
                case "year":                                //年
                case "y":
                    _datediffvalue = (long)(TS.Days / 365);
                    break;
            }
            return (_datediffvalue);
        }
        /// <summary>
        /// 两个时间的差值
        /// </summary>
        /// <param name="StartDate">大时间</param>
        /// <param name="EndDate">小时间</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static string DateDiff(DateTime StartDate, DateTime EndDate, int type)
        {

            string dateDiff = null;

            TimeSpan ts1 = new TimeSpan(StartDate.Ticks);

            TimeSpan ts2 = new TimeSpan(EndDate.Ticks);

            TimeSpan ts = ts1.Subtract(ts2).Duration();
            string strResult = "";
            switch (type)
            {
                case 1:
                    strResult = ts.Days.ToString() + "天";
                    break;
                case 2:
                    strResult = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时";
                    break;
                case 3:
                    strResult = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟";
                    break;
                case 4:
                    strResult = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
                    break;

                default:
                    strResult = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
                    break;
            }


            return strResult;

        }

        /// <summary>
        /// 按照指定格式输出日期时间
        /// </summary>
        /// <param name="NowDate">时间</param>
        /// <param name="type">输出类型</param>
        /// <returns></returns>
        public static string WriteDate(string NowDate, int type)
        {
            double TimeZone = 0;
            DateTime NewDate = DateTime.Parse(NowDate).AddHours(TimeZone);
            string strResult = "";

            switch (type)
            {
                case 1:
                    strResult = NewDate.ToString();
                    break;
                case 2:
                    strResult = NewDate.ToShortDateString().ToString();
                    break;
                case 3:
                    strResult = NewDate.Year + "年" + NewDate.Month + "月" + NewDate.Day + "日 " + NewDate.Hour + "点" + NewDate.Minute + "分" + NewDate.Second + "秒";
                    break;
                case 4:
                    strResult = NewDate.Year + "年" + NewDate.Month + "月" + NewDate.Day + "日";
                    break;
                case 5:
                    strResult = NewDate.Year + "年" + NewDate.Month + "月" + NewDate.Day + "日 " + NewDate.Hour + "点" + NewDate.Minute + "分";
                    break;
                case 6:
                    strResult = NewDate.Year + "-" + NewDate.Month + "-" + NewDate.Day + "  " + NewDate.Hour + ":" + NewDate.Minute;
                    break;
                case 7:
                    strResult = NewDate.Month + "-" + NewDate.Day;
                    break;
                case 8:
                    strResult = NewDate.Year + "/" + NewDate.Month + "/" + NewDate.Day;
                    break;
                default:
                    strResult = NewDate.ToString();
                    break;
            }
            return strResult;
        }

        /// <summary>
        /// 毫秒转化时分秒毫秒
        /// </summary>
        /// <param name="ms">毫秒</param>
        /// <returns></returns>
        public static String MillisecondToTime(int ms)
        {
            int ss = 1000;
            int mi = ss * 60;
            int hh = mi * 60;
            int dd = hh * 24;

            int day = ms / dd;
            int hour = (ms - day * dd) / hh;
            int minute = (ms - day * dd - hour * hh) / mi;
            int second = (ms - day * dd - hour * hh - minute * mi) / ss;
            int milliSecond = ms - day * dd - hour * hh - minute * mi - second * ss;

            StringBuilder sb = new StringBuilder();
            if (day > 0)
            {
                sb.Append(day + "天");
            }
            if (hour > 0)
            {
                sb.Append(hour + "小时");
            }
            if (minute > 0)
            {
                sb.Append(minute + "分");
            }
            if (second > 0)
            {
                sb.Append(second + "秒");
            }
            if (milliSecond > 0)
            {
                sb.Append(milliSecond + "毫秒");
            }
            return sb.ToString();
        }


    }
}