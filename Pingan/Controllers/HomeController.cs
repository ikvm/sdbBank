using System;
using System.Text;
using System.Web.Mvc;
using com.ecc.emp.data;
using com.sdb.payclient.core;
using ikvm.extensions;
using sdbBank;
using TimeZone = System.TimeZone;
 

namespace Pingan.Controllers
{
    public class HomeController : Controller
    {
        #region old
         

        public string  testJava()
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
            string sing="";

            try
            {//发送前，得到签名数据和签名后数据，单独使用

                origData = input.toString().replace("\n", "").replace("\t", "");

                 // sing = MD5WithRSA.SignData(pemFile, "changeit", origData);
                sing = MD5WithRSA.sdbPaySign( origData);
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
            inputOrig.put("amount", String.Format("{0:F}",  count * price));  //订单金额，12整数，2小数
                                                  
            inputOrig.put("paydate", "20170323114518");  //下单时间，YYYYMMDDHHMMSS	
            inputOrig.put("objectName", "KHpaygate");  //订单款项描述（商户自定）
            inputOrig.put("validtime", "0");  //订单有效期(秒)，0不生效	
            inputOrig.put("remark", "2000311146");  //备注字段（商户自定）
 
            return inputOrig;
        }

 


        // GET: Home
        public ActionResult Index()
        {

            return Redirect("/home/sdbPayDemo");
         //  return View();
        }


  

    }
}