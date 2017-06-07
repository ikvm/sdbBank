using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sdbBank;


namespace Pingan.Controllers
{
    public class sdbController : Controller
    {
        /// <summary>
        /// 查询余额
        /// </summary>
        /// <param name="accountNo"></param>
        /// <returns></returns>
        public ActionResult Index( string accountNo = "11014803543008")
        {
        
            var oResult =  PinganQuery.YQ4001NormalQuery(accountNo);
            //解释返回码与返回描述
            var a = PAHelper.getReturnXmlInfo(oResult);
            var b = PAHelper.getAccBalance(oResult);
            return Content(a +b  );
           
        }


        /// <summary>
        /// 系统探测
        /// </summary>
        /// <returns></returns>
        public ActionResult sys()
        {
            var sResultNormal = PinganQuery.S001NormalQuery();
            return Content(sResultNormal);

        }

        /// <summary>
        /// 跨行快付
        /// </summary>
        /// <returns></returns>
        public ActionResult KHKF03(string InAcctNo= "6221558812340000", string InAcctName= "互联网", string TranAmount="5.59")
        {
         //   var OrderNumber =  "ZXLKF0320170417TV003";   //20位客户的订单号
            var OrderNumber =Util.get20OrderID();   //20位客户的订单号
            var oResult = PinganQuery.KHKF03(  InAcctNo,   InAcctName,   TranAmount, OrderNumber);

            var cc= PAHelper.getKHKF03Result(oResult);

            return Content(cc);
        }



        /// <summary>
        /// 跨行快付结果查询查询   http://localhost:4113/sdb/KHKF04
        /// </summary>
        /// <returns></returns>
        public ActionResult KHKF04(string OrderNumber = "ZXLKF0320170417TV001")
        {
            //传订单号
            var oResult = PinganQuery.KHKF04(OrderNumber);

            var cc = PAHelper.getKHKF04Result(oResult);

            return Content(cc);
        }



        /// <summary>
        /// 当日明细
        /// </summary>
        /// <param name="accountNo"></param>
        /// <returns></returns>

        public ActionResult todayDetail4008(string accountNo = "11014803543008")
        {

            var oResult = PinganQuery.YQ4008todayDetailQuery(accountNo);
             
            var cc = PAHelper.get4008Result(oResult);
            return Content(cc);

        }


        /// <summary>
        /// 历史明细
        /// </summary> 
        /// <returns></returns>

        public ActionResult HisDetail4013(string accountNo = "11014803543008",string BeginDate="20170424",string EndDate="20170424")
        {

            var oResult = PinganQuery.YQ4013HisDetailQuery(accountNo, BeginDate, EndDate);

            var cc = PAHelper.get4013Result(oResult);
            return Content(cc);

        }



        //使用post查询_无效
        public ActionResult sResult()
        {
            var sResult = PinganQuery.S001Query();
            return Content(sResult.ToString());

        }

    }
}