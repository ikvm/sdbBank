using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace sdbBank
{
  public  class PinganQuery
  {
      private static string apiUrl = PAConfigHelper.GetConfiguration("PALocalApiUrl");
        private static string  YQCode = PAConfigHelper.GetConfiguration("sdbYQCode");

        private static string sendStr="A001010101001010799000099990000000000053S001  123450120170411102040YQTEST20170411102040                                                                                                          000001            00000000000<?xml version=\"1.0\" encoding=\"GBK\"?><Result></Result>";

 
      private static string sendStrBalance = "A0010101010010107990000999900000000001084001  123450120170411105802YQTEST20170411105802                                                                                                          000001            00000000000<?xml version=\"1.0\" encoding=\"GBK\"?><Result><Account>0122100613675</Account> <CcyCode>RMB</CcyCode></Result>";


      private static string S001XML = "<?xml version=\"1.0\" encoding=\"GBK\"?><Result></Result>";


        /// <summary>
        /// 系统探测
        /// </summary>
        /// <returns></returns>
        public static string S001NormalQuery()
        {
            string postParams = "{0}";
            StringBuilder sb = new StringBuilder();
         
       // sb.Append(sendStrBalance);
            var str = YQHelp.asemblyYQPackets(YQCode, "S001", S001XML);
            sb.Append(str);
            string postData = string.Format(postParams, sb.ToString());
          var cc=  PAHelper.NcPost(apiUrl, postData);
            return cc;
        }


        /// <summary>
        /// 余额查询
        /// </summary>
        /// <returns></returns>
        public static string YQ4001NormalQuery(string Account)
        {
         
            string postParams = "{0}";
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"GBK\" ?><Result><Account>");
            sb.Append(Account);
            sb.Append("</Account><CcyCode>");
            sb.Append("RMB");
            sb.Append("</CcyCode></Result>");
            string S4001XML = string.Format(postParams, sb.ToString());
 
            var str = YQHelp.asemblyYQPackets(YQCode, "4001", S4001XML);
 
          var cc = PAHelper.NcPost(apiUrl, str);
            return cc;
        }


      /// <summary>
      /// 历史明细
      /// </summary> 
      /// <returns></returns>
        public static string YQ4013HisDetailQuery(string accountNo  , string BeginDate  , string EndDate )
      {

          string postParams = "{0}";
          StringBuilder sb = new StringBuilder();
          sb.Append("<?xml version=\"1.0\" encoding=\"GBK\" ?><Result><AcctNo>");
          sb.Append(accountNo);
          sb.Append("</AcctNo><CcyCode>");
          sb.Append("RMB");
          sb.Append("</CcyCode>");
          sb.Append("<BeginDate>"+ BeginDate + "</BeginDate>");
          sb.Append("<EndDate>" + EndDate + "</EndDate>");
            sb.Append("<PageNo>0</PageNo>");
          sb.Append("<PageSize>30</PageSize>");
          sb.Append("<OrderMode>002</OrderMode></Result>");
            string YQ4013XML = string.Format(postParams, sb.ToString());

          var str = YQHelp.asemblyYQPackets(YQCode, "4013", YQ4013XML);

          var cc = PAHelper.NcPost(apiUrl, str);
          return cc;
      }


      /// <summary>
      /// 当日明细
      /// </summary>
      /// <param name="accountNo"></param>
      /// <returns></returns>
      public static string YQ4008todayDetailQuery(string accountNo)
      {

          string postParams = "{0}";
          StringBuilder sb = new StringBuilder();
          sb.Append("<?xml version=\"1.0\" encoding=\"GBK\" ?><Result><AcctNo>");
          sb.Append(accountNo);
          sb.Append("</AcctNo><CcyCode>");
          sb.Append("RMB");
          sb.Append("</CcyCode>");
          sb.Append("<PageNo>0</PageNo>");
          sb.Append("<PageSize></PageSize></Result>");
          string YQ4008XML = string.Format(postParams, sb.ToString());

          var str = YQHelp.asemblyYQPackets(YQCode, "4008", YQ4008XML);

          var cc = PAHelper.NcPost(apiUrl, str);
          return cc;
      }




        /// <summary>
        /// 跨行快付  单笔
        /// </summary> 
        /// <returns></returns>

        public static string KHKF03(string InAcctNo, string InAcctName,string TranAmount,string OrderNumber)
        {
      string sdbYQAcctNo = PAConfigHelper.GetConfiguration("sdbYQAcctNo");
            string sdbYQCorpId = PAConfigHelper.GetConfiguration("sdbYQCorpId");
            string postParams = "{0}";
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"GBK\" ?><Result><OrderNumber>");

            sb.Append(OrderNumber);
            sb.Append("</OrderNumber>");
            sb.Append("<AcctNo>");
            sb.Append(sdbYQAcctNo);   //企业签约帐号
            sb.Append("</AcctNo>");

            sb.Append("<BusiType>");
            sb.Append("00000");   
            sb.Append("</BusiType>");

            //sb.Append("<CorpId>");
            //sb.Append(sdbYQCorpId);//单位代码
            //sb.Append("</CorpId>");

            sb.Append("<CcyCode>");
            sb.Append("RMB");
            sb.Append("</CcyCode>");
            sb.Append("<TranAmount>");
            sb.Append(TranAmount);
            sb.Append("</TranAmount>");
 
            sb.Append("<InAcctNo>");
            sb.Append(InAcctNo);
            sb.Append("</InAcctNo>");

            sb.Append("<InAcctName>");
            sb.Append(InAcctName);
            sb.Append("</InAcctName>");


            sb.Append("</Result>");

            string KHKF03XML=string.Format(postParams, sb.ToString());
            var str = YQHelp.asemblyYQPackets(YQCode, "KHKF03", KHKF03XML);

            var cc = PAHelper.NcPost(apiUrl, str);
            return cc;
        }


        /// <summary>
        /// 3.4 单笔付款结果查询[KHKF04]
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <param name="BussFlowNo"></param>
        /// <returns></returns>
        public static string KHKF04(string OrderNumber   , string BussFlowNo = "")
      {
          string sdbYQAcctNo = PAConfigHelper.GetConfiguration("sdbYQAcctNo");
   
          string postParams = "{0}";
          StringBuilder sb = new StringBuilder();
          sb.Append("<?xml version=\"1.0\" encoding=\"GBK\" ?><Result><AcctNo>");
          sb.Append(sdbYQAcctNo);   //企业签约帐号
            sb.Append("</AcctNo>");
          sb.Append("<OrderNumber>");
          sb.Append(OrderNumber); //20位订单号
            sb.Append("</OrderNumber>");


            //两者不能同时为空    只取上面一个的  BussFlowNo为空
            sb.Append("<BussFlowNo>");
            sb.Append(BussFlowNo); //银行业务流水号
            sb.Append("</BussFlowNo>");
            sb.Append("</Result>");
            string KHKF04XML = string.Format(postParams, sb.ToString());
            var str = YQHelp.asemblyYQPackets(YQCode, "KHKF04", KHKF04XML);

            var cc = PAHelper.NcPost(apiUrl, str);
            return cc;
        }


      #region 早期测试


            //3.1 系统状态探测 _无效
        public static object S001Query()
        {
            string postParams = "{0}";
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"GBK\" ?><Result></Result>");
         
            string postData = string.Format(postParams, sb.ToString());

            var cc = PAHelper.NcPost(apiUrl, postData);
            return cc;
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

        #endregion
    }
}
