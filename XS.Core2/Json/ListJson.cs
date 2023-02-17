using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XS.Core2
{

    /// <summary>
    /// List列表类的json处理
    /// </summary>
    public class ListJson
    {
       /// <summary>
        ///  List<T>转Json
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="data"></param>
       /// <returns></returns>
        public static string Obj2Json<T>(T data) 
        { 
            try
            { 
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(data.GetType()); 
                using (MemoryStream ms = new MemoryStream()) 
                { 
                    serializer.WriteObject(ms, data); 
                    return Encoding.UTF8.GetString(ms.ToArray()); 
                } 
            } 
            catch
            { 
                return null; 
            } 
        } 

        /// <summary>
        /// Json转List<T>
        /// </summary>
        /// <param name="json"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Object Json2Obj(String json,Type t) 
        { 
            try
            { 
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(t); 
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json))) 
                { 
             
                    return  serializer.ReadObject(ms); 
                } 
            } 
            catch
            { 
                return null; 
            } 
        }
        /// <summary>
        /// 单个对象转JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Json2Obj<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        } 


    }
}
