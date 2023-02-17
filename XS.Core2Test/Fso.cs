using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using XS.Core2.FSO;

namespace XSCoreTest
{
    [TestClass]
    public class Fso
    {
         

        [TestMethod]
        public void GetFileListByTypesTest()
        {
            string[] sTyps = new[] { "txt", "log" };
            var lstFiles = FObject.GetFileListByTypes(@"D:\IIS日志", sTyps);

        }
    }
}