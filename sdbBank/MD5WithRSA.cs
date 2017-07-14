


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.IO;

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
            RSACryptoServiceProvider rsa =null;
            try
            {
               
           rsa = objx5092.PrivateKey as RSACryptoServiceProvider;
           
            }
            catch (Exception e)
            {
                string priKey = "";
                string file = "E:\\tool\\410350260100001.key";
                priKey = File.ReadAllText(file);
                priKey = priKey.Replace("-----BEGIN RSA PRIVATE KEY-----", "")
                    .Replace("-----END RSA PRIVATE KEY-----", "");
                rsa = DecodeRSAPrivateKey(priKey);
            }
         
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




        #region 自行添加



        private static RSACryptoServiceProvider DecodeRSAPrivateKey(string privateKey)
        {
            var privateKeyBits = System.Convert.FromBase64String(privateKey);

            var RSA = new RSACryptoServiceProvider();
            var RSAparams = new RSAParameters();

            using (BinaryReader binr = new BinaryReader(new MemoryStream(privateKeyBits)))
            {
                byte bt = 0;
                ushort twobytes = 0;
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    throw new Exception("Unexpected value read binr.ReadUInt16()");

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    throw new Exception("Unexpected version");

                bt = binr.ReadByte();
                if (bt != 0x00)
                    throw new Exception("Unexpected value read binr.ReadByte()");

                RSAparams.Modulus = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Exponent = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.D = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.P = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Q = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DP = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DQ = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
            }

            RSA.ImportParameters(RSAparams);
            return RSA;
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else
            if (bt == 0x82)
            {
                highbyte = binr.ReadByte();
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }


        #endregion



    }
}