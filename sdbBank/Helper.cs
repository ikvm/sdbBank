using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
 
namespace sdbBank
{

   
    public class AsymmetricAlgorithmHelper<T>
        where T : AsymmetricAlgorithm, new()
    {



        protected static TResult Execute<TResult>(string key, Func<T, TResult> func)
        {
            using (T algorithm = new T())
            {
                algorithm.FromXmlString(key);
                return func(algorithm);
            }
        }
        /// <summary>
        /// 按默认规则生成公钥、私钥
        /// </summary>
        /// <param name="publicKey">公钥（Xml格式）</param>
        /// <param name="privateKey">私钥（Xml格式）</param>
        //public static void Create(out string publicKey, out string privateKey)
        //{
        //    KeyGenerator.CreateAsymmetricAlgorithmKey<T>(out publicKey, out privateKey);
        //}
    }
    public class RSAHelper : AsymmetricAlgorithmHelper<RSACryptoServiceProvider>
    {
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="publickey">公钥</param>
        /// <param name="content">加密前的原始数据</param>
        /// <param name="fOAEP">如果为 true，则使用 OAEP 填充（仅在运行 Microsoft Windows XP 或更高版本的计算机上可用）执行直接的 System.Security.Cryptography.RSA加密；否则，如果为 false，则使用 PKCS#1 1.5 版填充。</param>
        /// <returns>加密后的结果（base64格式）</returns>
        public static string Encrypt(string publickey, string content, bool fOAEP = false)
        {
            return Execute(publickey,
                algorithm => Convert.ToBase64String(algorithm.Encrypt(Encoding.UTF8.GetBytes(content), fOAEP)));
        }
        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privatekey">私钥</param>
        /// <param name="content">加密后的内容(base64格式)</param>
        /// <param name="fOAEP">如果为 true，则使用 OAEP 填充（仅在运行 Microsoft Windows XP 或更高版本的计算机上可用）执行直接的 System.Security.Cryptography.RSA加密；否则，如果为 false，则使用 PKCS#1 1.5 版填充。</param>
        /// <returns></returns>
        public static string Decrypt(string privatekey, string content, bool fOAEP = false)
        {
            return Execute(privatekey,
                algorithm => Encoding.UTF8.GetString(algorithm.Decrypt(Convert.FromBase64String(content), fOAEP)));
        }
        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="privatekey">私钥</param>
        /// <param name="content">需签名的原始数据(utf-8)</param>
        /// <param name="halg">签名采用的算法，如果传null，则采用MD5算法</param>
        /// <returns>签名后的值(base64格式)</returns>
        public static string SignData(string privatekey, string content, object halg = null)
        {
            return Execute(privatekey,
                algorithm => Convert.ToBase64String(algorithm.SignData(Encoding.UTF8.GetBytes(content), GetHalg(halg))));
        }
        /// <summary>
        /// RSA验签
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="content">需验证签名的数据(utf-8)</param>
        /// <param name="signature">需验证的签名字符串(base64格式)</param>
        /// <param name="halg">签名采用的算法，如果传null，则采用MD5算法</param>
        /// <returns></returns>
        public static bool VerifyData(string publicKey, string content, string signature, object halg = null)
        {
            return Execute(publicKey,
                algorithm => algorithm.VerifyData(Encoding.UTF8.GetBytes(content), GetHalg(halg), Convert.FromBase64String(signature)));
        }
        private static object GetHalg(object halg)
        {
            if (halg == null)
            {
                halg = "MD5";
            }
            return halg;
        }
        /// <summary>
        /// 生成公钥、私钥
        /// </summary>
        /// <param name="publicKey">公钥（Xml格式）</param>
        /// <param name="privateKey">私钥（Xml格式）</param>
        /// <param name="keySize">要生成的KeySize，支持的MinSize:384 MaxSize:16384 SkipSize:8</param>
        //public static void Create(out string publicKey, out string privateKey, int keySize = 1024)
        //{
        //    RSACryptoServiceProvider provider = new RSACryptoServiceProvider(keySize);
        //    KeyGenerator.CreateAsymmetricAlgorithmKey(out publicKey, out privateKey, provider);
        //}
    }
    public class DSAHelper : AsymmetricAlgorithmHelper<DSACryptoServiceProvider>
    {
        /// <summary>
        /// DSA签名
        /// </summary>
        /// <param name="privatekey">私钥</param>
        /// <param name="content">需签名的原始数据(utf-8)</param>
        /// <returns>签名后的值(base64格式)</returns>
        public static string SignData(string privatekey, string content)
        {
            return Execute(privatekey,
                algorithm => Convert.ToBase64String(algorithm.SignData(Encoding.UTF8.GetBytes(content))));
        }
        /// <summary>
        /// DSA验签
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="content">需验证签名的数据(utf-8)</param>
        /// <param name="signature">需验证的签名字符串(base64格式)</param>
        /// <returns></returns>
        public static bool VerifyData(string publicKey, string content, string signature)
        {
            return Execute(publicKey,
                algorithm => algorithm.VerifyData(Encoding.UTF8.GetBytes(content), Convert.FromBase64String(signature)));
        }
        /// <summary>
        /// 生成公钥、私钥
        /// </summary>
        /// <param name="publicKey">公钥（Xml格式）</param>
        /// <param name="privateKey">私钥（Xml格式）</param>
        /// <param name="keySize">要生成的KeySize，支持的MinSize:512 MaxSize:1024 SkipSize:64</param>
        //public static void Create(out string publicKey, out string privateKey, int keySize = 1024)
        //{
        //    DSACryptoServiceProvider provider = new DSACryptoServiceProvider(keySize);
        //    KeyGenerator.CreateAsymmetricAlgorithmKey(out publicKey, out privateKey, provider);
        //}
    }


    public class Helper
    {
    }
}
