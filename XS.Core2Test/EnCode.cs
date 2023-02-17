
using XS.Core2;
using XS.Core2.Encrypts;

namespace XSCoreTest
{
    [TestClass]
    public class EnCodeTest
    {
        
        [TestMethod]
        public void B64Encode()
        {
            string key = "atq@cqs2022";
            string str = Base64.EncryptData("62179", key);
            Console.WriteLine($"执行:{str}");

        }
        [TestMethod]
        public void B64Decode()
        {
            string key = "atq@cqs2022";
            string str = Base64.DecryptData("KnH1HEDHK3H%20HH==", key);
            Console.WriteLine($"解密密结果:{str}");

        }

        [TestMethod]
        public void AesEncode()
        {
            string key = "fsdfsdf@fsdfsdf";
            RunTimeWatch wt = new RunTimeWatch();
            wt.start();
            for (int i = 0; i < 10000; i++)
            {
                string str = AES.Encode("我是中国人", key);
                
            }
            Console.WriteLine($"执行时间:{wt.endmillisecond()}");

        }
        [TestMethod]
        public void AesDecode()
        {
            string key = "fsdfsdf@fsdfsdf";
            string str = AES.Decode("xuyvrCUmWszAlVgmclF54g==", key);
            Console.WriteLine($"解密密结果:{str}");

        }
        [TestMethod]
        public void UrlDecode()
        { 
            string str = XsUtils.UrlDecode("KnH1HEDHK3H%2bHH%3d%3d");
            Console.WriteLine($"解密密结果:{str}");

        }
    }
}