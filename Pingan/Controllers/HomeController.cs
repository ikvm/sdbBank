using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using com.ecc.emp.data;
using com.sdb.payclient.core;
using ikvm.extensions;
using sdbBank;
using BankPackage;
using System.Data;
using Aspose.Cells;
using Codeplex.Data;

namespace Pingan.Controllers
{
    public class HomeController : Controller
    {
        #region old,放弃

        /// <summary>
        /// 取公钥串
        /// </summary>
        /// <returns></returns>

        public ActionResult GetRsaPubkey()
        {
              var loca = "E:\\tool\\公钥.cer";
            //        var loca = "E:\\tool\\私钥.pfx";
            
                   var cc = RSAFromPkcs8.GetPublicKeyCer(loca);
         //   var cc = RSAFromPkcs8.GetPublicKey(loca, "031115");
            return Content(cc);
        }


        public ActionResult testHello()
        {
            String encoding = "GBK";
            String orig = "PGtDb2xsIGlkPSJvdXRwdXQiIGFwcGVuZD0iZmFsc2UiPjxmaWVsZCBpZD0ic3RhdHVzIiB2YWx1%0AZT0iMDEiLz48ZmllbGQgaWQ9ImRhdGUiIHZhbHVlPSIyMDE0MDUwOTA4NTUwMiIvPjxmaWVsZCBp%0AZD0iY2hhcmdlIiB2YWx1ZT0iMTAiLz48ZmllbGQgaWQ9Im1hc3RlcklkIiB2YWx1ZT0iMjAwMDMx%0AMTE0NiIvPjxmaWVsZCBpZD0ib3JkZXJJZCIgdmFsdWU9IjIwMDAzMTExNDYyMDE0MDUwOTI1OTk1%0AMTE0Ii8%2BPGZpZWxkIGlkPSJjdXJyZW5jeSIgdmFsdWU9IlJNQiIvPjxmaWVsZCBpZD0iYW1vdW50%0AIiB2YWx1ZT0iLjAxIi8%2BPGZpZWxkIGlkPSJwYXlkYXRlIiB2YWx1ZT0iMjAxNDA1MDkwODU1MzAi%0ALz48ZmllbGQgaWQ9InJlbWFyayIgdmFsdWU9IiIvPjxmaWVsZCBpZD0ib2JqZWN0TmFtZSIgdmFs%0AdWU9IktIcGF5Z2F0ZSIvPjxmaWVsZCBpZD0idmFsaWR0aW1lIiB2YWx1ZT0iMCIvPjwva0NvbGw%2B%0A";
            //模拟银行返回通知原始数据，实际页面接收程序应为：
            //String sign = request.getParameter("sign");
            String sign = "MjY5YzJlMDBhMzcyZTJkNWJjYjAxMzhmNGMxNmRkNDVjNjVjYTY3YzhiMjc1NTZhNTk0MTI0MzE5%0AN2Q1MWZkNWI5OTMxNzJhZTJiZDEyNDNmMjE3ZTk4MjU1N2E2YzAzOGI1YjI2YTQ0ZWU0M2EyNjUx%0AZTdmNjk2NDMzMDZhNTM5Y2NjMDM0YzJjZjJjZGE2ZjZlOTE1NTU3MzE1NzYxOGE4NGI1YTAwNTZi%0AODg4ZjVlMDdlMmNjODlmNzUyNzVmMGFmZDAzMWY4MDg3MjRjNjc0ZGE0MmRjNjYzNTM1YjM2MDFi%0ANDA4ZjllYWI4YjgxNDI4Y2E4NWM1NjMxMzA2ZA%3D%3D%0A";
            bool result = false;
            orig = System.Web.HttpUtility.UrlDecode(orig, Encoding.GetEncoding("GBK"));
            sign = System.Web.HttpUtility.UrlDecode(sign, Encoding.GetEncoding("GBK"));


            orig = Base64.DecodeBase64(orig, encoding);
            sign = Base64.DecodeBase64(sign, encoding);
            //    testRSA t = new testRSA();
            //     var cc = t.JavaRsaVerify(orig, sign);
            //  result= SignCheck.JavaRsaVerify(orig, sign);
            result = SignCheck.verifyData(orig, sign);

            return Content(result.toString());
        }

      


