using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace XS.Core2
{
    /// <summary>
    /// 工具类：对象与二进制流间的转换
    /// </summary>
    public class ByteConvertHelper
    {
        /// <summary>
        /// 将对象转换为byte数组
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="obj">被转换对象</param>
        /// <returns>转换后byte数组</returns>
        public static byte[] Object2Bytes<T>(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            // 使用 System.Text.Json 进行序列化
            return JsonSerializer.SerializeToUtf8Bytes(obj);
        }

        /// <summary>
        /// 将byte数组转换成对象
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="buff">被转换byte数组</param>
        /// <returns>转换完成后的对象</returns>
        public static T Bytes2Object<T>(byte[] buff)
        {
            if (buff == null || buff.Length == 0)
                throw new ArgumentNullException(nameof(buff));

            // 使用 System.Text.Json 进行反序列化
            return JsonSerializer.Deserialize<T>(buff)
                   ?? throw new InvalidOperationException("反序列化失败");
        }
    }
}
