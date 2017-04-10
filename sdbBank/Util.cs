﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.ecc.emp.data;
using ikvm.extensions;
using java.util;
using Test;
using Random = System.Random;
using TimeZone = System.TimeZone;

namespace sdbBank
{
   public  class Util
    {

        /// <summary>
        /// 创建提交的数据test
        /// </summary>
        /// <returns></returns>
        public static string CreatePayorigStringTest()
        {
         
            KeyedCollection inputOrig = new KeyedCollection("output");
            inputOrig.put("status", "01");  
            inputOrig.put("date", "20140509085502");
            inputOrig.put("charge", "10");
            inputOrig.put("masterId", "2000311146");  //商户号，注意生产环境上要替换成商户自己的生产商户号
            inputOrig.put("orderId", "20003111462014050925995114");  //订单号，严格遵守格式：商户号+8位日期YYYYMMDD+8位流水

            inputOrig.put("currency", "RMB");  //币种，目前只支持RMB
                                               //   inputOrig.put("amount", count * price);  //订单金额，12整数，2小数
            inputOrig.put("amount", ".01");  //订单金额，12整数，2小数

            inputOrig.put("paydate", "20140509085530");  //下单时间，YYYYMMDDHHMMSS	
            inputOrig.put("remark", "");  //备注字段（商户自定）
            inputOrig.put("objectName", "KHpaygate");  //订单款项描述（商户自定）
            inputOrig.put("validtime", "0");  //订单有效期(秒)，0不生效	
     

            return inputOrig.toString().replace("\n", "").replace("\t", "");
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



        #region 处理请求数据

        /// <summary>
        /// 创建自身业务提交的数据
        /// </summary>
        /// <returns></returns>
        public static string CreateSelfPayorigString(string MasterOrderID, decimal amount)
        {
            
            String timestamp = $"{DateTime.Now:yyyyMMddHHmmss}";
            String datetamp = timestamp.substring(0, 8);  //日期	


            KeyedCollection inputOrig = new KeyedCollection("inputOrig");

            inputOrig.put("masterId", SDKConfig.MasterID);  //商户号，注意生产环境上要替换成商户自己的生产商户号
            inputOrig.put("orderId", SDKConfig.MasterID + datetamp + getOrderId());  //订单号，严格遵守格式：商户号+8位日期YYYYMMDD+8位流水

            inputOrig.put("currency", "RMB");  //币种，目前只支持RMB
            inputOrig.put("amount", String.Format("{0:F}", amount));  //订单金额，12整数，2小数

            inputOrig.put("paydate", timestamp);  //下单时间，YYYYMMDDHHMMSS	
            inputOrig.put("objectName", "KHpaygate");  //订单款项描述（商户自定）
            inputOrig.put("validtime", "0");  //订单有效期(秒)，0不生效	
            inputOrig.put("remark", MasterOrderID);  //备注字段（原业务系统订单号）

            return inputOrig.toString().replace("\n", "").replace("\t", "");
        }




        /// <summary>
        /// 创建提交的数据test
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

        #endregion

     

        #region 回调后的数据处理

 public static KeyedCollection parseOrigData(String origData)
     {
       KeyedCollection output = new KeyedCollection();
      try {
        output = (KeyedCollection)DataElementSerializer.serializeFrom(origData);
        }
       catch (Exception e1) {
         throw new Exception("源数据解析异常！"+ e1);
       }
        return output;
       }


        #endregion

        #region 对帐相关

        public static Properties parseStringToProperties(String data, String token)
        {
            String PROPERTY_DELIMER = "=";
            Boolean singleFlag = false;
            if (data == null)
            {
                return null;
            }
            if ((token == null) || (token.length() == 0))
            {
                singleFlag = true;
            }

            StringTokenizer tokenizer = new StringTokenizer(data, token);
            Properties props = new Properties();

            if (tokenizer.countTokens() == 0)
            {
                throw new NoSuchElementException("");
            }
            do
            {
                String element = tokenizer.nextToken();

                if (element.indexOf(PROPERTY_DELIMER) != -1)
                    props.put(element.substring(0, element.indexOf(PROPERTY_DELIMER)),
                            element.substring(element.indexOf(PROPERTY_DELIMER) + 1));
            } while (tokenizer.hasMoreTokens());

            return props;
        }




        //最终远程操作
        public static string getDataFromPayGate2Dotnet(String businessCode, String toOrig, String toUrl,
             KeyedCollection recv)
        {
            String payFlag = null;
            String fromOrig = null;
   String fromSign = null;
            String encoding = "GBK";
            String toSign = MD5WithRSA.sdbPaySign(toOrig);
            String toSignData = Base64.EncodeBase64(toSign, encoding);
            String toOrigData = Base64.EncodeBase64(toOrig, encoding);
            toSignData = System.Web.HttpUtility.UrlEncode(toSignData, Encoding.GetEncoding("GBK"));  //Base64Encode转码后原始数据,再做URL转码
           toOrigData = System.Web.HttpUtility.UrlEncode(toOrigData, Encoding.GetEncoding("GBK"));  //Base64Encode转码后签名数据,再做URL转码
            string aOutputData = null;
            aOutputData = "orig=" + toOrigData + "&sign=" + toSignData + "&businessCode=" + businessCode.getBytes(encoding);
          var response =  PAHelper.NcPost(toUrl, aOutputData);
            Properties res = parseStringToProperties(response, "\r\n");
            fromSign = ((String)res.get("sign")).trim();
            fromOrig = ((String)res.get("orig")).trim();
            payFlag = "SDBPAYGATE=" + ((String)res.get("SDBPAYGATE")).trim();
              fromSign = System.Web.HttpUtility.UrlDecode(fromSign, Encoding.GetEncoding("GBK"));
            fromOrig = System.Web.HttpUtility.UrlDecode(fromOrig, Encoding.GetEncoding("GBK"));
            fromSign = Base64.DecodeBase64(fromSign, encoding);
            fromOrig = Base64.DecodeBase64(fromOrig, encoding);

            try
            {
                recv.addDataField("toSign", toSign);
                recv.addDataField("toOrig", toOrig);
                recv.addDataField("fromSign", fromSign);
                recv.addDataField("fromOrig", fromOrig);
            }
            catch (Exception e)
            {
              //  System.out.println("Exception:" + e);
            }

            return   fromOrig  ;

        }


        /// <summary>
        /// 中间接口操作  
        /// </summary>
        /// <param name="businessCode"></param>
        /// <param name="input"></param>
        /// <param name="toUrl"></param>
        /// <returns></returns>

        public static KeyedCollection NETExecute(string businessCode,string input,string toUrl)
        {
            KeyedCollection recv = new KeyedCollection();
            String outputString = getDataFromPayGate2Dotnet(businessCode, input , toUrl, recv);

            KeyedCollection output = new KeyedCollection();
            try
            {
                output = (KeyedCollection)DataElementSerializer.serializeFrom(outputString);
                output.setName("output");
                output.put("sendSign", recv.getDataValue("toSign"));
                output.put("sendOrig", recv.getDataValue("toOrig"));
                output.put("orig", recv.getDataValue("fromOrig"));
                output.put("sign", recv.getDataValue("fromSign"));
            }
            catch (Exception e1)
            {
                throw  new Exception("返回报文解析失败！");
            }
            return output;
        }


        /// <summary>
        /// 单笔订单状态查询
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static string KH0001Data(string orderId)
        {
            com.ecc.emp.data.KeyedCollection input = new com.ecc.emp.data.KeyedCollection("input");
         com.ecc.emp.data.KeyedCollection output = new com.ecc.emp.data.KeyedCollection("output");

            input.put("masterId", SDKConfig.MasterID);  //商户号，注意生产环境上要替换成商户自己的生产商户号
            input.put("orderId", orderId);  //订单号，严格遵守格式：商户号+8位日期YYYYMMDD+8位流水

            KeyedCollection recv = new KeyedCollection();
            String businessCode = "KH0001";
            String toOrig     = input.toString().replace("\n", "").replace("\t", "");
            String toUrl = SDKConfig.sdbQueryUrl+"KH0001.pay";  

            output = NETExecute(businessCode, toOrig, toUrl);

            String errorCode = (String)output.getDataValue("errorCode");
            String errorMsg = (String)output.getDataValue("errorMsg");
             

            if ((errorCode == null || errorCode.Equals("")) && (errorMsg == null || errorMsg.Equals("")))
            {
                //System.out.println("---订单状态---" + output.getDataValue("status"));
                //System.out.println("---支付完成时间---" + output.getDataValue("date"));
                //System.out.println("---手续费金额---" + output.getDataValue("charge"));
                //System.out.println("---商户号---" + output.getDataValue("masterId"));
                //System.out.println("---订单号---" + output.getDataValue("orderId"));
                //System.out.println("---币种---" + output.getDataValue("currency"));
                //System.out.println("---订单金额---" + output.getDataValue("amount"));
                //System.out.println("---下单时间---" + output.getDataValue("paydate"));
                //System.out.println("---商品描述---" + output.getDataValue("objectName"));
                //System.out.println("---订单有效期---" + output.getDataValue("validtime"));
                //System.out.println("---备注---" + output.getDataValue("remark"));
                //System.out.println("---本金清算标志---" + output.getDataValue("settleflg"));  //1已清算，0待清算
                //System.out.println("---本金清算时间---" + output.getDataValue("settletime"));
                //System.out.println("---手续费清算标志---" + output.getDataValue("chargeflg"));  //1已清算，0待清算
                //System.out.println("---手续费清算时间---" + output.getDataValue("chargetime"));
            }
            else
            {
             //   System.out.println("---错误码---" + output.getDataValue("errorCode"));
            //    System.out.println("---错误说明---" + output.getDataValue("errorMsg"));
            }
            return output.toString();
        }



        /// <summary>
        /// 订单列表信息查询
        /// </summary> 
        /// <returns></returns>
        public static string KH0002Data(string beginDate  , string endDate  )
        {
            com.ecc.emp.data.KeyedCollection input = new com.ecc.emp.data.KeyedCollection("input");
            com.ecc.emp.data.KeyedCollection output = new com.ecc.emp.data.KeyedCollection("output");

            input.put("masterId", SDKConfig.MasterID);  //商户号，注意生产环境上要替换成商户自己的生产商户号
            input.put("beginDate", beginDate);  //查询开始时间（支付完成时间）YYYYMMDDHHMMSS
            input.put("endDate", endDate);//查询结束时间（支付完成时间）YYYYMMDDHHMMSS
            KeyedCollection recv = new KeyedCollection();
            String businessCode = "KH0002";
            String toOrig = input.toString().replace("\n", "").replace("\t", "");
            String toUrl = SDKConfig.sdbQueryUrl + "KH0002.pay";

            output = NETExecute(businessCode, toOrig, toUrl);

            String errorCode = (String)output.getDataValue("errorCode");
            String errorMsg = (String)output.getDataValue("errorMsg");


            if ((errorCode == null || errorCode.Equals("")) && (errorMsg == null || errorMsg.Equals("")))
            {
                //System.out.println("---订单状态---" + output.getDataValue("status"));
                //System.out.println("---支付完成时间---" + output.getDataValue("date"));
                //System.out.println("---手续费金额---" + output.getDataValue("charge"));
                //System.out.println("---商户号---" + output.getDataValue("masterId"));
                //System.out.println("---订单号---" + output.getDataValue("orderId"));
                //System.out.println("---币种---" + output.getDataValue("currency"));
                //System.out.println("---订单金额---" + output.getDataValue("amount"));
                //System.out.println("---下单时间---" + output.getDataValue("paydate"));
                //System.out.println("---商品描述---" + output.getDataValue("objectName"));
                //System.out.println("---订单有效期---" + output.getDataValue("validtime"));
                //System.out.println("---备注---" + output.getDataValue("remark"));
                //System.out.println("---本金清算标志---" + output.getDataValue("settleflg"));  //1已清算，0待清算
                //System.out.println("---本金清算时间---" + output.getDataValue("settletime"));
                //System.out.println("---手续费清算标志---" + output.getDataValue("chargeflg"));  //1已清算，0待清算
                //System.out.println("---手续费清算时间---" + output.getDataValue("chargetime"));
            }
            else
            {
                //   System.out.println("---错误码---" + output.getDataValue("errorCode"));
                //    System.out.println("---错误说明---" + output.getDataValue("errorMsg"));
            }
            return output.toString();
        }

        #endregion

        #region wsdl
        /// <summary>
        /// webservice固定调用方法
        /// </summary>
        /// <returns></returns>
        public static object CallWebServiceObj(string name, object[] obj)
        {
            try
            {
           //     string url = "http://localhost:8080/axis2/services/BankService?wsdl";//wsdl地址  
                string url = SDKConfig.sdbLocalVerifyUrl;
                WebServiceProxy wsd = new WebServiceProxy(url, name);
                object suc =  wsd.ExecuteQuery(name, obj);
              
                //记录结果

                return suc;
            }
            catch (Exception e)
            {
                return "wsdlFail";
            }

        }



        /// <summary>
        /// webservice固定调用方法
        /// </summary>
        /// <returns></returns>
        private static string CallWebService(string name, object[] obj)
        {
            try
            {
                string url = "http://localhost:8080/axis2/services/BankService?wsdl";//wsdl地址  
                WebServiceProxy wsd = new WebServiceProxy(url, name);
                string suc = (string)wsd.ExecuteQuery(name, obj);
                if (obj.Length == 1)
                {
                    if (obj[0] != null)
                    {
                        //记录？
                    }

                }
                if (obj.Length == 2)
                {
                    if (obj[1] != null)
                    {   //记录？
                    }
                }

                //记录结果

                return suc;
            }
            catch (Exception e)
            {
                return "wsdlFail";
            }

        }

        #endregion


    }
}
