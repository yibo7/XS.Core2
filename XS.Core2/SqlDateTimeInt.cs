using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace XS.Core2
{

    /// <summary>
    /// 将时间转成数字或将数字转成时间，常用于提高sql查询时间的配合使用
    /// </summary>
    public class SqlDateTimeInt
    {
        //public static readonly SqlDateTimeInt Instance = new SqlDateTimeInt();

        static private int _BaseYear = 1970;
      static  SqlDateTimeInt()
        {
        }

      //static public SqlDateTimeInt(int BaseYear)
      //  {
      //      _BaseYear = BaseYear;
      //  }

        /// <summary>
        /// 日期转秒数
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns>秒数</returns>
      static public int GetSecond(DateTime dt)
        {
            dt = dt.AddHours(-8);
            DateTime dt_end = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
            DateTime dt_begin = new DateTime(1970, 1, 1, 0, 0, 0);
            return int.Parse(dt_end.Subtract(dt_begin).TotalSeconds.ToString());

        }
      static public int GetSecond()
      {
          DateTime dt = DateTime.Now;
          dt = dt.AddHours(-8);//不知道为什么，减去8个小时就跟mysql数据库生成的时间一样
          DateTime dt_end = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
          DateTime dt_begin = new DateTime(1970, 1, 1, 0, 0, 0);
          return int.Parse(dt_end.Subtract(dt_begin).TotalSeconds.ToString());

      }
        private static object _SyncRoot = new object();
        /// <summary>
        /// 永远不会重复的订单号
        /// </summary>
        /// <returns></returns>
        static public string NewOrderNumberSleep()
        {
            lock (_SyncRoot)
            {
                Thread.Sleep(1);
                string n1 = NewOrderNumber();
                return n1;
            }
        }
        /// <summary>
        /// 永远不会重复的订单号
        /// </summary>
        /// <returns></returns>
        static public long NewOrderNumberLong()
        {
           return long.Parse(NewOrderNumberSleep());
        }
        /// <summary>
        /// 同时执行可能会有重复
        /// </summary>
        /// <returns></returns>
        static public string NewOrderNumber()
        {
            //DateTime dt_s = DateTime.Now;
            //DateTime dt_end = new DateTime(dt_s.Year, dt_s.Month, dt_s.Day, dt_s.Hour, dt_s.Minute, dt_s.Second, dt_s.Millisecond);
            //DateTime dt_begin = new DateTime(2013, 1, 1, 0, 0, 0, 0);
            //return dt_end.Subtract(dt_begin).TotalMilliseconds.ToString();

            return NewOrderNumber(DateTime.Now);

        }

        static public string NewOrderNumber(DateTime dt)
        {
            DateTime dt_s = dt;
            DateTime dt_end = new DateTime(dt_s.Year, dt_s.Month, dt_s.Day, dt_s.Hour, dt_s.Minute, dt_s.Second, dt_s.Millisecond);
            DateTime dt_begin = new DateTime(2013, 1, 1, 0, 0, 0, 0);
            return dt_end.Subtract(dt_begin).TotalMilliseconds.ToString();
        }

        /// <summary>
        /// 秒数转日期
        /// </summary>
        /// <param name="Value">秒数</param>
        /// <returns>日期</returns>
        static public DateTime GetDateTime(int Value)
        {
            //秒数转时间日期
            //GMT时间从2000年1月1日开始，先把它作为赋为初值
            long Year = _BaseYear, Month = 1, Day = 1;
            long Hour = 0, Min = 0, Sec = 0;
            //临时变量
            long iYear = 0, iDay = 0;
            long iHour = 0, iMin = 0, iSec = 0;
            //计算文件创建的年份
            iYear = Value / (365 * 24 * 60 * 60);
            Year = Year + iYear;
            //计算文件除创建整年份以外还有多少天
            iDay = (Value % (365 * 24 * 60 * 60)) / (24 * 60 * 60);
            //把闰年的年份数计算出来
            int RInt = 0;
            for (int i = _BaseYear; i < Year; i++)
            {
                if ((i % 4) == 0)
                    RInt = RInt + 1;
            }
            //计算文件创建的时间(几时)
            iHour = ((Value % (365 * 24 * 60 * 60)) % (24 * 60 * 60)) / (60 * 60);
            Hour = Hour + iHour;
            //计算文件创建的时间(几分)
            iMin = (((Value % (365 * 24 * 60 * 60)) % (24 * 60 * 60)) % (60 * 60)) / 60;
            Min = Min + iMin;
            //计算文件创建的时间(几秒)
            iSec = (((Value % (365 * 24 * 60 * 60)) % (24 * 60 * 60)) % (60 * 60)) % 60;
            Sec = Sec + iSec;
            DateTime t = new DateTime((int)Year, (int)Month, (int)Day, (int)Hour, (int)Min, (int)Sec);
            DateTime t1 = t.AddDays(iDay - RInt).AddHours(8);
            return t1;
        }

    }
}
