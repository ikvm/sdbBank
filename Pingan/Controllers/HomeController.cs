﻿using System;
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

namespace Pingan.Controllers
{
    public class HomeController : Controller
    {


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

        #region old,放弃


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
                //     orig=    Util.CreatePayorigStringTest();
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
            BankService.BankService ba = new BankService.BankService();
            var webrsaVerfy = ba.JavaRsaVerify(orig, sign);
            MicroWeb.General.Common.LogResult("JavaRsaVerify如下" + webrsaVerfy);
            try
            {
                orig = System.Web.HttpUtility.UrlDecode(orig, Encoding.GetEncoding("GBK"));
                sign = System.Web.HttpUtility.UrlDecode(sign, Encoding.GetEncoding("GBK"));


                orig = Base64.DecodeBase64(orig, encoding);
                sign = Base64.DecodeBase64(sign, encoding);


                var webrsaVerfyDecode = ba.JavaRsaVerifyDecode(orig, sign);
                MicroWeb.General.Common.LogResult("webrsaVerfyDecode" + webrsaVerfyDecode);
                //  result = SignCheck.verifyData(orig, sign);
                if (webrsaVerfyDecode)
                //    if (result)
                {
                    KeyedCollection output = Util.parseOrigData(orig);

                    MicroWeb.General.Common.LogResult("订单详细信息" + output);
                    MicroWeb.General.Common.LogResult("订单状态" + output.getDataValue("status"));
                    MicroWeb.General.Common.LogResult("订单号" + output.getDataValue("orderId"));
                    MicroWeb.General.Common.LogResult("商品描述" + output.getDataValue("objectName"));
                    MicroWeb.General.Common.LogResult("备注" + output.getDataValue("remark"));
                    //System.out.println("---支付完成时间---" + output.getDataValue("date"));
                    //System.out.println("---手续费金额---" + output.getDataValue("charge"));
                    //System.out.println("---商户号---" + output.getDataValue("masterId"));

                    //System.out.println("---币种---" + output.getDataValue("currency"));
                    //System.out.println("---订单金额---" + output.getDataValue("amount"));
                    //System.out.println("---下单时间---" + output.getDataValue("paydate"));

                    //System.out.println("---订单有效期---" + output.getDataValue("validtime"));

                }
            }
            catch (Exception e)
            {
                MicroWeb.General.Common.LogResult("返回测试报错" + e);
                return Content("返回测试报错" + e.InnerException);

            }



            return Content("返回" + orig + "^^^^^" + sign + "^^^^" + result);
        }
        public ActionResult sdbReturnNotify()
        {
            var cc = Request.RawUrl;
            String orig = this.Request["orig"];
            String sign = this.Request["sign"];
            MicroWeb.General.Common.LogResult("orig Notify如下" + orig);
            MicroWeb.General.Common.LogResult("sign Notify如下" + sign);
            return Content("返回");
        }



        // GET: Home
        public ActionResult Index()
        {

            //  return Redirect("/home/sdbPayDemo");
            return View();
        }

        public ActionResult sdbPayReturn()
        {

            String encoding = "GBK";
            //模拟银行返回通知原始数据，实际页面接收程序应为：

            String orig = this.Request["orig"];
            String sign = this.Request["sign"];

           //   orig = "PGtDb2xsIGlkPSJvdXRwdXQiIGFwcGVuZD0iZmFsc2UiPjxmaWVsZCBpZD0ic3RhdHVzIiB2YWx1%0AZT0iMDEiLz48ZmllbGQgaWQ9ImRhdGUiIHZhbHVlPSIyMDE0MDUwOTA4NTUwMiIvPjxmaWVsZCBp%0AZD0iY2hhcmdlIiB2YWx1ZT0iMTAiLz48ZmllbGQgaWQ9Im1hc3RlcklkIiB2YWx1ZT0iMjAwMDMx%0AMTE0NiIvPjxmaWVsZCBpZD0ib3JkZXJJZCIgdmFsdWU9IjIwMDAzMTExNDYyMDE0MDUwOTI1OTk1%0AMTE0Ii8%2BPGZpZWxkIGlkPSJjdXJyZW5jeSIgdmFsdWU9IlJNQiIvPjxmaWVsZCBpZD0iYW1vdW50%0AIiB2YWx1ZT0iLjAxIi8%2BPGZpZWxkIGlkPSJwYXlkYXRlIiB2YWx1ZT0iMjAxNDA1MDkwODU1MzAi%0ALz48ZmllbGQgaWQ9InJlbWFyayIgdmFsdWU9IiIvPjxmaWVsZCBpZD0ib2JqZWN0TmFtZSIgdmFs%0AdWU9IktIcGF5Z2F0ZSIvPjxmaWVsZCBpZD0idmFsaWR0aW1lIiB2YWx1ZT0iMCIvPjwva0NvbGw%2B%0A";
            //模拟银行返回通知原始数据，实际页面接收程序应为：
      
           //   sign = "MjY5YzJlMDBhMzcyZTJkNWJjYjAxMzhmNGMxNmRkNDVjNjVjYTY3YzhiMjc1NTZhNTk0MTI0MzE5%0AN2Q1MWZkNWI5OTMxNzJhZTJiZDEyNDNmMjE3ZTk4MjU1N2E2YzAzOGI1YjI2YTQ0ZWU0M2EyNjUx%0AZTdmNjk2NDMzMDZhNTM5Y2NjMDM0YzJjZjJjZGE2ZjZlOTE1NTU3MzE1NzYxOGE4NGI1YTAwNTZi%0AODg4ZjVlMDdlMmNjODlmNzUyNzVmMGFmZDAzMWY4MDg3MjRjNjc0ZGE0MmRjNjYzNTM1YjM2MDFi%0ANDA4ZjllYWI4YjgxNDI4Y2E4NWM1NjMxMzA2ZA%3D%3D%0A";
            bool result = false;

            try
            {
                orig = System.Web.HttpUtility.UrlDecode(orig, Encoding.GetEncoding("GBK"));
                sign = System.Web.HttpUtility.UrlDecode(sign, Encoding.GetEncoding("GBK"));


                orig = Base64.DecodeBase64(orig, encoding);
                sign = Base64.DecodeBase64(sign, encoding);

                 //此方法改成以上的方法
                result = SignCheck.verifyData(orig, sign);  

                if (result)
                {
                    //output = Util.parseOrigData(orig);
                 
                }
            }
            catch (Exception e)
            {
                return Content("返回测试报错" + e);

            }


            return Content("返回测试结果:" + result);
        }

    }
}