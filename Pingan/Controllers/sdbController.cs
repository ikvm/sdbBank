using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test;

namespace Pingan.Controllers
{
    public class sdbController : Controller
    {
        // GET: sdb
        public ActionResult Index()
        {
            string accountNo = "11002873390701";
            var oResult =  PinganQuery.Account4001Query(accountNo);
            return View(oResult);
        }

        public ActionResult sResultNormal()
        {
            var sResultNormal = PinganQuery.S001NormalQuery();
            return Content(sResultNormal);

        }

        public ActionResult sResult()
        {
            var sResult = PinganQuery.S001Query();
            return Content(sResult.ToString());

        }

    }
}