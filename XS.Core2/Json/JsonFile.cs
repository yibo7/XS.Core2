using System;
using System.Data;
using System.IO;
using System.Linq; 
using Newtonsoft.Json;
using XS.Core2.FSO;

namespace XS.Core2
{
    [Obsolete("这个类已经过时. 请使用BaseJsonFile替换.", true)]
    public class JsonFile<T>  where T: class, new ()   
    {

        public T Inst; 
        private string sPath = "";
        public string Id = ""; 
        public JsonFile(string spath)
        {
            FObject.ExistsDirectory(spath);
            sPath = spath;
            if (FObject.IsExist(sPath, FsoMethod.File))
            {
                string json = FObject.ReadFile(sPath);
                Inst = JsonConvert.DeserializeObject<T>(json);
                Id = XsUtils.MD5(sPath);
            }
            else
            {
                Inst = new T();
                Id = Guid.NewGuid().ToString();
            }

            
        } 
        public void Save()
        {
           string json  = JsonConvert.SerializeObject(Inst);
            FObject.WriteFileUtf8(sPath, json);
        }
    }
}