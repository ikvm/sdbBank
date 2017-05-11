using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using abcBank;
using com.abc.trustpay.client;

namespace Agricultural_Bank.Controllers
{
    public class ABCController : Controller
    {
        // GET: ABC
        public ActionResult Index()
        {
            return View();
        }

        //localhost:4443/abc/abcPayDemo
        /// <summary>
        /// 模拟方式1支付
        /// </summary>
        /// <returns></returns>
        public ActionResult abcPayDemo()
        {
            var  cc= abcUtil.abcMerChantPayment();
            if (cc .Contains("/"))
            {
                return  Redirect(cc);
            }
             
            return Content("农行返回代码"+cc);
            
        }



        //服务器通知 页面
        public ActionResult ReciveServerPage()
        {
              string tMerchantPage = "";
        //1、取得MSG参数，并利用此参数值生成支付结果对象
        com.abc.trustpay.client.ebus.PaymentResult tResult = new com.abc.trustpay.client.ebus.PaymentResult();
            tResult.init(Request["MSG"]);

            //2、判断支付结果状态，进行后续操作
            if (tResult.isSuccess())
            {
                //3、支付成功
              
                tMerchantPage = "http://localhost:4443/abc/CustomerPage?OrderNo=" + tResult.getValue("OrderNo");
            }
            else
            {
                //4、支付失败
             
                tMerchantPage = "http://localhost:4443/abc/MerchantFailure?OrderNo=" + tResult.getValue("OrderNo");
            }


            return View();
        }


        /// <summary>
        /// 通知服务器通知支付失败结果接收
        /// </summary>
        /// <returns></returns>
        public ActionResult MerchantFailure()
        {

            //1、取得参数，并利用此参数值作后续处理
            string tOrderNo = Request["OrderNo"];

            //
            //后续处理
            //...
            //...
            //
            ViewBag["MerchantFailureTxt"] = "接收通知失败，交易编号为：" + tOrderNo;  


            return View();

        }

        public ActionResult CustomerPage()
        {
            //1、取得参数，并利用此参数值作后续处理
            string tOrderNo = Request["OrderNo"];

            //
            //后续处理
            //...
            //...
            //
            ViewBag["CustomerPageTxt"] = "已接收到通知，交易编号为：" + tOrderNo;

            return View();
        }


        /// <summary>
        /// 商户交易查询  http://localhost:4443/abc/MerchantQueryOrder?orderno=ON200412230001
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>

        public ActionResult MerchantQueryOrder(string OrderNo)
        {

            var cc = abcUtil.abcQueryOrder(OrderNo);


            ViewData["strMessage"] = cc;


            return View();
        }


    }
}