using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XS.Core2.XsExtensions
{
    public static class StringExtensions
    {
        public static dynamic ToJsonDynamic(this string str)
        {
            // 定义一个正则表达式
            string pattern = @"^(\\{.*\\}|\\[.*\\])$";
            // 使用 Regex 类进行匹配
            bool isJson = Regex.IsMatch(str, pattern);
            if (isJson)
            {
                try
                {
                    return JsonConvert.DeserializeObject<dynamic>(str);
                }
                catch (Exception ex)
                {
                    throw new Exception("字符串转成dynamic失败：" + ex.Message);
                }

            }
            else
            {
                throw new Exception("不是正确的json字符");
            }

        }
        public static JToken ToJsonJToken(this string str)
        {
            try
            {
                return JToken.Parse(str);
            }
            catch (JsonReaderException ex)
            {
                throw new Exception("转成json失败：" + ex.Message);
            }

        }
        //public static bool IsNullOrEmpty(this string str)
        //{
        //     return String.IsNullOrEmpty(str);
        //}
        
    }
}
