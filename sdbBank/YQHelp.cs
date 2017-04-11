using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankPackage;

namespace sdbBank
{
    public class YQHelp
    {

        /// <summary>
        ///  * 组装报文
        /// </summary>
        /// <param name="yqdm">20位银企代码</param>
        /// <param name="bsnCode">交易代码</param>
        /// <param name="xmlBody">xml主体报文</param>
        /// <returns></returns>
        public static string asemblyYQPackets(String yqdm, String bsnCode, String xmlBody)
        {
            try
            {
                var cc = YQ.asemblyPackets(yqdm, bsnCode, xmlBody);
                return cc;
            }
            catch (Exception)
            {
                return ".net组装报文头失败";
            }
    
        }
    }
}
