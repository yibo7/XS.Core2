using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace XS.Core2
{

    /// <summary>
    /// DataSet的序列化.
    /// </summary>
    public class DataSetHelper
    {
        /// <summary>
        /// DataSet序列化
        /// </summary>
        /// <param name="ds">需要序列化的DataSet</param>
        /// <returns></returns>
        public static byte[] GetBinaryFormatDataSet(DataSet ds)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(stream, ds);
            return stream.GetBuffer(); 


        }
        /// <summary>
        /// DataSet反序列化
        /// </summary>
        /// <param name="binaryData">需要反序列化的byte[]</param>
        /// <returns></returns>
        public static DataSet RetrieveDataSet(byte[] binaryData)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream(binaryData);
            System.Runtime.Serialization.IFormatter formatter = new BinaryFormatter();

            return formatter.Deserialize(stream) as DataSet; 


        }
    }
}
