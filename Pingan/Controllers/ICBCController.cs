using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test;
namespace Pingan.Controllers
{
    public class ICBCController : Controller
    {


//        企业按照工行提供的xml包格式进行打包，在局域网内通过http协议以POST方式将交易包发送到NetSafe Client的安全http协议服务器。
//http请求格式：action=”http://客户端NetSafe Client的地址和加密端口号/servlet/ICBCCMPAPIReqServlet?userID=证书ID&PackageID=包序列ID &SendTime=请求时间”
//请求数据格式（post方式）：Version=版本号（0.0.0.1，不同版本号对应的接口格式不同，请参考具体交易的接口文档) &TransCode=交易代码（区分交易类型，每个交易固定)
//&BankCode=客户的归属单位&GroupCIS=客户的归属编码&ID=客户的证书ID（无证书客户可空)&PackageID=客户的指令包序列号（由客户ERP系统产生，不可重复)
//&Cert=客户的证书公钥信息（进行BASE64编码；NC客户送空) &reqData=客户的xml请求数据




        // GET: ICBC
        public ActionResult Index()
        {
            //调用方法实例
            //源 http://blog.csdn.net/xie3400656/article/details/7011106
            var oResult = Test.ICBCQuery.B2BQuery("10087", "20111124", "2307EC.........", "2307.........");
            string strResultMsg = string.Empty;
            //正常返回查询结果
            if (oResult.GetType() == typeof(Test.ICBCQuery))
            {
                var bInfo = ((Test.ICBCQuery)oResult);
                strResultMsg = Enum.GetName(typeof(Test.ICBCQuery.CommandState), Convert.ToInt32(bInfo.tranStat)).ToString();
            }
            else
                strResultMsg = Enum.GetName(typeof(Test.ICBCQuery.ErrorCode), Convert.ToInt32(oResult));


            return View();
        }
    }
}