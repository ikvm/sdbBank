using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace sdbBank
{
 
    /// <summary>
    /// 余额返回
    /// </summary>
    [XmlRoot("Result")]
    public   class YQ4001Model
    {
        public string Account { get; set; }
        public string CcyCode { get; set; }
        public string CcyType { get; set; }
        public string AccountName { get; set; }
        public string Balance { get; set; }
        public string TotalAmount { get; set; }
        public string AccountType { get; set; }
        public string AccountStatus { get; set; }
        public string BankName { get; set; }
        public string LastBalance { get; set; }
        public string HoldBalance { get; set; }
    }



    /// <summary>
    /// 跨行快付提交返回
    /// </summary>
    [XmlRoot("Result")]
    public class YQKHKF03Model
    {
        public string OrderNumber { get; set; }
        public string BussFlowNo { get; set; }
       

    }




    /// <summary>
    /// 跨行快付结果返回
    /// </summary>
    [XmlRoot("Result")]
    public class YQKHKF04Model
    {
        public string OrderNumber { get; set; }
        public string BussFlowNo { get; set; }
        public string TranFlowNo { get; set; }
        public string Status { get; set; }
        public string RetCode { get; set; }
        public string RetMsg { get; set; }
        public string SettleDate { get; set; }
        public string CcyCode { get; set; }
        public string TranAmount { get; set; }
        public string InAcctNo { get; set; }
        public string InAcctName { get; set; }
        public string Mobile { get; set; }
        public string Remark { get; set; }
  
    }



}
