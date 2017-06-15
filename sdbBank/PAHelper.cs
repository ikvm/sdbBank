using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace sdbBank
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
     


        /// <summary>
        /// 解释返回码与返回描述
        /// </summary> 
        /// <returns></returns>
        public static string getReturnXmlInfo(string source)
        {
            try
            {
                if (source.Length > 97)
                {
                    string tmpStr = source.Substring(87, 60).Trim();
                    return tmpStr;
                }
                return source;
            }
            catch (Exception e)
            {
                return "解释失败"+e;
            }
          }


        /// <summary>
        /// 取返回的xml
        /// </summary> 
        /// <returns></returns>
        public static string getReturnXmlStr(string source)
        {
            try
            {
                if (source.Length > 222)
                {
                    int p = source.IndexOf("<?",  StringComparison.Ordinal);
                    return source.Substring(p);
                }
                return source;
            }
            catch (Exception e)
            {
                return "解释失败" + e;
            }
        }



        /// <summary>
        /// 取帐户余额数值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>

        public static string getAccBalance(string source)
        {
            try
            {
                if (source.Length > 222)
                {
                    int p = source.IndexOf("<?", StringComparison.Ordinal);
                    var xmlstr= source.Substring(p);

                    YQ4001Model list = XmlDeserialize<YQ4001Model>(xmlstr);
                    return list.Balance;
                }
                return source;
            }
            catch (Exception e)
            {
                return "余额数值解释失败" + e;
            }
        }


        /// <summary>
        /// 当日明细__查询返回解释
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string get4008Result(string source)
        {
            try
            {
                if (source.Length > 222)
                {
                    int p = source.IndexOf("<?", StringComparison.Ordinal);
                    var xmlstr = source.Substring(p);

                    YQ4008Model list = XmlDeserialize<YQ4008Model>(xmlstr);
                    return list.PageRecCount;
                }
                return source;
            }
            catch (Exception e)
            {
                return "当日明细返回解释失败" + e;
            }
        }


        /// <summary>
        /// 取历史明细
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string get4013Result(string source)
        {
            try
            {
                if (source.Length > 222)
                {
                    int p = source.IndexOf("<?", StringComparison.Ordinal);
                    var xmlstr = source.Substring(p);

                    YQ4013Model list = XmlDeserialize<YQ4013Model>(xmlstr);
                    return list.AcctNo;
                }
                return source;
            }
            catch (Exception e)
            {
                return "历史明细返回解释失败" + e;
            }
        }




        /// <summary>
        /// 跨行快付提交返回_ 银行业务流水号
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string getKHKF03Result(string source)
        {
            try
            {
                if (source.Length > 222)
                {
                    int p = source.IndexOf("<?", StringComparison.Ordinal);
                    var xmlstr = source.Substring(p);

                    YQKHKF03Model list = XmlDeserialize<YQKHKF03Model>(xmlstr);
                    return list.BussFlowNo;
                }
                return source;
            }
            catch (Exception e)
            {
                return "跨行快付返回解释失败" + e;
            }
        }


        /// <summary>
        /// 查询跨行快付结果
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string getKHKF04Result(string source)
        {
            try
            {
                if (source.Length > 222)
                {
                    int p = source.IndexOf("<?", StringComparison.Ordinal);
                    var xmlstr = source.Substring(p);

                    YQKHKF04Model list = XmlDeserialize<YQKHKF04Model>(xmlstr);
                    var cc= list.Status;
                    if (cc != "20" & cc != "30")
                    {
                        return list.RetMsg;
                    }
                    return cc;
                }
                return source;
            }
            catch (Exception e)
            {
                return "跨行快付解释失败" + e;
            }
        }



        #region 工具方法

        public static string NcPost(string url, string postCont, int timeOut = 60, bool sign = false)
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
            //    httpRequest.ContentType = "application/x-www-form-urlencoded";
            httpRequest.ContentType = "text/xml; charset=GBK";
            httpRequest.Method = "POST";
            httpRequest.Timeout = timeOut * 1000;
            httpRequest.Accept = "text/xml";
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
            catch (Exception e)
            {
                return e.Message;
            }
            return stringResponse;
        }


        /// <summary>
        /// 20170531 修改了 ContentType等
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postCont"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static string NcPostUnionNew(string url, string postCont, int timeOut = 60 )
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
            httpRequest.UserAgent = "payClient";
              httpRequest.ContentType = "application/x-www-form-urlencoded";
 
            httpRequest.Method = "POST";
            httpRequest.Timeout = timeOut * 1000;
            httpRequest.Accept = "text/xml";
 


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
            catch (Exception e)
            {
                return e.Message;
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

        private static T XmlDeserialize<T>(string str) where T : class
        {
            object obj;
            using (System.IO.MemoryStream mem = new MemoryStream(Encoding.Default.GetBytes(str)))
            {
                using (XmlReader reader = XmlReader.Create(mem))
                {
                    XmlSerializer formatter = new XmlSerializer(typeof(T));
                    obj = formatter.Deserialize(reader);
                }
            }
            return obj as T;
        }
        #endregion

    }
}
