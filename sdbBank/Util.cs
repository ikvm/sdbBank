using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.ecc.emp.data;
using ikvm.extensions;

namespace sdbBank
{
   public  class Util
    {


        /// <summary>
        /// 创建提交的数据
        /// </summary>
        /// <returns></returns>
        public static string CreatePayorigString()
        {
            int count = getRandomCount( );  //商品数量
            double price = 2.17;  //商品单价

            String timestamp = $"{DateTime.Now:yyyyMMddHHmmss}";
            String datetamp = timestamp.substring(0, 8);  //日期	


            KeyedCollection inputOrig = new KeyedCollection("inputOrig");

            inputOrig.put("masterId", SDKConfig.MasterID);  //商户号，注意生产环境上要替换成商户自己的生产商户号
          inputOrig.put("orderId", SDKConfig.MasterID + datetamp + getOrderId());  //订单号，严格遵守格式：商户号+8位日期YYYYMMDD+8位流水
    
            inputOrig.put("currency", "RMB");  //币种，目前只支持RMB
                                               //   inputOrig.put("amount", count * price);  //订单金额，12整数，2小数
            inputOrig.put("amount", String.Format("{0:F}", count * price));  //订单金额，12整数，2小数

            inputOrig.put("paydate", timestamp);  //下单时间，YYYYMMDDHHMMSS	
            inputOrig.put("objectName", "KHpaygate");  //订单款项描述（商户自定）
            inputOrig.put("validtime", "0");  //订单有效期(秒)，0不生效	
            inputOrig.put("remark", "付款");  //备注字段（商户自定）

            return inputOrig.toString().replace("\n", "").replace("\t", "");
        }



        /// <summary>
        /// 建立请求，以表单HTML形式构造（默认）----网页版用
        /// </summary>
        /// <param name="sign">请求参数</param>
        /// <param name="orig">请求参数</param>
        /// <param name="strMethod">提交方式。两个值可选：post、get</param>
        /// <param name="strButtonValue">确认按钮显示文字</param>
        /// <returns>提交表单HTML文本</returns>
        public static string BuildRequest(string sign,string orig ,string strMethod, string strButtonValue="提交")
        {

            StringBuilder sbHtml = new StringBuilder(); 
            string GATEWAY_NEW = SDKConfig.GATEWAYUrl;
            sbHtml.Append("<form id='sdbsubmit' name='NetPayForm' action='" + GATEWAY_NEW + "' method='" + strMethod.ToLower().Trim() + "'>");
 
         
                sbHtml.Append("<input type='hidden' name='sign' value='" + sign + "'/>");
            sbHtml.Append("<input type='hidden' name='orig' value='" + orig + "'/>");
            sbHtml.Append("<input type='hidden' name='returnurl' value='" + SDKConfig.FrontUrl + "'/>");
            sbHtml.Append("<input type='hidden' name='NOTIFYURL' value='" + SDKConfig.BackUrl + "'/>");


            //submit按钮控件请不要含有name属性
            sbHtml.Append("<input type='submit' value='" + strButtonValue + "' style='display:none;'></form>");

            sbHtml.Append("<script>document.forms['sdbsubmit'].submit();</script>");

            return sbHtml.ToString();
        }



        /// <summary>  
        /// DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time"> DateTime时间格式</param>  
        /// <returns>Unix时间戳格式</returns>  
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        public static string GenUniqueString()
        {
            string KeleyiStr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] rtn = new char[8];
            Guid gid = Guid.NewGuid();
            var ba = gid.ToByteArray();
            for (var i = 0; i < 8; i++)
            {
                rtn[i] = KeleyiStr[((ba[i] + ba[8 + i]) % 35)];
            }
            return "" + rtn[0] + rtn[1] + rtn[2] + rtn[3] + rtn[4] + rtn[5] + rtn[6] + rtn[7];
        }

        private static int getRandomCount()
        {
            Random Rdm = new Random();

            //产生1到100的随机数
            int iRdm = Rdm.Next(1, 100);
            return iRdm;
        }


        //生成8位随机数
        private static String getOrderId()
        {
            String orderId;
            java.util.Random r = new java.util.Random();
            while (true)
            {
                int i = r.nextInt(99999999);
                if (i < 0) i = -i;
                //  orderId = String.valueOf(i);
                orderId = i.ToString();
                //    System.out.println("---生成随机数---" + orderId);
                if (orderId.length() < 8)
                {
                    //      System.out.println("---位数不够8位---" + orderId);
                    continue;
                }
                if (orderId.length() >= 8)
                {
                    orderId = orderId.substring(0, 8);
                    //       System.out.println("---生成8位流水---" + orderId);
                    break;
                }
            }
            return orderId;
        }

    }
}
