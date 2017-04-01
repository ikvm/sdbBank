using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Test
{
  public  class PinganQuery
  {
      private static string apiUrl = PAConfigHelper.GetConfiguration("PALocalApiUrl");


        public static string S001NormalQuery()
        {
            string postParams = "{0}";
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><Result></Result>");

            string postData = string.Format(postParams, sb.ToString());
          var cc=  PAHelper.NcPost(apiUrl, postData);
            return cc;
        }


        //3.1 系统状态探测
        public static object S001Query()
        {
            string postParams = "{0}";
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"GBK\" ?><Result></Result>");
         
            string postData = string.Format(postParams, sb.ToString());

            //配置请求参数
            HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(apiUrl);
            wReq.ContentType = "application/x-www-form-urlencoded";
            wReq.Method = "POST";
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(postData);
            wReq.ContentLength = data.Length;
            Stream reqStream = wReq.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();
            //获取结果
            WebResponse wResp = wReq.GetResponse();
            Stream respStream = wResp.GetResponseStream();
            string stringResp = string.Empty;
            if (respStream != null)
            {
                using (StreamReader respReader = new StreamReader(respStream, Encoding.GetEncoding("GBK")))
                {
                    stringResp = HttpContext.Current.Server.UrlDecode(respReader.ReadToEnd());
                }
                respStream.Close();
            }
            if (stringResp.IndexOf("Result") > -1)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(stringResp);
                var pubXml = xmlDoc.SelectSingleNode("Result");
                var Result = pubXml.SelectSingleNode("Result").InnerText;
            }
            return stringResp;
        }


        public static object Account4001Query(string Account)
        {
            string postParams = "{0}";
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"GBK\" ?><Result><Account>");
            sb.Append(Account);
            sb.Append("</Account><CcyCode>");
            sb.Append("RMB");
            sb.Append("</CcyCode></Result>");
            string postData = string.Format(postParams, sb.ToString());

            //配置请求参数
            HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(apiUrl);
            wReq.ContentType = "application/x-www-form-urlencoded";
            wReq.Method = "POST";
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(postData);
            wReq.ContentLength = data.Length;
            Stream reqStream = wReq.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();
            //获取结果
            WebResponse wResp = wReq.GetResponse();
            Stream respStream = wResp.GetResponseStream();
            string stringResp = string.Empty;
            if (respStream != null)
            {
                using (StreamReader respReader = new StreamReader(respStream, Encoding.GetEncoding("GBK")))
                {
                    stringResp = HttpContext.Current.Server.UrlDecode(respReader.ReadToEnd());
                }
                respStream.Close();
            }
            if (stringResp.IndexOf("Balance") > -1)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(stringResp);
                var pubXml = xmlDoc.SelectSingleNode("Balance");
             var Balance = pubXml.SelectSingleNode("Balance").InnerText;
            }
            return stringResp;
        }


    }
}
