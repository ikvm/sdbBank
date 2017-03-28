using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace sdbBank
{
    public class Base64
    {

        /// <summary>
        /// Base64加密
        /// </summary>
        ///     <param name="source">待加密的明文</param>
        /// <param name="code_type">加密采用的编码方式</param>
        /// <returns></returns>
        public static string EncodeBase64(string source,string code_type)
        {
           
            string encode = "";
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string EncodeBase64(string source)
        {
            byte[] bytes = Encoding.Default.GetBytes(source);
            var cc = Convert.ToBase64String(bytes);
            return cc;
        }

        /// <summary>
        /// Base64解密
        /// </summary>
  /// <param name="result">待解密的密文</param>
        ///   <param name="code_type">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(string result, string code_type)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = Encoding.GetEncoding(code_type).GetString(bytes);
         
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(string result)
        {
            return DecodeBase64Default( result, Encoding.UTF8);
        }


        public static string DecodeBase64Default(string result, Encoding code_type)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = code_type.GetString(bytes);

            }
            catch
            {
                decode = result;
            }
            return decode;
        }
    }
}