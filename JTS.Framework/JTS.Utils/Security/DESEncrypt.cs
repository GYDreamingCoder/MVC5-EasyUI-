using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace JTS.Utils
{
    /// <summary>
    /// DES����/���ܹ����ࡣ
    /// </summary>
    public class DESEncrypt
    {
        private static string encryptKey = "km4250km";

        #region ========����========

        /// <summary>
        /// DES�����ܡ��ַ�����ʹ��ȱʡ��Կ��
        /// </summary>
        /// <param name="text">�ַ���</param>
        /// <returns>����string</returns>
        public static string Encrypt(string text)
        {
            return Encrypt(text, encryptKey);
        }
 
        /// <summary> 
        /// DES�����ܡ��ַ�����ʹ�ø�����Կ��
        /// </summary> 
        /// <param name="text">�ַ���</param> 
        /// <param name="sKey">��Կ�ַ���</param> 
        /// <returns>����string</returns> 
        public static string Encrypt(string text, string key)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                //rgbIV��rgbKey���Բ�һ��������ֻ��Ϊ�˼�㣬���߿��������޸�
                byte[] rgbIV = rgbKey;// Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] inputByteArray = Encoding.UTF8.GetBytes(text); 
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return text;
            }

        }

        #endregion

        #region ========����========


        /// <summary>
        /// DES�����ܡ��ַ�����ʹ��ȱʡ��Կ�� 
        /// </summary>
        /// <param name="text">�ַ���</param> 
        /// <returns>����string</returns>
        public static string Decrypt(string text)
        {
            return Decrypt(text, encryptKey);
        }

        /// <summary> 
        /// DES�����ܡ��ַ�����ʹ�ø�����Կ��
        /// </summary> 
        /// <param name="text">�ַ���</param> 
        /// <param name="sKey">��Կ�ַ���</param> 
        /// <returns>����string</returns> 
        public static string Decrypt(string text, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Convert.FromBase64String(text);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray()); 
        }

        #endregion
    }
}