        public string testJava()
        {
            var cc = java.lang.System.getProperties().getProperty("Java.class.path");
            java.lang.System.getProperties().setProperty("Java.class.path", "E:\\tool");

            com.ecc.emp.data.KeyedCollection input = new com.ecc.emp.data.KeyedCollection("input");
            com.ecc.emp.data.KeyedCollection signDataput = new com.ecc.emp.data.KeyedCollection("signDataput");
            // com.sdb.payclient.core.PayclientInterfaceUtil util = new com.sdb.payclient.core.PayclientInterfaceUtil();

            string timestamp = Util.ConvertDateTimeInt(DateTime.Now).ToString();


            input.put("masterId", "2000311146");
            //    input.put("orderId", "2000311146" + "20170321" + GenUniqueString());
            input.put("orderId", "20003111462014050880763832");

            input.put("currency", "RMB");
            input.put("amount", 0.01);
            input.put("paydate", timestamp);
            input.put("remark", "2000311146");
            input.put("objectName", "KHpaygate");
            input.put("validtime", "0");

            String orig = "";       //原始数据
            String origData = "";
            String sign = "";        //产生签名
            String signData = "";
            String encoding = "GBK";
            string pemFile = "E:\\tool\\merchant.p12";
            string sing = "";

            try
            {//发送前，得到签名数据和签名后数据，单独使用

                origData = input.toString().replace("\n", "").replace("\t", "");

                // sing = MD5WithRSA.SignData(pemFile, "changeit", origData);
                sing = MD5WithRSA.sdbPaySign(origData);
                //       signDataput = (KeyedCollection) util.getSignData(input);
                //    System.out.println("--2222----" + signDataput.toString());
                orig = (String)signDataput.getDataValue("orig");
                origData = orig.replace("\t", "");
                //        System.out.println(origData);
                sign = (String)signDataput.getDataValue("sign");
                signData = sign;
                orig = PayclientInterfaceUtil.Base64Encode(orig, encoding);
                sign = PayclientInterfaceUtil.Base64Encode(sign, encoding);
                orig = java.net.URLEncoder.encode(orig, encoding);
                sign = java.net.URLEncoder.encode(sign, encoding);
            }
            catch (Exception e1)
            {
                e1.printStackTrace();
                orig = e1.getMessage();
            }
            return sing;


        }

        //生成包含原始订单信息的KeyedCollection
        private static KeyedCollection getInputOrig()
        {
            int count = 2;  //商品数量
            double price = 2.17;  //商品单价

            String timestamp = $"{DateTime.Now:yyyyMMddHHmmss}";
            String datetamp = timestamp.substring(0, 8);  //日期	


            KeyedCollection inputOrig = new KeyedCollection("inputOrig");

            inputOrig.put("masterId", "2000311146");  //商户号，注意生产环境上要替换成商户自己的生产商户号
                                                      //     inputOrig.put("orderId", "2000311146" + datetamp + getOrderId());  //订单号，严格遵守格式：商户号+8位日期YYYYMMDD+8位流水
            inputOrig.put("orderId", "20003111462017032398759353");  //订单号，严格遵守格式：商户号+8位日期YYYYMMDD+8位流水
            inputOrig.put("currency", "RMB");  //币种，目前只支持RMB
                                               //   inputOrig.put("amount", count * price);  //订单金额，12整数，2小数
            inputOrig.put("amount", String.Format("{0:F}", count * price));  //订单金额，12整数，2小数

            inputOrig.put("paydate", "20170323114518");  //下单时间，YYYYMMDDHHMMSS	
            inputOrig.put("objectName", "KHpaygate");  //订单款项描述（商户自定）
            inputOrig.put("validtime", "0");  //订单有效期(秒)，0不生效	
            inputOrig.put("remark", "2000311146");  //备注字段（商户自定）

            return inputOrig;
        }



        #endregion
      
        // GET: Home
        public ActionResult Index()
        {

            //  return Redirect("/home/sdbPayDemo");
            return View();
        }


        #region 平安银联接口


