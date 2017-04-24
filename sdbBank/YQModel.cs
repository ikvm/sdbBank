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



   
   
 
    /// <summary>
    ///当日明细查询结果集返回
    /// </summary>
    [XmlRoot("Result")]
    public class YQ4008Model
    {
        public string AcctNo { get; set; }
        public string CcyCode { get; set; }
        public string PageNo { get; set; }
        public string PageSize { get; set; }
        public string TranDate { get; set; }
        public string EndFlag { get; set; }
        public string PageRecCount { get; set; }
        public string JournalNo { get; set; }
        public string LogCount { get; set; }

        [XmlElement("list")]
        public List<listInfo> listInfoList;

    }
 


    public class listInfo
    {
        [XmlElement]
        public string TranTime1 { get; set; }
        [XmlElement]
        public string HostSeqNo { get; set; }
        [XmlElement]
        public string DetailSerialNo { get; set; }
        [XmlElement]
        public string BussSeqNo { get; set; }

        [XmlElement]
        public string SummonNo { get; set; }
        [XmlElement]
        public string SendBank { get; set; }
        [XmlElement]
        public string SendBankNode { get; set; }
        [XmlElement]
        public string SendAccount { get; set; }



        [XmlElement]
        public string SendName { get; set; }
        [XmlElement]
        public string TxAmount { get; set; }
        [XmlElement]
        public string AcctBank { get; set; }
        [XmlElement]
        public string AcctBankNode { get; set; }

        [XmlElement]
        public string AcctAccount { get; set; }
        [XmlElement]
        public string AcctName { get; set; }
        [XmlElement]
        public string TxType { get; set; }
        [XmlElement]
        public string AbstractStr { get; set; }

        [XmlElement]
        public string Notes { get; set; }
        [XmlElement]
        public string Fee1 { get; set; }
        [XmlElement]
        public string Fee2 { get; set; }
        [XmlElement]
        public string AbstractStr_Desc { get; set; }


        [XmlElement]
        public string CVoucherNo { get; set; }
        [XmlElement]
        public string CstInnerFlowNo { get; set; }
        [XmlElement]
        public string TranChannel { get; set; }
        [XmlElement]
        public string TranCode { get; set; }

        [XmlElement]
        public string HostDate { get; set; }
    }




    /// <summary>
    ///历史明细查询结果集返回
    /// </summary>
    [XmlRoot("Result")]
    public class YQ4013Model
    {
        
    }

}
