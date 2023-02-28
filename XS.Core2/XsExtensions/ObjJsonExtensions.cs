using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XS.Core2.XsExtensions
{
    /// <summary>
    /// 让任何类型都具备ToJsonString的功能
    /// </summary>
    static public class ObjJsonExtensions
    {
        private static JsonSerializerSettings s_jsonSettings = new JsonSerializerSettings
        {
            Culture = CultureInfo.InvariantCulture,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Include
        };
        /// <summary>
        /// 将类型转换成JSON字符
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonString<T>(this T obj)
        {
            return obj.ToJsonString(s_jsonSettings);
        }
        /// <summary>
        /// 将类型转换成JSON字符
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string ToJsonString<T>(this T obj, JsonSerializerSettings settings)
        {
            if (obj == null)
            {
                return "null";
            }

            return JsonConvert.SerializeObject(obj, settings);
        }

        public static T FromJsonString<T>(this Stream stream) where T : class
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            using StreamReader streamReader = new StreamReader(stream);
            return streamReader.ReadToEnd().ToJson<T>();
        }
    }
}
