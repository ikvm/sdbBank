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

        public listInfos listInfos;


    }



    public class listInfos
    {
        [XmlElement("list")]
        public List<listInfo> listInfoList;
    }


    public class listInfo
    {
        [XmlAttribute]
        public string TranTime1 { get; set; }
        [XmlAttribute]
        public string HostSeqNo { get; set; }
        [XmlAttribute]
        public string DetailSerialNo { get; set; }
        [XmlAttribute]
        public string BussSeqNo { get; set; }

        [XmlAttribute]
        public string SummonNo { get; set; }
        [XmlAttribute]
        public string SendBank { get; set; }
        [XmlAttribute]
        public string SendBankNode { get; set; }
        [XmlAttribute]
        public string SendAccount { get; set; }



        [XmlAttribute]
        public string SendName { get; set; }
        [XmlAttribute]
        public string TxAmount { get; set; }
        [XmlAttribute]
        public string AcctBank { get; set; }
        [XmlAttribute]
        public string AcctBankNode { get; set; }

        [XmlAttribute]
        public string AcctAccount { get; set; }
        [XmlAttribute]
        public string AcctName { get; set; }
        [XmlAttribute]
        public string TxType { get; set; }
        [XmlAttribute]
        public string AbstractStr { get; set; }

        [XmlAttribute]
        public string Notes { get; set; }
        [XmlAttribute]
        public string Fee1 { get; set; }
        [XmlAttribute]
        public string Fee2 { get; set; }
        [XmlAttribute]
        public string AbstractStr_Desc { get; set; }


        [XmlAttribute]
        public string CVoucherNo { get; set; }
        [XmlAttribute]
        public string CstInnerFlowNo { get; set; }
        [XmlAttribute]
        public string TranChannel { get; set; }
        [XmlAttribute]
        public string TranCode { get; set; }

        [XmlAttribute]
        public string HostDate { get; set; }
    }

  

}