        //银行卡开通  localhost:4113/home/UnionAPI_OpenDemo
        public ActionResult UnionAPI_OpenDemo()
        {
            String orig = "";  //原始数据
            String sign = "";  //签名数据
            String encoding = "GBK";
            try
            {
                string customerId = "SH00001";   //商户会员号  ，自己业务中的,不超过30位
                orig = Util.CreateUnionAPI_OpenString(customerId);

                sign = MD5WithRSA.sdbPaySign(orig);
             orig = Base64.EncodeBase64(orig, encoding);  //原始数据先做Base64Encode转码
           sign = Base64.EncodeBase64(sign, encoding);  //签名数据先做Base64Encode转码
                  orig = HttpUtility.UrlEncode(orig, Encoding.GetEncoding("GBK"));  //Base64Encode转码后原始数据,再做URL转码
                 sign =  HttpUtility.UrlEncode(sign, Encoding.GetEncoding("GBK"));  //Base64Encode转码后签名数据,再做URL转码
            }
            catch (Exception e)
            {
                e.printStackTrace();
            }
            string sHtmlText = Util.UnionBuildRequest(sign, orig, "post", "确认");
            return Content(sHtmlText);

        }

        /// <summary>
        /// 单个银行卡开通查询  http://localhost:4113/home/UnionAPI_QueryOPNQuery
        /// </summary>
        /// <returns></returns>
        public ActionResult UnionAPI_QueryOPNQuery(string accNo= "6226330151030000")
        {
            string customerId = "SH00001";   //商户会员号  ，自己业务中的,不超过30位
            var cc = Util.UnionAPI_QueryOPNData(customerId, accNo);
            return Content("单个银行卡开通查询(自行处理):" + cc);
        }



