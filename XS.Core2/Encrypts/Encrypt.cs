using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace XS.Core2.Encrypts
{
    /// <summary> 
    /// 加密
    /// </summary> 
    public class AES
    {
        //默认密钥向量
        private static byte[] Keys = { 0x41, 0x72, 0x65, 0x79, 0x6F, 0x75, 0x6D, 0x79, 0x53, 0x6E, 0x6F, 0x77, 0x6D, 0x61, 0x6E, 0x3F };

        public static string Encode(string encryptString, string encryptKey)
        {
            encryptKey = XsUtils.GetSubString(encryptKey, 32, "");
            encryptKey = encryptKey.PadRight(32, ' ');

            RijndaelManaged rijndaelProvider = new RijndaelManaged();
            rijndaelProvider.Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 32));
            rijndaelProvider.IV = Keys;
            ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();

            byte[] inputData = Encoding.UTF8.GetBytes(encryptString);
            byte[] encryptedData = rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);

            return Convert.ToBase64String(encryptedData);
        }

        public static string Decode(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = XsUtils.GetSubString(decryptKey, 32, "");
                decryptKey = decryptKey.PadRight(32, ' ');

                RijndaelManaged rijndaelProvider = new RijndaelManaged();
                rijndaelProvider.Key = Encoding.UTF8.GetBytes(decryptKey);
                rijndaelProvider.IV = Keys;
                ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();

                byte[] inputData = Convert.FromBase64String(decryptString);
                byte[] decryptedData = rijndaelDecrypt.TransformFinalBlock(inputData, 0, inputData.Length);

                return Encoding.UTF8.GetString(decryptedData);
            }
            catch
            {
                return "";
            }

        }

    }

    /// <summary> 
    /// 加密
    /// </summary> 
    public class DES
    {
        //默认密钥向量
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
        public static string Encode(string encryptString, string encryptKey)
        {
            encryptKey = XsUtils.GetSubString(encryptKey, 8, "");
            encryptKey = encryptKey.PadRight(8, ' ');
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());

        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
        public static string Decode(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = XsUtils.GetSubString(decryptKey, 8, "");
                decryptKey = decryptKey.PadRight(8, ' ');
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();

                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return "";
            }
        }
    }


    public class Base64
    {
        protected Base64()
        {
            //Avoid to be inited
        }

        #region Base64_Algorithm_Implement
        /// <summary>
        /// Create base64 char array using default base64 char array
        /// </summary>
        /// <param name="CreatePara"></param>
        /// <returns>return the new base64 char array</returns>
        private static char[] CreateBase64Char(ref char[] CreatePara)
        {
            char[] BaseTable = new char[64] {  'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
                                             'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                                             '0','1','2','3','4','5','6','7','8','9','+','/'};


            const int CREATE_TYPE = 8;
            byte bParaValue = 0;
            byte bTemp;
            for (int i = 0; i < CreatePara.Length; i++)
            {
                bTemp = (byte)CreatePara[i];
                switch (bTemp % CREATE_TYPE)
                {
                    case 1:
                        // 00000001
                        bTemp = (byte)(bTemp ^ 0x01);
                        break;
                    case 2:
                        // 00000010
                        bTemp = (byte)(bTemp ^ 0x02);
                        break;
                    case 3:
                        // 00000100
                        bTemp = (byte)(bTemp ^ 0x04);
                        break;
                    case 4:
                        // 00001000
                        bTemp = (byte)(bTemp ^ 0x08);
                        break;
                    case 5:
                        // 00010000
                        bTemp = (byte)(bTemp ^ 0x10);
                        break;
                    case 6:
                        // 00100000
                        bTemp = (byte)(bTemp ^ 0x20);
                        break;
                    case 7:
                        // 01000000
                        bTemp = (byte)(bTemp ^ 0x40);
                        break;
                    default:
                        // 10000000
                        bTemp = (byte)(bTemp ^ 0x80);
                        break;
                }
                bParaValue = (byte)(bParaValue ^ bTemp);
            }

            char chrTemp;
            int nIndex;

            switch (bParaValue % CREATE_TYPE)
            {
                case 1:
                    // Exechange 0 <--> 1, 2 <--> 3, 4 <--> 5, 6 <--> 7
                    for (int i = 0; i < BaseTable.Length / CREATE_TYPE; i++)
                    {
                        nIndex = i * CREATE_TYPE;
                        chrTemp = BaseTable[nIndex];
                        BaseTable[nIndex] = BaseTable[nIndex + 1];
                        BaseTable[nIndex + 1] = chrTemp;
                        chrTemp = BaseTable[nIndex + 2];
                        BaseTable[nIndex + 2] = BaseTable[nIndex + 3];
                        BaseTable[nIndex + 3] = chrTemp;
                        chrTemp = BaseTable[nIndex + 4];
                        BaseTable[nIndex + 4] = BaseTable[nIndex + 5];
                        BaseTable[nIndex + 5] = chrTemp;
                        chrTemp = BaseTable[nIndex + 6];
                        BaseTable[nIndex + 6] = BaseTable[nIndex + 7];
                        BaseTable[nIndex + 7] = chrTemp;
                    }
                    break;
                case 2:
                    // Exechange 0 <--> 2, 1 <--> 3, 4 <--> 6, 5 <--> 7
                    for (int i = 0; i < BaseTable.Length / CREATE_TYPE; i++)
                    {
                        nIndex = i * CREATE_TYPE;
                        chrTemp = BaseTable[nIndex];
                        BaseTable[nIndex] = BaseTable[nIndex + 2];
                        BaseTable[nIndex + 2] = chrTemp;
                        chrTemp = BaseTable[nIndex + 1];
                        BaseTable[nIndex + 1] = BaseTable[nIndex + 3];
                        BaseTable[nIndex + 3] = chrTemp;
                        chrTemp = BaseTable[nIndex + 4];
                        BaseTable[nIndex + 4] = BaseTable[nIndex + 6];
                        BaseTable[nIndex + 6] = chrTemp;
                        chrTemp = BaseTable[nIndex + 5];
                        BaseTable[nIndex + 5] = BaseTable[nIndex + 7];
                        BaseTable[nIndex + 7] = chrTemp;
                    }
                    break;
                case 3:
                    // Exechange 0 <--> 3, 1 <--> 2, 4 <--> 7, 5 <--> 6
                    for (int i = 0; i < BaseTable.Length / CREATE_TYPE; i++)
                    {
                        nIndex = i * CREATE_TYPE;
                        chrTemp = BaseTable[nIndex];
                        BaseTable[nIndex] = BaseTable[nIndex + 3];
                        BaseTable[nIndex + 3] = chrTemp;
                        chrTemp = BaseTable[nIndex + 1];
                        BaseTable[nIndex + 1] = BaseTable[nIndex + 2];
                        BaseTable[nIndex + 2] = chrTemp;
                        chrTemp = BaseTable[nIndex + 4];
                        BaseTable[nIndex + 4] = BaseTable[nIndex + 7];
                        BaseTable[nIndex + 7] = chrTemp;
                        chrTemp = BaseTable[nIndex + 5];
                        BaseTable[nIndex + 5] = BaseTable[nIndex + 6];
                        BaseTable[nIndex + 6] = chrTemp;
                    }
                    break;
                case 4:
                    // Mirror exechange
                    for (int i = 0; i < BaseTable.Length / CREATE_TYPE; i++)
                    {
                        nIndex = i * CREATE_TYPE;
                        chrTemp = BaseTable[nIndex];
                        BaseTable[nIndex] = BaseTable[nIndex + CREATE_TYPE - 1];
                        BaseTable[nIndex + CREATE_TYPE - 1] = chrTemp;
                        chrTemp = BaseTable[nIndex + 1];
                        BaseTable[nIndex + 1] = BaseTable[nIndex + CREATE_TYPE - 2];
                        BaseTable[nIndex + CREATE_TYPE - 2] = chrTemp;
                        chrTemp = BaseTable[nIndex + 2];
                        BaseTable[nIndex + 2] = BaseTable[nIndex + CREATE_TYPE - 3];
                        BaseTable[nIndex + CREATE_TYPE - 3] = chrTemp;
                        chrTemp = BaseTable[nIndex + 3];
                        BaseTable[nIndex + 3] = BaseTable[nIndex + CREATE_TYPE - 4];
                        BaseTable[nIndex + CREATE_TYPE - 4] = chrTemp;
                    }
                    break;
                case 5:
                    // Exechange 0 <--> 4, 1 <--> 5, 2 <--> 6, 3 <--> 7
                    for (int i = 0; i < BaseTable.Length / CREATE_TYPE; i++)
                    {
                        nIndex = i * CREATE_TYPE;
                        chrTemp = BaseTable[nIndex];
                        BaseTable[nIndex] = BaseTable[nIndex + 4];
                        BaseTable[nIndex + 4] = chrTemp;
                        chrTemp = BaseTable[nIndex + 1];
                        BaseTable[nIndex + 1] = BaseTable[nIndex + 5];
                        BaseTable[nIndex + 5] = chrTemp;
                        chrTemp = BaseTable[nIndex + 2];
                        BaseTable[nIndex + 2] = BaseTable[nIndex + 6];
                        BaseTable[nIndex + 6] = chrTemp;
                        chrTemp = BaseTable[nIndex + 3];
                        BaseTable[nIndex + 3] = BaseTable[nIndex + 7];
                        BaseTable[nIndex + 7] = chrTemp;
                    }
                    break;
                case 6:
                    // Exechange 0 <--> 5, 1 <--> 6, 2 <--> 7, 3 <--> 4
                    for (int i = 0; i < BaseTable.Length / CREATE_TYPE; i++)
                    {
                        nIndex = i * CREATE_TYPE;
                        chrTemp = BaseTable[nIndex];
                        BaseTable[nIndex] = BaseTable[nIndex + 5];
                        BaseTable[nIndex + 5] = chrTemp;
                        chrTemp = BaseTable[nIndex + 1];
                        BaseTable[nIndex + 1] = BaseTable[nIndex + 6];
                        BaseTable[nIndex + 6] = chrTemp;
                        chrTemp = BaseTable[nIndex + 2];
                        BaseTable[nIndex + 2] = BaseTable[nIndex + 7];
                        BaseTable[nIndex + 7] = chrTemp;
                        chrTemp = BaseTable[nIndex + 3];
                        BaseTable[nIndex + 3] = BaseTable[nIndex + 4];
                        BaseTable[nIndex + 4] = chrTemp;
                    }
                    break;
                case 7:
                    // Exechange 0 <--> 6, 1 <--> 7, 2 <--> 4, 3 <--> 5
                    for (int i = 0; i < BaseTable.Length / CREATE_TYPE; i++)
                    {
                        nIndex = i * CREATE_TYPE;
                        chrTemp = BaseTable[nIndex];
                        BaseTable[nIndex] = BaseTable[nIndex + 6];
                        BaseTable[nIndex + 6] = chrTemp;
                        chrTemp = BaseTable[nIndex + 1];
                        BaseTable[nIndex + 1] = BaseTable[nIndex + 7];
                        BaseTable[nIndex + 7] = chrTemp;
                        chrTemp = BaseTable[nIndex + 2];
                        BaseTable[nIndex + 2] = BaseTable[nIndex + 4];
                        BaseTable[nIndex + 4] = chrTemp;
                        chrTemp = BaseTable[nIndex + 3];
                        BaseTable[nIndex + 3] = BaseTable[nIndex + 5];
                        BaseTable[nIndex + 5] = chrTemp;
                    }
                    break;
                default:
                    break;
            }
            return BaseTable;
        }

        /// <summary>
        /// Encode string at specific parameter
        /// </summary>
        /// <param name="Para"></param>
        /// <param name="input"></param>
        /// <returns>return encoded string</returns>
        private static char[] GetEncoded(char[] Para, byte[] input)
        {
            char[] lookupTable;
            int length, length2;
            int blockCount;
            int paddingCount;
            length = input.Length;

            if (length % 3 == 0)
            {
                paddingCount = 0;
                blockCount = length / 3;
            }
            else
            {
                paddingCount = 3 - length % 3;//need to add padding
                blockCount = (length + paddingCount) / 3;
            }
            length2 = length + paddingCount;//or blockCount *3
            byte[] source2;
            source2 = new byte[length2];

            //copy data over insert padding
            for (int x = 0; x < length2; x++)
            {
                if (x < length)
                {
                    source2[x] = input[x];
                }
                else
                {
                    source2[x] = 0;
                }
            }
            byte b1, b2, b3;
            byte temp, temp1, temp2, temp3, temp4;
            byte[] buffer = new byte[blockCount * 4];
            char[] result = new char[blockCount * 4];
            for (int x = 0; x < blockCount; x++)
            {
                b1 = source2[x * 3];
                b2 = source2[x * 3 + 1];
                b3 = source2[x * 3 + 2];
                temp1 = (byte)((b1 & 252) >> 2);//first
                temp = (byte)((b1 & 3) << 4);
                temp2 = (byte)((b2 & 240) >> 4);
                temp2 += temp; //second
                temp = (byte)((b2 & 15) << 2);
                temp3 = (byte)((b3 & 192) >> 6);
                temp3 += temp; //third
                temp4 = (byte)(b3 & 63); //fourth
                buffer[x * 4] = temp1;
                buffer[x * 4 + 1] = temp2;
                buffer[x * 4 + 2] = temp3;
                buffer[x * 4 + 3] = temp4;
            }

            lookupTable = CreateBase64Char(ref Para);
            for (int x = 0; x < blockCount * 4; x++)
            {
                result[x] = sixbit2char(lookupTable, buffer[x]);
            }

            //covert last "A"s to "=", based on paddingCount
            switch (paddingCount)
            {
                case 0: break;
                case 1: result[blockCount * 4 - 1] = '='; break;
                case 2:
                    result[blockCount * 4 - 1] = '=';
                    result[blockCount * 4 - 2] = '=';
                    break;
                default: break;
            }

            return result;

        }

        private static char sixbit2char(char[] lookupTable, byte b)
        {
            if (b >= 0 && b <= 63)
            {
                return lookupTable[b];
            }
            else
            {
                //should not happen;
                return ' ';
            }
        }

        /// <summary>
        /// Decode string using specific parameter
        /// </summary>
        /// <param name="Para"></param>
        /// <param name="input"></param>
        /// <returns>If decoded successfully, return the decoded string; else return NULL</returns>
        private static byte[] GetDecoded(char[] Para, char[] input)
        {
            char[] source;
            char[] lookupTable;
            int length, length2, length3;
            int blockCount;
            int paddingCount;
            int temp = 0;
            source = input;
            length = input.Length;
            if (length % 4 != 0) return null; // The string is not encoded with my base64;

            //find how many padding there are
            while (input[length - temp - 1] == '=' && temp < 3)
            {
                temp++;
            }

            if (temp == 3) return null; // The string is not encoded with my base64;
            paddingCount = temp;
            //calculate the blockCount;
            //assuming all whitespace and carriage returns/newline were removed.
            blockCount = length / 4;
            length2 = blockCount * 3;
            byte[] buffer = new byte[length];//first conversion result
            byte[] buffer2 = new byte[length2];//decoded array with padding
            lookupTable = CreateBase64Char(ref Para);
            for (int x = 0; x < length; x++)
            {
                buffer[x] = char2sixbit(lookupTable, source[x]);
            }
            byte b, b1, b2, b3;
            byte temp1, temp2, temp3, temp4;
            for (int x = 0; x < blockCount; x++)
            {
                temp1 = buffer[x * 4];
                temp2 = buffer[x * 4 + 1];
                temp3 = buffer[x * 4 + 2];
                temp4 = buffer[x * 4 + 3];
                b = (byte)(temp1 << 2);
                b1 = (byte)((temp2 & 48) >> 4);
                b1 += b;
                b = (byte)((temp2 & 15) << 4);
                b2 = (byte)((temp3 & 60) >> 2);
                b2 += b;
                b = (byte)((temp3 & 3) << 6);
                b3 = temp4;
                b3 += b;
                buffer2[x * 3] = b1;
                buffer2[x * 3 + 1] = b2;
                buffer2[x * 3 + 2] = b3;
            }

            //remove paddings
            length3 = length2 - paddingCount;

            byte[] result = new byte[length3];

            for (int x = 0; x < length3; x++)
            {
                result[x] = buffer2[x];
            }
            return result;
        }



        private static byte char2sixbit(char[] lookupTable, char c)
        {
            if (c == '=') return 0;
            else
            {
                for (int x = 0; x < 64; x++)
                {
                    if (lookupTable[x] == c)
                        return (byte)x;
                }
                //should not reach here
                return 0;
            }
        }

        #endregion
        /// <summary>
        /// Encrypt data based on specific key
        /// </summary>
        /// <param name="Data">the data to be encrypted</param>
        /// <param name="Key">key data</param>
        /// <returns>If successfully, return encrypted string; else return NULL</returns>
        public static string EncryptData(string Data, string Key)
        {
            if (Data == null || Data == "") return null;

            if (Key == null || Key == "") return null;

            char[] chrEncrypted = GetEncoded(Key.ToCharArray(),
                Encoding.Unicode.GetBytes(Data));
            if (chrEncrypted != null)
                return new string(chrEncrypted);
            else
                return null;
        }
        /// <summary>
        /// Decrypt data based on specific key
        /// </summary>
        /// <param name="Data">the data to be decrypted</param>
        /// <param name="Key">key data</param>
        /// <returns>If successfully, return decrypted string; else return NULL</returns>
        public static string DecryptData(string Data, string Key)
        {
            if (Data == null || Data == "") return null;

            if (Key == null || Key == "") return null;

            byte[] bDecrypted = GetDecoded(Key.ToCharArray(),
                Data.ToCharArray());
            if (bDecrypted != null)
                return Encoding.Unicode.GetString(bDecrypted);
            else
                return null;
        }
    }
}
