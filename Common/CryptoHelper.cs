/**********************************************************************************
* 创 建 者：江腾
* 创建日期：2017/7/13 14:07:40
* 类 名 称：CryptoHelper
* 版 本 号：v1.0.0.0
* 功能说明：
* 命名空间：Common
* 修改历史：
* *********************************************************************************
* 修 改 者：
* 修改说明：
* *********************************************************************************
* Copyright  All right reserved
* *********************************************************************************/
using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Common
{
    /// <summary>
    /// 加解密帮助类
    /// </summary>
    public class CryptoHelper
    {
        /// <summary>
        /// 进度方法委托
        /// </summary>
        /// <param name="FileSize">文件大小</param>
        /// <param name="CheckSize">文件进度</param>
        /// <param name="MD5">完成后返回MD5值</param>
        public delegate void Method(int FileSize, int CheckSize, string MD5);

        /// <summary>
        /// 字符串加密(DES算法)。
        /// </summary>
        /// <param name="str">待加密的字符串。</param>
        /// <param name="key">密匙字符串。</param>
        /// <returns>加密后的字符串(加密失败则返回原字符串)。</returns>
        public static string DesEncrypt(string str, string key)
        {
            string encryptStr;
            string curKey = key;
            string tempKey = key;
            string codeBound = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            foreach (char t in curKey)
            {
                if (codeBound.IndexOf(t) == -1)
                {
                    tempKey = tempKey.Replace(t.ToString(), "");
                }
            }

            if (tempKey.Length < 32)
            {
                tempKey = tempKey.PadRight(32, 'Z');
            }

            if (tempKey.Length > 32)
            {
                tempKey = tempKey.Substring(0, 32);
            }

            var keyBytes = Convert.FromBase64String(tempKey);

            try
            {
                var tripleDes = TripleDES.Create();
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, tripleDes.CreateEncryptor(keyBytes, keyBytes), CryptoStreamMode.Write);
                var sw = new StreamWriter(cs);
                sw.Write(str);
                sw.Flush();
                cs.FlushFinalBlock();
                ms.Flush();
                var ba = ms.ToArray();
                sw.Dispose();
                cs.Dispose();
                ms.Dispose();
                tripleDes.Clear();
                encryptStr = Convert.ToBase64String(ba);
            }
            catch
            {
                encryptStr = str;
            }

            return encryptStr;
        }
        /// <summary>
        /// 字符串解密(DES算法)。
        /// </summary>
        /// <param name="str">待解密字符串。</param>
        /// <param name="key">密匙字符串。</param>
        /// <returns>解密后的字符串(解密失败则返回原字符串)。</returns>
        public static string DesDecrypt(string str, string key)
        {
            string decrypStr;
            string curKey = key;
            string tempKey = key;
            string codeBound = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            foreach (char t in curKey)
            {
                if (codeBound.IndexOf(t) == -1)
                {
                    tempKey = tempKey.Replace(t.ToString(), "");
                }
            }

            if (tempKey.Length < 32)
            {
                tempKey = tempKey.PadRight(32, 'Z');
            }

            if (tempKey.Length > 32)
            {
                tempKey = tempKey.Substring(0, 32);
            }

            Byte[] keyBytes = Convert.FromBase64String(tempKey);

            try
            {
                TripleDES tripleDes = TripleDES.Create();
                Byte[] buffer = Convert.FromBase64String(str);
                MemoryStream ms = new MemoryStream(buffer);
                CryptoStream cs = new CryptoStream(ms, tripleDes.CreateDecryptor(keyBytes, keyBytes), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                decrypStr = sr.ReadToEnd();
                sr.Dispose();
                cs.Dispose();
                ms.Dispose();
                tripleDes.Clear();
            }
            catch
            {
                decrypStr = str;
            }

            return decrypStr;
        }

        /// <summary>
        /// 字符串加密(DES算法)。
        /// </summary>
        /// <param name="str">待加密的字符串。</param>
        /// <returns>加密后的字符串(加密失败则返回原字符串)。</returns>
        public static string DesEncrypt(string str)
        {
            string encryptStr;
            Byte[] key = { 0x40, 0x9D, 0x68, 0x6D, 0x67, 0x27, 0x53, 0x32, 0x13, 0xD2, 0xB1, 0x92, 0xC4, 0xC7, 0xEF, 0xE1, 0xEC, 0xBB, 0x2B, 0xCD, 0xC0, 0x1D, 0xD9, 0x4F };
            Byte[] iv = { 0x1E, 0x73, 0x34, 0xA8, 0x96, 0x1C, 0x6F, 0xB0, 0xF0, 0x55, 0x91, 0x3D, 0xB3, 0x6D, 0x4A, 0xEA, 0xD3, 0x8F, 0x20, 0xB4, 0x79, 0x51, 0x71, 0x40 };

            try
            {
                TripleDES tripleDes = TripleDES.Create();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, tripleDes.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                StreamWriter sw = new StreamWriter(cs);
                sw.Write(str);
                sw.Flush();
                cs.FlushFinalBlock();
                ms.Flush();
                Byte[] ba = ms.ToArray();
                sw.Dispose();
                cs.Dispose();
                ms.Dispose();
                tripleDes.Clear();
                encryptStr = Convert.ToBase64String(ba);
            }
            catch
            {
                encryptStr = str;
            }

            return encryptStr;
        }
        /// <summary>
        /// 字符串解密(DES算法)。
        /// </summary>
        /// <param name="str">待解密字符串。</param>
        /// <returns>解密后的字符串(解密失败则返回原字符串)。</returns>
        public static string DesDecrypt(string str)
        {
            string decrypStr;
            Byte[] key = { 0x40, 0x9D, 0x68, 0x6D, 0x67, 0x27, 0x53, 0x32, 0x13, 0xD2, 0xB1, 0x92, 0xC4, 0xC7, 0xEF, 0xE1, 0xEC, 0xBB, 0x2B, 0xCD, 0xC0, 0x1D, 0xD9, 0x4F };
            Byte[] iv = { 0x1E, 0x73, 0x34, 0xA8, 0x96, 0x1C, 0x6F, 0xB0, 0xF0, 0x55, 0x91, 0x3D, 0xB3, 0x6D, 0x4A, 0xEA, 0xD3, 0x8F, 0x20, 0xB4, 0x79, 0x51, 0x71, 0x40 };

            try
            {
                TripleDES tripleDes = TripleDES.Create();
                Byte[] buffer = Convert.FromBase64String(str);
                MemoryStream ms = new MemoryStream(buffer);
                CryptoStream cs = new CryptoStream(ms, tripleDes.CreateDecryptor(key, iv), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                decrypStr = sr.ReadToEnd();
                sr.Dispose();
                cs.Dispose();
                ms.Dispose();
                tripleDes.Clear();
            }
            catch
            {
                decrypStr = str;
            }

            return decrypStr;
        }


        /// <summary>
        /// 字符串加密(MD5算法)。
        /// </summary>
        /// <param name="str">待加密的字符串。</param>
        /// <returns>加密后的字符串(加密失败则返回原字符串)。</returns>
        public static string Md5(string str)
        {
            string tmpStr = str;
            string returnStr = "";

            try
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(tmpStr));
                md5.Clear();

                foreach (byte t in s)
                {
                    returnStr += t.ToString("X").PadLeft(2, '0');
                }
            }
            catch
            {
                returnStr = str;
            }

            return returnStr;
        }

        /// <summary>
        /// 取文件的MD5算法,不带回调
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="BufferSize">自定缓冲区大小,单位KB</param>
        /// <returns></returns>
        public static string FileMd5(string path, int BufferSize)
        {
            if (!File.Exists(path))
                throw new ArgumentException(string.Format("<{0}>, 不存在", path));
            int bufferSize = 1024 * BufferSize;//自定义缓冲区大小16K  
            byte[] buffer = new byte[bufferSize];
            Stream inputStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider();
            int readLength = 0;//每次读取长度  
            var output = new byte[bufferSize];
            //MessageBox.Show(inputStream.Length.ToString());                
            while ((readLength = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {

                //计算MD5  
                hashAlgorithm.TransformBlock(buffer, 0, readLength, output, 0);
            }
            //完成最后计算，必须调用(由于上一部循环已经完成所有运算，所以调用此方法时后面的两个参数都为0)  
            hashAlgorithm.TransformFinalBlock(buffer, 0, 0);
            string md5 = BitConverter.ToString(hashAlgorithm.Hash);
            hashAlgorithm.Clear();
            inputStream.Close();
            md5 = md5.Replace("-", "");
            return md5;
        }

        /// <summary>
        /// 取大文件的MD5值
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="Method">委托回调</param>
        /// <param name="BufferSize">缓冲区大小KB</param>
        public static void FileMd5(string path, Method Method, int BufferSize)
        {
            if (!File.Exists(path))
                throw new ArgumentException(string.Format("<{0}>, 不存在", path));
            int bufferSize = 1024 * BufferSize;//自定义缓冲区大小16K  
            byte[] buffer = new byte[bufferSize];
            Stream inputStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider();
            int readLength = 0;//每次读取长度  
            var output = new byte[bufferSize];
            //MessageBox.Show(inputStream.Length.ToString());  
            int buff = 0;
            while ((readLength = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                //计算MD5  
                hashAlgorithm.TransformBlock(buffer, 0, readLength, output, 0);
                Method((int)(inputStream.Length / 1024), (int)(buff / 1024), "");
                buff += buffer.Length;
            }
            //完成最后计算，必须调用(由于上一部循环已经完成所有运算，所以调用此方法时后面的两个参数都为0)  
            hashAlgorithm.TransformFinalBlock(buffer, 0, 0);
            string md5 = BitConverter.ToString(hashAlgorithm.Hash);
            hashAlgorithm.Clear();
            md5 = md5.Replace("-", "");
            Method((int)(inputStream.Length / 1024), (int)(inputStream.Length / 1024), md5);
            inputStream.Close();
        }
        /// <summary>
        /// 计算文件流的MD5
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetStreamMD5(Stream stream)
        {
            MD5 md5Hasher = new MD5CryptoServiceProvider();
            /*计算指定Stream对象的哈希值*/
            byte[] arrbytHashValue = md5Hasher.ComputeHash(stream);
            /*由以连字符分隔的十六进制对构成的String，其中每一对表示value中对应的元素；例如“F-2C-4A”*/
            string strHashData = System.BitConverter.ToString(arrbytHashValue).Replace("-", "");
            return strHashData;

        }
        /// <summary>
        /// 混合加密(MD5+DES算法)。
        /// </summary>
        /// <param name="str">待加密的字符串。</param>
        /// <returns>加密后的字符串(加密失败则返回原字符串)。</returns>
        public static string MixEncrypt(string str)
        {
            string tmpStr = DesEncrypt(str);
            string returnStr = "";

            try
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(tmpStr));
                md5.Clear();

                foreach (byte t in s)
                {
                    returnStr += t.ToString("X").PadLeft(2, '0');
                }
            }
            catch
            {
                returnStr = str;
            }

            return returnStr;
        }

        public static string Sha256To16(string str)
        {
            SHA256 sha256 = new SHA256CryptoServiceProvider();//建立一个SHA256  
            byte[] source = Encoding.Default.GetBytes(str);//将字串转Byte[]  
            byte[] crypto = sha256.ComputeHash(source);//進行SHA256加密  
            string result = string.Empty;
            foreach (byte t in crypto)
            {
                result += Convert.ToString(t, 16).PadLeft(2, '0');
            }
            return result;
        }

        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string Sha256Password(string str, string guid)
        {
            return Sha256To16(str + guid);
        }
    }
}
