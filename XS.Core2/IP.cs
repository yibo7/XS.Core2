using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace XS.Core2
{

    /// <summary>
    /// IP处理类.
    /// </summary>
    public class IP
    {
        /// <summary>
        /// 将IP地址转为整数形式
        /// </summary>
        /// <returns>整数</returns>
        public static long IPToInt(IPAddress ip)
        {
            int x = 3;
            long o = 0;
            foreach (byte f in ip.GetAddressBytes())
            {

                o += (long)f << 8 * x--;

            }
            return o;

        }

        /// <summary>
        /// 将整数转为IP地址
        /// </summary>
        /// <returns>IP地址</returns>
        public static IPAddress IntToIP(long iIP)
        {

            var b = new byte[4];

            for (int i = 0; i < 4; i++)
            {

                b[3 - i] = (byte)(iIP >> 8 * i & 255);

            }

            return new IPAddress(b);

        }

        /// <summary>
        /// 验证传入IP地址是否应被屏蔽。
        /// </summary>
        /// <param name="IP地址">待验证的IP</param>
        /// <returns>是否应被屏蔽</returns>
        public static bool IsAllowIP(IPAddress CurrentIP, IPAddress StarIP, IPAddress EndIP, DateTime dtEnd)
        {

            long ipCurrent = IPToInt(CurrentIP);
            long iStarIP = IPToInt(StarIP);
            long iEndIP = IPToInt(EndIP);
            return !(dtEnd > DateTime.Now && iStarIP <= ipCurrent && iEndIP >= ipCurrent);

        }

        /// <summary>
        /// 检测指定IP地址是否已受到屏蔽
        /// </summary>
        /// <param name="IP地址">要检测的IP地址</param>
        /// <returns>是否属于已屏蔽的IP</returns>
        public static bool IsAllowIP(string CurrentIP, string StarIP, string EndIP, DateTime dtEnd)
        {

            return IsAllowIP(IPAddress.Parse(CurrentIP), IPAddress.Parse(StarIP), IPAddress.Parse(EndIP), dtEnd);

        }
    }

    /// <summary>
    /// 将IP转换成整数或整数转换成ip的处理类
    /// </summary>
    public class IpToInt
    {
        public static long ConvertIPToLong(string ipAddress)
        {
            System.Net.IPAddress ip;

            if (System.Net.IPAddress.TryParse(ipAddress, out ip))
            {
                byte[] bytes = ip.GetAddressBytes();

                return (long)(((long)bytes[0] << 24) | (bytes[1] << 16) |
                    (bytes[2] << 8) | bytes[3]);
            }
            else
                return 0;
        }

        /// <summary>
        /// 将IPv4格式的字符串转换为int型表示
        /// </summary>
        /// <param name="strIPAddress">IPv4格式的字符</param>
        /// <returns></returns>
        public static long IPToNumber(string strIPAddress)
        {
            //将目标IP地址字符串strIPAddress转换为数字
            string[] arrayIP = strIPAddress.Split('.');
            int sip1 = Int32.Parse(arrayIP[0]);
            int sip2 = Int32.Parse(arrayIP[1]);
            int sip3 = Int32.Parse(arrayIP[2]);
            int sip4 = Int32.Parse(arrayIP[3]);
            long tmpIpNumber;
            tmpIpNumber = sip1 * 256 * 256 * 256 + sip2 * 256 * 256 + sip3 * 256 + sip4;
            return tmpIpNumber;
        }

        /// <summary>
        /// 将int型表示的IP还原成正常IPv4格式。
        /// </summary>
        /// <param name="intIPAddress">int型表示的IP</param>
        /// <returns></returns>
        public static string NumberToIP(int intIPAddress)
        {
            int tempIPAddress;
            //将目标整形数字intIPAddress转换为IP地址字符串
            //-1062731518 192.168.1.2 
            //-1062731517 192.168.1.3 
            if (intIPAddress >= 0)
            {
                tempIPAddress = intIPAddress;
            }
            else
            {
                tempIPAddress = intIPAddress + 1;
            }
            int s1 = tempIPAddress / 256 / 256 / 256;
            int s21 = s1 * 256 * 256 * 256;
            int s2 = (tempIPAddress - s21) / 256 / 256;
            int s31 = s2 * 256 * 256 + s21;
            int s3 = (tempIPAddress - s31) / 256;
            int s4 = tempIPAddress - s3 * 256 - s31;
            if (intIPAddress < 0)
            {
                s1 = 255 + s1;
                s2 = 255 + s2;
                s3 = 255 + s3;
                s4 = 255 + s4;
            }
            string strIPAddress = s1.ToString() + "." + s2.ToString() + "." + s3.ToString() + "." + s4.ToString();
            return strIPAddress;
        }




    }
}
