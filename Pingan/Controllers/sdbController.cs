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
        public ActionResult Index( string accountNo = "11002923034501" )
        {
        
            var oResult =  PinganQuery.YQ4001NormalQuery(accountNo);
            return Content(oResult );
            //return View(oResult);
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
            var oResult = PinganQuery.KHKF03(  InAcctNo,   InAcctName,   TranAmount);
            return Content(oResult);
        }



        /// <summary>
        /// 跨行快付查询  3.4 单笔付款结果查询  http://localhost:4113/sdb/KHKF04
        /// </summary>
        /// <returns></returns>
        public ActionResult KHKF04(string OrderNumber = "ZXLKF0320170411TV001")
        {
            //传订单号
            var oResult = PinganQuery.KHKF04(OrderNumber);
            return Content(oResult);
        }




        //使用post查询_无效
        public ActionResult sResult()
        {
            var sResult = PinganQuery.S001Query();
            return Content(sResult.ToString());

        }

    }
}