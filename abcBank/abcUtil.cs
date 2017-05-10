using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.abc.trustpay.client;

namespace abcBank
{
    public class abcUtil
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


        /// <summary>
        /// 农行方式1支付
        /// </summary>
        /// <returns></returns>
        public static string abcMerChantPayment()
        {

            //1、生成定单订单对象，并将订单明细加入订单中
            com.abc.trustpay.client.ebus.PaymentRequest tPaymentRequest = new com.abc.trustpay.client.ebus.PaymentRequest();
            //2、设定订单属性
            tPaymentRequest.dicOrder["PayTypeID"] = "ImmediatePay";    //设定交易类型
            tPaymentRequest.dicOrder["OrderNo"] = "ON200412230001'";                       //设定订单编号
            tPaymentRequest.dicOrder["ExpiredDate"] = "20240619104901";//设定订单保存时间 非必须
            tPaymentRequest.dicOrder["OrderAmount"] = "0.01";    //设定交易金额
        //    tPaymentRequest.dicOrder["Fee"] = ""; //设定手续费金额 非必须 
          //  tPaymentRequest.dicOrder["AccountNo"] = ""; //设定支付账户
            tPaymentRequest.dicOrder["CurrencyCode"] = "156";    //设定交易币种
            //tPaymentRequest.dicOrder["ReceiverAddress"] = "";     //收货地址
            tPaymentRequest.dicOrder["InstallmentMark"] = "0";  //分期标识
            //若分期有2其它配置
       //     tPaymentRequest.dicOrder["InstallmentCode"] = "";    //设定分期代码
      //      tPaymentRequest.dicOrder["InstallmentNum"] ="";    //设定分期期数


            //    tPaymentRequest.dicOrder["BuyIP"] ="";                           //IP
            tPaymentRequest.dicOrder["OrderDesc"] = "订单说明";                   //设定订单说明
          //  tPaymentRequest.dicOrder["OrderURL"] = "";                   //设定订单地址
            tPaymentRequest.dicOrder["OrderDate"] = DateTime.Now.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo); ;                   //设定订单日期 （必要信息 - YYYY/MM/DD）
            tPaymentRequest.dicOrder["OrderTime"] = DateTime.Now.ToString("HH:MM:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo); ;                   //设定订单时间 （必要信息 - HH:MM:SS）
      //      tPaymentRequest.dicOrder["orderTimeoutDate"] = "";                     //设定订单有效期
            tPaymentRequest.dicOrder["CommodityType"] = "0499";   //设置商品种类


            //3、添加订单明细
            System.Collections.Generic.Dictionary<string, string> orderitem = new System.Collections.Generic.Dictionary<string, string>();
            orderitem["SubMerName"] = "测试二级商户1";    //设定二级商户名称
            orderitem["SubMerId"] = "12345";    //设定二级商户代码
            orderitem["SubMerMCC"] = "0000";   //设定二级商户MCC码 
            orderitem["SubMerchantRemarks"] = "测试";   //二级商户备注项
            orderitem["ProductID"] = "IP000001";//商品代码，预留字段
            orderitem["ProductName"] = "慧生活缴费";//商品名称            必须设定
            orderitem["UnitPrice"] = "0.01";//商品总价
            orderitem["Qty"] = "1";//商品数量
            orderitem["ProductRemarks"] = "测试商品"; //商品备注项
            orderitem["ProductType"] = "充值类";//商品类型
            orderitem["ProductDiscount"] = "1";//商品折扣
            orderitem["ProductExpiredDate"] = "10";//商品有效期
            tPaymentRequest.dic.Add(1, orderitem);

            //4、设定支付请求对象
            tPaymentRequest.dicRequest["PaymentType"] = "1";          //设定支付类型         6：银联跨行支付
            tPaymentRequest.dicRequest["PaymentLinkType"] = "1";      //设定支付接入方式

            tPaymentRequest.dicRequest["UnionPayLinkType"] = "0";          //当支付类型为6，支付接入方式为2的条件满足时，需要设置银联跨行移动支付接入方式

            tPaymentRequest.dicRequest["ReceiveAccount"] ="";    //设定收款方账号
            tPaymentRequest.dicRequest["ReceiveAccName"] = "";    //设定收款方户名
            tPaymentRequest.dicRequest["NotifyType"] ="1";    //设定通知方式
            tPaymentRequest.dicRequest["ResultNotifyURL"] =   GetConfiguration("ABCResultNotifyURL");    //设定通知URL地址
            tPaymentRequest.dicRequest["MerchantRemarks"] = "";    //设定附言
            tPaymentRequest.dicRequest["IsBreakAccount"] = "0";    //设定交易是否分账
            tPaymentRequest.dicRequest["SplitAccTemplate"] = "";      //分账模版编号
             
            //5、传送支付请求并取得支付网址
            // tPaymentRequest.postJSONRequest();

            //多商户
            tPaymentRequest.extendPostJSONRequest(1);
            StringBuilder strMessage = new StringBuilder();
            string ReturnCode = JSON.GetKeyValue("ReturnCode");
            string ErrorMessage = JSON.GetKeyValue("ErrorMessage");
            if (ReturnCode.Equals("0000"))
            {
                strMessage.Append("ReturnCode   = [" + ReturnCode + "]<br/>");
                strMessage.Append("ErrorMessage = [" + ErrorMessage + "]<br/>");
                //6、支付请求提交成功，将客户端导向支付页面
                return JSON.GetKeyValue("PaymentURL");
            //    Response.Redirect(JSON.GetKeyValue("PaymentURL"));
            }
            else
            {
                //7、支付请求提交失败，商户自定后续动作
                strMessage.Append("ReturnCode   = [" + ReturnCode + "]<br/>");
                strMessage.Append("ErrorMessage = [" + ErrorMessage + "]<br/>");
                return ReturnCode;
            }

       
        }
    }
}
