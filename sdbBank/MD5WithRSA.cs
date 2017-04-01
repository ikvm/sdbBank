 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace sdbBank
{
    public class   MD5WithRSA
    {
        private static char[] bcdLookup = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

     public   static string pemFile =SDKConfig.SignPFXPath;
        public static string RsaKeyPass =SDKConfig.sdbKeyPass;
        public static string sdbPaySign(string strdata)
        {

            return SignData(pemFile, RsaKeyPass, strdata);
        }


        /// <summary>
        /// 返回MD5WithRSA的签名字符串
        /// </summary>
        /// <param name="fileName">pfx证书文件的路径</param>
        /// <param name="password">pfx证书密码</param>
        /// <param name="strdata">待签名字符串</param>
        /// <param name="encoding">字符集,默认为ISO-8859-1</param>
        /// <returns>返回MD5WithRSA的签名字符串</returns>
        private static string SignData(string fileName, string password, string strdata, string encoding = "GBK")
        {
            X509Certificate2 objx5092;
            if (string.IsNullOrWhiteSpace(password))
            {
                objx5092 = new X509Certificate2(fileName);
            }
            else
            {
                objx5092 = new X509Certificate2(fileName, password);
            }
            RSACryptoServiceProvider rsa = objx5092.PrivateKey as RSACryptoServiceProvider;
            byte[] data = Encoding.GetEncoding(encoding).GetBytes(strdata);
            byte[] hashvalue = rsa.SignData(data, "MD5");//为证书采用MD5withRSA 签名
            return bytesToHexStr(hashvalue);///将签名结果转化为16进制字符串
        }
        /// <summary>
        /// 将签名结果转化为16进制字符串
        /// </summary>
        /// <param name="bcd">签名结果的byte数字</param>
        /// <returns>16进制字符串</returns>
        private static string bytesToHexStr(byte[] bcd)
        {
            StringBuilder s = new StringBuilder(bcd.Length * 2);
            for (int i = 0; i < bcd.Length; i++)
            {
                s.Append(bcdLookup[(bcd[i] >> 4) & 0x0f]);
                s.Append(bcdLookup[bcd[i] & 0x0f]);
            }
            return s.ToString();
        }


      

 



    }
}