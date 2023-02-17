using System;
using System.Data;
using System.Linq; 
using Newtonsoft.Json;
using XS.Core2.FSO;

namespace XS.Core2
{
    public class JsonFile<T> where T:new() 
    {

        public T Model;
        private string sPath = "";
        public string Id = "";
        public JsonFile(string spath)
        {
            sPath = spath;
            if (FObject.IsExist(sPath, FsoMethod.File))
            {
                string json = FObject.ReadFile(sPath);
                Model =  JsonConvert.DeserializeObject<T>(json);
                Id = XsUtils.MD5(sPath);
            }
            else
            {
                Model = new T();
                Id = Guid.NewGuid().ToString();
            }

            
        }
         

        public void Save()
        {
           string json  = JsonConvert.SerializeObject(Model);
            FObject.WriteFileUtf8(sPath, json);
        }
    }
}