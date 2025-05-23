using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace XS.Core2.Encrypts
{
    public class Md5Helper
    {
        // 比较字符串的MD5哈希值
        public static bool VerifyMd5Hash(string input, string hash)
        {
            string hashOfInput = Md5Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);
        }

        // 计算字符串的MD5哈希值
        public static string Md5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        // 计算对象的MD5哈希值
        public static string Md5Hash(object obj)
        {
            byte[] b = ByteConvertHelper.Object2Bytes(obj); // 使用你的对象转字节数组的帮助类
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(b);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        // 计算文件的MD5哈希值
        public static string FileMd5Hash(string filePath)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open))
                {
                    byte[] hashBytes = md5.ComputeHash(file);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
        }


        /// <summary>
        /// SHA256函数
        /// </summary>
        /// /// <param name="str">原始字符串</param>
        /// <returns>SHA256结果</returns>
        public static string SHA_256(string str)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
                byte[] Result = sha256.ComputeHash(SHA256Data);
                return Convert.ToBase64String(Result);  //返回长度为44字节的字符串
            }
        }
    }

    // 示例：对象转字节数组的帮助类 (你需要根据你的实际情况实现)
    //public static class ByteConvertHelper
    //{
    //    public static byte[] Object2Bytes(object obj)
    //    {
    //        // 这里需要根据你的对象序列化方式来实现
    //        // 例如：使用BinaryFormatter, Newtonsoft.Json等
    //        // 这里只是一个示例，你需要替换为你的实际实现
    //        //throw new NotImplementedException();
    //        if (obj == null)
    //        {
    //            return new byte[0];
    //        }

    //        string str = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
    //        return Encoding.UTF8.GetBytes(str);
    //    }
    //}
}
