using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XS.Core2.Json
{
    /// <summary>
    /// 处理json文件，可以继承这个基类扩展配置字段
    /// </summary>
    public class BaseJsonFile
    {
        private readonly string _filePath;
        /// <summary>
        /// 构造函数是指定json文件路径
        /// </summary>
        /// <param name="filePath">json文件路径，可以是相对路径，如 conf/settings.json</param>
        protected BaseJsonFile(string filePath)
        {
            _filePath = filePath;
            if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                JsonConvert.PopulateObject(json, this);
            }
        }

        public void Save()
        {
            if (!string.IsNullOrWhiteSpace(_filePath) && File.Exists(_filePath))
            {
                var json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(_filePath, json, Encoding.UTF8);
            }
            else
            {
                LogHelper.Debug<BaseJsonFile>("保存json文件出错：json文件的路径为空！");
            }

        }
    }
}
