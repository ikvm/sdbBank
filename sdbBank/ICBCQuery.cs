using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace sdbBank
{ 

    public class ICBCQuery
    {
        static string apiUrl = "https://corporbank.icbc.com.cn/servlet/ICBCINBSEBusinessServlet";
        static string cerPath = HttpContext.Current.Server.MapPath("~/Bank/zysggzyjyzx.pfx");
        static string cerPwd = "123456";


        #region 属性
        //pub
        //接口名称
        private string _APIName;
        public string APIName
        {
            get { return _APIName; }
            set { _APIName = value; }
        }
        //接口版本号
        private string _APIVersion;
        public string APIVersion
        {
            get { return _APIVersion; }
            set { _APIVersion = value; }
        }
        //in
        //订单号
        private string _orderNum;
        public string orderNum
        {
            get { return _orderNum; }
            set { _orderNum = value; }
        }
        //交易日期
        private string _tranDate;
        public string tranDate
        {
            get { return _tranDate; }
            set { _tranDate = value; }
        }
        //商家号码
        private string _ShopCode;
        public string ShopCode
        {
            get { return _ShopCode; }
            set { _ShopCode = value; }
        }
        //商城账号
        private string _ShopAccount;
        public string ShopAccount
        {
            get { return _ShopAccount; }
            set { _ShopAccount = value; }
        }
        //out
        //指令序号
        private string _tranSerialNum;
        public string tranSerialNum
        {
            get { return _tranSerialNum; }
            set { _tranSerialNum = value; }
        }
        //订单处理状态
        private string _tranStat;
        public string tranStat
        {
            get { return _tranStat; }
            set { _tranStat = value; }
        }
        //指令错误信息
        private string _bankRem;
        public string bankRem
        {
            get { return _bankRem; }
            set { _bankRem = value; }
        }


        //订单总金额
        private string _amount;
        public string amount
        {
            get { return _amount; }
            set { _amount = value; }
        }


        //支付币种
        private string _currType;
        public string currType
        {
            get { return _currType; }
            set { _currType = value; }
        }


        //返回通知日期时间
        private string _tranTime;
        public string tranTime
        {
            get { return _tranTime; }
            set { _tranTime = value; }
        }


        //收款人账号
        private string _PayeeAcct;
        public string PayeeAcct
        {
            get { return _PayeeAcct; }
            set { _PayeeAcct = value; }
        }


        //收款人户名
        private string _PayeeName;
        public string PayeeName
        {
            get { return _PayeeName; }
            set { _PayeeName = value; }
        }


        //校验联名标志
        private string _JoinFlag;
        public string JoinFlag
        {
            get { return _JoinFlag; }
            set { _JoinFlag = value; }
        }


        //商城联名标志
        private string _MerJoinFlag;
        public string MerJoinFlag
        {
            get { return _MerJoinFlag; }
            set { _MerJoinFlag = value; }
        }


        //客户联名标志
        private string _CustJoinFlag;
        public string CustJoinFlag
        {
            get { return _CustJoinFlag; }
            set { _CustJoinFlag = value; }
        }


        //联名会员号
        private string _CustJoinNum;
        public string CustJoinNum
        {
            get { return _CustJoinNum; }
            set { _CustJoinNum = value; }
        }


        //DDD商户签名证书id
        private string _CertID;


        public string CertID
        {
            get { return _CertID; }
            set { _CertID = value; }
        }


        public ICBCQuery()
        {


        }


        #endregion


        #region 方法体
        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="orderNum">订单号</param>
        /// <param name="tradeDate">订单日期(yyyyMMdd)</param>
        /// <param name="shopCode">商户代码</param>
        /// <param name="shopAccount">商户帐号</param>
        /// <returns></returns>
        public static object B2BQuery(string orderNum, string tradeDate, string shopCode, string shopAccount)
        {
            //gen post data
            string postParams = "APIName=EAPI&APIVersion=001.001.001.001&MerReqData={0}";
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"GBK\" standalone=\"no\" ?><ICBCAPI><in><orderNum>");
            sb.Append(orderNum);
            sb.Append("</orderNum><tranDate>");
            sb.Append(tradeDate);
            sb.Append("</tranDate><ShopCode>");
            sb.Append(shopCode);
            sb.Append("</ShopCode><ShopAccount>");
            sb.Append(shopAccount);
            sb.Append("</ShopAccount></in></ICBCAPI>");
            string postData = string.Format(postParams, sb.ToString());


            //验证证书,默认有效
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);


            //配置请求参数
            HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(apiUrl);
            wReq.ContentType = "application/x-www-form-urlencoded";
            wReq.Method = "POST";
            wReq.ClientCertificates.Add(new X509Certificate2(cerPath, cerPwd));
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
            if (stringResp.IndexOf("APIName") > -1)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(stringResp);
                var oneXml = xmlDoc.SelectSingleNode("ICBCAPI");
                ICBCQuery cInfo = new ICBCQuery();
                //pub
                var pubXml = oneXml.SelectSingleNode("pub");
                //接口名称
                cInfo.APIName = pubXml.SelectSingleNode("APIName").InnerText;
                //接口版本号
                cInfo.APIVersion = pubXml.SelectSingleNode("APIVersion").InnerText;
                //in
                var inXml = oneXml.SelectSingleNode("in");
                //订单号
                cInfo.orderNum = inXml.SelectSingleNode("orderNum").InnerText;
                //交易日期
                cInfo.tranDate = inXml.SelectSingleNode("tranDate").InnerText;
                //商家号码
                cInfo.ShopCode = inXml.SelectSingleNode("ShopCode").InnerText;
                //商城账号
                cInfo.ShopAccount = inXml.SelectSingleNode("ShopAccount").InnerText;
                //out
                var outXml = oneXml.SelectSingleNode("out");
                //指令序号
                cInfo.tranSerialNum = outXml.SelectSingleNode("tranSerialNum").InnerText;
                //订单处理状态
                cInfo.tranStat = outXml.SelectSingleNode("tranStat").InnerText;
                //指令错误信息
                cInfo.bankRem = outXml.SelectSingleNode("bankRem").InnerText;
                //订单总金额
                cInfo.amount = outXml.SelectSingleNode("amount").InnerText;
                //支付币种
                cInfo.currType = outXml.SelectSingleNode("currType").InnerText;
                //返回通知日期时间
                cInfo.tranTime = outXml.SelectSingleNode("tranTime").InnerText;
                //收款人账号
                cInfo.PayeeAcct = outXml.SelectSingleNode("PayeeAcct").InnerText;
                //收款人户名
                cInfo.PayeeName = outXml.SelectSingleNode("PayeeName").InnerText;
                //校验联名标志
                cInfo.JoinFlag = outXml.SelectSingleNode("JoinFlag").InnerText;
                //商城联名标志
                cInfo.MerJoinFlag = outXml.SelectSingleNode("MerJoinFlag").InnerText;
                //客户联名标志
                cInfo.CustJoinFlag = outXml.SelectSingleNode("CustJoinFlag").InnerText;
                //联名会员号
                cInfo.CustJoinNum = outXml.SelectSingleNode("CustJoinNum").InnerText;
                //DDD商户签名证书id
                cInfo.CertID = outXml.SelectSingleNode("CertID").InnerText;
                return cInfo;
            }
            return stringResp;
        }
        // The following method is invoked by the RemoteCertificateValidationDelegate.
        public static bool ValidateServerCertificate(
              object sender,
              X509Certificate certificate,
              X509Chain chain,
              SslPolicyErrors sslPolicyErrors)
        {
            //if (sslPolicyErrors == SslPolicyErrors.None)
            //    return true;


            //Console.WriteLine("Certificate error: {0}", sslPolicyErrors);


            // Do not allow this client to communicate with unauthenticated servers.
            //return false;
            return true;
        }
        #endregion


        #region 枚举
        /// <summary>
        /// 错误代码含义
        /// </summary>
        public enum ErrorCode
        {
            API查询的订单不存在 = 40972,
            API查询过程中系统异常 = 40973,
            API查询系统异常 = 40976,
            商户证书信息错 = 40977,
            解包商户请求数据报错 = 40978,
            查询的订单不存在 = 40979,
            API查询过程中系统出现异常 = 40980,
            给商户打包返回数据错 = 40981,
            系统错误 = 40982,
            查询的订单不唯一 = 40983,
            请求数据中接口名错误 = 40987,
            商户代码或者商城账号有误 = 40947,
            商城状态非法 = 40948,
            商城类别非法 = 40949,
            商城应用类别非法 = 40950,
            商户证书id状态非法 = 40951,
            商户证书id未绑定 = 40952,
            商户id权限非法 = 40953,
            检查商户状态时数据库异常 = 40954
        }
        /// <summary>
        /// 指令状态含义
        /// </summary>
        public enum CommandState
        {
            指令处理完成转账成功 = 3,
            指令处理失败转账未完成 = 4,
            指令超过支付人的限额正在等待主管会计批复 = 6,
            指令超过支付人的限额正在等待主管会计第二次批复 = 7,
            指令超过支付人的限额被主管会计否决 = 8,
            银行正在处理可疑 = 9
        }
        #endregion
    }
}