        /// <summary>
        /// 已开通银行卡列表查询接口  localhost:4113/home/UnionAPI_OpenedQuery
        /// </summary>
        /// <returns></returns>
        public ActionResult UnionAPI_OpenedQuery()
        {
            string customerId = "SH00001";   //商户会员号  ，自己业务中的,不超过30位
            var cc = Util.UnionAPI_OpenedData(customerId);
            return Content("已开通银行卡列表(自行处理):" + cc);
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult UnionAPI_SSMS(string   money="0.1",string OpenId = "20003111462017051540429706")
        {
            string customerId = "SH00001";   //商户会员号  ，自己业务中的,不超过30位
            //OpenId为银行卡开通ID 
            decimal amount = Convert.ToDecimal(money);   //单笔支付不能小于0.1元

            var cc = Util.UnionAPI_SSMSData(customerId, OpenId, amount);
            return Content("发送验证码返回(订单号,下单时间要用在下一个方法):" + cc);
        }



        /// <summary>
        /// 发起后台支付交易   orderID timestamp  来源上次发送sms时得到的  http://localhost:4113/home/UnionAPI_Submit?orderID=20003111462017053147225799&timestamp=20170531153503
        /// </summary>
        /// <returns></returns>
        public ActionResult UnionAPI_Submit(string orderID,string timestamp, string verifyCode="111111", string money = "0.1", string OpenId = "20003111462017051540429706")
        {
           // string customerId = "SH00001";   //商户会员号  ，自己业务中的,不超过30位
            
            string customerId = "39B6B340";
            decimal amount = Convert.ToDecimal(money);
            //短信验证码 verifyCode
            var cc = Util.UnionAPI_SubmitData(customerId, OpenId, amount, orderID, timestamp, verifyCode);
            return Content("发起后台支付交易:" + cc);
        }



        /// <summary>
        /// 后台支付交易结果   orderID    来源上次发送sms时得到的  http://localhost:4113/home/UnionAPI_OrderQuery?orderID=20003111462017053147225799 
        /// </summary>
        /// <returns></returns>
        public ActionResult UnionAPI_OrderQuery(string orderID )
        {
            string customerId = "SH00001";   //商户会员号  ，自己业务中的,不超过30位
      
            var cc = Util.UUnionAPI_OrderQueryData(customerId,   orderID );
            return Content("后台支付交易结果:" + cc);
        }




        /// <summary>
        /// 单个银行卡关闭  http://localhost:4113/home/UnionAPI_OPNCL
        /// </summary>
        /// <returns></returns>
        public ActionResult UnionAPI_OPNCL(string OpenId = "20003111462017051540429706")
        {
            string customerId = "SH00001";   //商户会员号  ，自己业务中的,不超过30位
            var cc = Util.UnionAPI_OPNCLData(customerId, OpenId);
            return Content("单个银行卡关闭(自行处理):" + cc);
        }


        /// <summary>
        /// 银行卡开通后返回 
        /// </summary>
        /// <returns></returns>
        public ActionResult UnionBankOpenReturn()
        {
            var cc = Request.RawUrl;
            String orig = this.Request["orig"];
            String sign = this.Request["sign"];
            MicroWeb.General.Common.LogResult("UnionBankOpenRawUrl如下" + cc);
            MicroWeb.General.Common.LogResult("UnionBankOpen orig如下" + orig);
            MicroWeb.General.Common.LogResult("UnionBankOpen sign如下" + sign);
            String encoding = "GBK";
            bool result = false;
            //添加 远程验证服务 http://localhost:8080/axis2/services/BankService?wsdl
            //   BankService.BankService ba = new BankService.BankService();
            //   var webrsaVerfy = ba.JavaRsaVerify(orig, sign);
            //    MicroWeb.General.Common.LogResult("JavaRsaVerify如下" + webrsaVerfy);
            try
            {
                orig = System.Web.HttpUtility.UrlDecode(orig, Encoding.GetEncoding("GBK"));
                sign = System.Web.HttpUtility.UrlDecode(sign, Encoding.GetEncoding("GBK"));


                orig = Base64.DecodeBase64(orig, encoding);
                sign = Base64.DecodeBase64(sign, encoding);


                //     var webrsaVerfyDecode = ba.JavaRsaVerifyDecode(orig, sign);
                //      MicroWeb.General.Common.LogResult("webrsaVerfyDecode" + webrsaVerfyDecode);

                object[] obj = new object[2];
                obj[0] = orig;
                obj[1] = sign;
                var wsdl = Util.CallWebServiceObj("JavaRsaVerifyDecode", obj);   //用动态调用java的方式验签
                MicroWeb.General.Common.LogResult("JavaRsaVerifyDecode如下" + wsdl);
                if (wsdl.toString() == "wsdlFail")
                {
                    return Content("验签出错,原因请查看sdbLocalVerifyUrl配置的地址能否访问，可自行返回银行卡列表!");
                }

                //  result = SignCheck.verifyData(orig, sign);
                if (Convert.ToBoolean(wsdl.toString()))
                //    if (result)
                {
                    KeyedCollection output = Util.parseOrigData(orig);

                    MicroWeb.General.Common.LogResult("union 详细信息" + output);
                    MicroWeb.General.Common.LogResult("union 状态" + output.getDataValue("status"));   //对银联来说 01为成功，02为失败
                    //MicroWeb.General.Common.LogResult("订单号" + output.getDataValue("orderId"));
                    //MicroWeb.General.Common.LogResult("商品描述" + output.getDataValue("objectName"));
                    //MicroWeb.General.Common.LogResult("备注" + output.getDataValue("remark"));

                    return Content("绑卡完成，请等待系统验证 状态码:" + output.getDataValue("status"));
                }
            }
            catch (Exception e)
            {
                MicroWeb.General.Common.LogResult("UnionBankOpen返回测试报错" + e);
                return Content("UnionBankOpen返回测试报错" + e.InnerException);

            }

            return Content("返回" + orig + "^^^^^" + sign);
        }




        #endregion

        #region 网关支付



        ///home/sdbPayDemo
        public ActionResult sdbPayDemo()
        {

            String orig = "";  //原始数据
            String sign = "";  //签名数据
            String encoding = "GBK";
            //   string pemFile = "E:\\tool\\merchant.p12";

            try
            {
                //string  origData = getInputOrig().toString().replace("\n", "").replace("\t", "");
                // sign = MD5WithRSA.sdbPaySign(origData);   //签名
              orig = Util.CreatePayorigString();  //获取原始数据
                //  orig=    Util.CreateSelfPayorigString("dsd12121", Convert .ToDecimal(0.01));
                sign = MD5WithRSA.sdbPaySign(orig);

                orig = Base64.EncodeBase64(orig, encoding);  //原始数据先做Base64Encode转码

                sign = Base64.EncodeBase64(sign, encoding);  //签名数据先做Base64Encode转码


                orig = System.Web.HttpUtility.UrlEncode(orig, Encoding.GetEncoding("GBK"));  //Base64Encode转码后原始数据,再做URL转码

                sign = System.Web.HttpUtility.UrlEncode(sign, Encoding.GetEncoding("GBK"));  //Base64Encode转码后签名数据,再做URL转码

            }
            catch (Exception e)
            {
                e.printStackTrace();
            }
            string sHtmlText = Util.BuildRequest(sign, orig, "post", "确认");
            return Content(sHtmlText);
        }

        public ActionResult sdbReturn()
        {
            var cc = Request.RawUrl;
            String orig = this.Request["orig"];
            String sign = this.Request["sign"];
            MicroWeb.General.Common.LogResult("RawUrl如下" + cc);
            MicroWeb.General.Common.LogResult("orig如下" + orig);
            MicroWeb.General.Common.LogResult("sign如下" + sign);
            String encoding = "GBK";
            bool result = false;
            //添加 远程验证服务 http://localhost:8080/axis2/services/BankService?wsdl
         //   BankService.BankService ba = new BankService.BankService();
         //   var webrsaVerfy = ba.JavaRsaVerify(orig, sign);
        //    MicroWeb.General.Common.LogResult("JavaRsaVerify如下" + webrsaVerfy);
            try
            {
                orig = System.Web.HttpUtility.UrlDecode(orig, Encoding.GetEncoding("GBK"));
                sign = System.Web.HttpUtility.UrlDecode(sign, Encoding.GetEncoding("GBK"));


                orig = Base64.DecodeBase64(orig, encoding);
                sign = Base64.DecodeBase64(sign, encoding);


           //     var webrsaVerfyDecode = ba.JavaRsaVerifyDecode(orig, sign);
          //      MicroWeb.General.Common.LogResult("webrsaVerfyDecode" + webrsaVerfyDecode);

                object[] obj=new object[2];
                obj[0] = orig;
                obj[1] = sign;
              var wsdl=  Util.CallWebServiceObj("JavaRsaVerifyDecode", obj);   //用动态调用java的方式验签
                MicroWeb.General.Common.LogResult("JavaRsaVerifyDecode如下" + wsdl);
                if (wsdl.toString() == "wsdlFail")
                {
                    return Content("验签出错,原因1查看sdbLocalVerifyUrl配置的地址能否访问");
                }

                //  result = SignCheck.verifyData(orig, sign);
                if (Convert.ToBoolean(wsdl.toString()))
                //    if (result)
                {
                    KeyedCollection output = Util.parseOrigData(orig);

                    MicroWeb.General.Common.LogResult("订单详细信息" + output);
                    MicroWeb.General.Common.LogResult("订单状态" + output.getDataValue("status"));   //对银联来说 01为成功，02为失败
                    MicroWeb.General.Common.LogResult("订单号" + output.getDataValue("orderId"));
                    MicroWeb.General.Common.LogResult("商品描述" + output.getDataValue("objectName"));
                    MicroWeb.General.Common.LogResult("备注" + output.getDataValue("remark"));
                   
                    return Content("完成，请等待系统验证，备注字段"+ output.getDataValue("remark"));
                }
            }
            catch (Exception e)
            {
                MicroWeb.General.Common.LogResult("返回测试报错" + e);
                return Content("返回测试报错" + e.InnerException);

            }
             
  return Content("返回" + orig + "^^^^^" + sign );
        }
  

        public ActionResult sdbReturnNotify()
        {

            String encoding = "GBK";
            //模拟银行返回通知原始数据，实际页面接收程序应为：
        
            String orig = this.Request["orig"];
            String sign = this.Request["sign"];

            MicroWeb.General.Common.LogResult("异步orig如下" + orig);
            MicroWeb.General.Common.LogResult("异步sign如下" + sign);


        orig = "PGtDb2xsIGlkPSJvdXRwdXQiIGFwcGVuZD0iZmFsc2UiPjxmaWVsZCBpZD0iZXJyb3JDb2RlIiB2YWx1ZT0iVUtIUEFZMzQiIHJlcXVpcmVkPSJmYWxzZSIvPjxmaWVsZCBpZD0iZXJyb3JNc2ciIHZhbHVlPSLS+NDQsrvWp7PWIiByZXF1aXJlZD0iZmFsc2UiLz48ZmllbGQgaWQ9Im1hc3RlcklkIiB2YWx1ZT0iMjAwMDczOTc1NiIgcmVxdWlyZWQ9ImZhbHNlIi8+PGZpZWxkIGlkPSJwbGFudEJhbmtJZCIgdmFsdWU9Im51bGwwMSIgcmVxdWlyZWQ9ImZhbHNlIi8+PGZpZWxkIGlkPSJzdGF0dXMiIHZhbHVlPSIwMiIgcmVxdWlyZWQ9ImZhbHNlIi8+PGZpZWxkIGlkPSJkYXRlIiB2YWx1ZT0iMjAxNzEwMjcxNTI3MzIiIHJlcXVpcmVkPSJmYWxzZSIvPjxmaWVsZCBpZD0iYWNjTm8iIHZhbHVlPSI4ODE0IiByZXF1aXJlZD0iZmFsc2UiLz48ZmllbGQgaWQ9InRlbGVwaG9uZSIgdmFsdWU9IjEzNSoqKiozODUwIiByZXF1aXJlZD0iZmFsc2UiLz48ZmllbGQgaWQ9ImN1c3RvbWVySWQiIHZhbHVlPSIzQjk5MTdBQSIgcmVxdWlyZWQ9ImZhbHNlIi8+PGZpZWxkIGlkPSJPcGVuSWQiIHZhbHVlPSIiIHJlcXVpcmVkPSJmYWxzZSIvPjxmaWVsZCBpZD0ib3JkZXJJZCIgdmFsdWU9IjIwMDA3Mzk3NTYyMDE3MTAyNzEyMjU1MjI1IiByZXF1aXJlZD0iZmFsc2UiLz48ZmllbGQgaWQ9ImJhbmtUeXBlIiB2YWx1ZT0iIiByZXF1aXJlZD0iZmFsc2UiLz48ZmllbGQgaWQ9InBsYW50QmFua05hbWUiIHZhbHVlPSIiIHJlcXVpcmVkPSJmYWxzZSIvPjwva0NvbGw+";
            //模拟银行返回通知原始数据，实际页面接收程序应为：

             sign = "YzZmMDEzMzM5YTc0MjEyMmM1MTBmNzU1OWU5NjM2MmI3YzAzNjllZjVmNDJmZTFhMTZkN2M2YTk3NGE4YjgyMzE2ZjRjZmU4YjdlMDQ0NjQ5ZjJlMDQ2MzMzZmUwYzM4NzBmOTJkMjFhMmYzODkxZTE1ZjU4NzZjZGNkZGY1MWIyYTEyMDczMjU2ZDU3NjFlMmFkOTQ1YWRhNWIyYzY3YjMwMzM2YzBjYjc3MDFlNjMyMDhkYjQ1ZDIwMWI5MTM2ZDY0MjgxYzgxZmIwMDAxNTUzY2FkZTk4N2JmNTdmMDllMmZjMzcyNmJmZDNhNTg4NTQ4Y2I3NDMyMDkxMjlhZg==";
            // bool result = false;

            try
            {
             //   orig = System.Web.HttpUtility.UrlDecode(orig, Encoding.GetEncoding("GBK"));
           //     sign = System.Web.HttpUtility.UrlDecode(sign, Encoding.GetEncoding("GBK"));


                orig = Base64.DecodeBase64(orig, encoding);
                sign = Base64.DecodeBase64(sign, encoding);

                //此方法固定，改成上面动态的方法 
                //  result = SignCheck.verifyData(orig, sign);
                //BankService.BankService ba = new BankService.BankService();
                //var webrsaVerfyDecode = ba.JavaRsaVerifyDecode(orig, sign);
                //MicroWeb.General.Common.LogResult("webrsaVerfyDecode" + webrsaVerfyDecode);

                object[] obj = new object[2];
                obj[0] = orig;
                obj[1] = sign;
                var wsdl = Util.CallWebServiceObj("JavaRsaVerifyDecode", obj);   //用动态调用java的方式验签
                MicroWeb.General.Common.LogResult("sdbReturnNotify如下" + wsdl);
                if (wsdl.toString() == "wsdlFail")
                {
                    KeyedCollection output = Util.parseOrigData(orig);
                    return Content("验签出错，提示信息" + output.getDataValue("errorMsg"));
               //     return Content("验签出错,其它信息"+ orig.toString());
                }


                if (Convert.ToBoolean(wsdl.toString()))
                {
                    KeyedCollection output = Util.parseOrigData(orig);
                    return Content("完成，请等待系统验证，errorMsg信息(为空则正常):" + output.getDataValue("errorMsg"));
                    //做具体业务操作
                }
            }
            catch (Exception e)
            {
                return Content("返回测试报错" + e);

            }


            return Content("返回测试" );
        }


        #endregion


        #region 对账操作


        /// <summary>
        /// 单笔订单状态查询  http://localhost:4113/home/KH0001Result?OrderNO=20003111462017031115495117
        /// </summary>
        /// <returns></returns>
        public ActionResult KH0001Result(string OrderNO = "")
        {
         
         var cc=   Util.KH0001Data(OrderNO);

            return Content("返回KH0001对帐结果:"+ cc);
        }

        /// <summary>
        /// 订单列表信息查询(查询的时间范围间隔不能超过31天) localhost:4113/home/KH0002Result
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>

        public ActionResult KH0002Result(string beginDate = "20170601000000", string endDate= "20170607240000",string excel="false")
        {
            if (excel == "true")
            {
               var t= Util.KH0002DataList(beginDate, endDate);
                DataSet ds = ListToDataSet<KH0002ResultModel>(t);
                ReportExcel(ds, "KH"+ beginDate+"_"+ endDate);
            }
            var cc = Util.KH0002Data(beginDate, endDate);



            return Content("返回KH0002对帐结果:" + cc);
        }


        /// <summary>
        /// 每日对账单查询接口 对账日期，格式：YYYYMMDD   T-1日  http://localhost:4113/home/KH0003Result?Date=20170601
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public ActionResult KH0003Result(string  Date = "20170601")
        {

            var cc = Util.KH0003Data( Date);

            return Content("返回KH0003对帐结果:" + cc);
        }




        #endregion

        #region 数据转换

        public static DataSet ListToDataSet<T>(List<T> list)
        {
            if (list.Count == 0) return new DataSet();
            var properties = list[0].GetType().GetProperties();
            var cols = properties.Select(p => new DataColumn(p.Name));
            var dt = new DataTable();
            dt.Columns.AddRange(cols.ToArray());
            list.ForEach(x => dt.Rows.Add(properties.Select(p => p.GetValue(x)).ToArray()));
            return new DataSet { Tables = { dt } };
        }


        //dataSet导出Excel
        public void ReportExcel(DataSet ds, string title)
        {
            DataTable dt = ds.Tables[0];
            Workbook wb = new Workbook();
            Worksheet ws = wb.Worksheets[0];
            Cells cells = ws.Cells;
            Style sle = wb.Styles[wb.Styles.Add()];
            cells.SetRowHeight(0, 20);

            if (dt != null)
            {
                for (int i = 0; i <= dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (i == 0)
                            ws.Cells[i, j].PutValue(dt.Columns[j].Caption);
                        else
                            ws.Cells[i, j].PutValue(dt.Rows[(i - 1)][dt.Columns[j].Caption].ToString());

                    }
                }
                ws.AutoFitColumns();
                DataTableToExcel. WorkbookToExcel(wb, title + DateTime.Now.ToString("yy_MM_dd_HHmmss") + ".xls");

            }
        }


        

        #endregion

    }
}