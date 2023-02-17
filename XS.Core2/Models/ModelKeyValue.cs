using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XS.Core2.Models
{
    [Serializable]
    public class ModelKeyValue
    {
        public ModelKeyValue()
        {
        }

        public ModelKeyValue(string k, string v)
        {
            Key = k;
            Value = v;
        }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
