using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class PAConfigHelper
    {
        /// <summary>
        /// 获取appSettings里的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static string GetConfiguration(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }


    public class PAHelper
    {
        public static string NcPost(string url, string postCont, int timeOut=60, bool sign=false)
        {
            Encoding encoding = Encoding.GetEncoding("GBK");
            byte[] bytesToPost = encoding.GetBytes(postCont);
            string cookieheader = string.Empty;

            var cookieCon = new CookieContainer();

            #region 创建HttpWebRequest对象

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            #endregion

            #region 初始化HtppWebRequest对象

            httpRequest.CookieContainer = cookieCon;
            httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0;)";
            httpRequest.ContentType = "application/x-www-form-urlencoded";
            httpRequest.Method = "POST";
            httpRequest.Timeout = timeOut * 1000;
            if (sign)
            {
                httpRequest.ContentLength = bytesToPost.Length;
                httpRequest.ContentType = "INFOSEC_SIGN/1.0";
            }


            if (cookieheader.Equals(string.Empty))
            {
                httpRequest.CookieContainer.GetCookieHeader(new Uri(url));
            }
            else
            {
                httpRequest.CookieContainer.SetCookies(new Uri(url), cookieheader);
            }

            #endregion

            string stringResponse = "";
            try
            {

                #region 附加Post给服务器的数据到HttpWebRequest对象

                httpRequest.ContentLength = bytesToPost.Length;
                Stream requestStream = httpRequest.GetRequestStream();
                requestStream.Write(bytesToPost, 0, bytesToPost.Length);
                requestStream.Close();

                #endregion


                #region 读取服务器返回信息


                Stream responseStream = httpRequest.GetResponse().GetResponseStream();

                if (responseStream != null)
                {
                    using (
                        var responseReader = new StreamReader(responseStream, Encoding.GetEncoding("GBK")))
                    {
                        stringResponse = responseReader.ReadToEnd();
                    }
                    responseStream.Close();
                }

                #endregion
            }
            catch (Exception)
            {
                ;
            }
            return stringResponse;
        }



        public static void XmlReplace(ref string source, string searchStr, string targetStr)
        {
            searchStr = "<" + searchStr + ">";
            int p1 = source.IndexOf(searchStr, StringComparison.Ordinal);
            int p2 = source.IndexOf("</", p1 + searchStr.Length, StringComparison.Ordinal);
            if (p1 > 0 && p2 > p1)
            {
                string tmpStr = source.Substring(0, p1 + searchStr.Length) + targetStr + source.Substring(p2);
                source = tmpStr;
            }
        }
    }
}
