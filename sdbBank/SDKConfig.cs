using System.Web.Configuration;
using System.Configuration;

namespace sdbBank
{
    public class SDKConfig
    {
        private static Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        private static string masterId = config.AppSettings.Settings["sdbmasterId"].Value;   //商户id
        private static string gatewayurl = config.AppSettings.Settings["sdbgatewayurl"].Value; //功能：读取配置文件获取前台交易地址
        private static string signpfxPath = config.AppSettings.Settings["sdbsignpfxpath"].Value;  //功能：读取配置文件获取签名证书路径
        private static string signcerPath = config.AppSettings.Settings["sdbsignCerpath"].Value;  //功能：读取配置文件获取签名证书路径_公钥
        private static string frontUrl = config.AppSettings.Settings["sdbfrontUrl"].Value;//功能：读取配置文件获取前台通知地址
        private static string backUrl = config.AppSettings.Settings["sdbbackUrl"].Value;//功能：读取配置文件获取前台通知地址
        private static string sdbpublickey = config.AppSettings.Settings["sdbPublicKey"].Value;//功能：读取配置文件获取公钥
        private static string sdbkeypass = config.AppSettings.Settings["sdbKeyPass"].Value;//功能：读取配置文件获取公钥
        private static string sdblocalverifyUrl = config.AppSettings.Settings["sdbLocalVerifyUrl"].Value;//功能：读取配置文件获取本地验签地址
        
        private static string sdbqueryurl = config.AppSettings.Settings["sdbQueryUrl"].Value;//功能：读取配置文件获取查询地址
        private static string sdbunionurl = config.AppSettings.Settings["sdbUnionUrl"].Value;//功能：读取配置文件获取银联查询地址

        private static string UnionsdbfrontUrl = config.AppSettings.Settings["UnionsdbfrontUrl"].Value;//功能：读取配置文件获取前台通知地址
        private static string UnionsdbbackUrl = config.AppSettings.Settings["UnionsdbbackUrl"].Value;//功能：读取配置文件获取前台通知地址



        public static string UnionsdbFrontUrl
        {
            get { return SDKConfig.UnionsdbfrontUrl; }
            set { SDKConfig.UnionsdbfrontUrl = value; }
        }
        public static string UnionsdbBackUrl
        {
            get { return SDKConfig.UnionsdbbackUrl; }
            set { SDKConfig.UnionsdbbackUrl = value; }
        }



        public static string sdbLocalVerifyUrl
        {
            get { return SDKConfig.sdblocalverifyUrl; }
            set { SDKConfig.sdblocalverifyUrl = value; }
        }


        public static string sdbQueryUrl
        {
            get { return SDKConfig.sdbqueryurl; }
            set { SDKConfig.sdbqueryurl = value; }
        }

        public static string sdbUnionUrl
        {
            get { return SDKConfig.sdbunionurl; }
            set { SDKConfig.sdbunionurl = value; }
        }



        public static string sdbKeyPass
        {
            get { return SDKConfig.sdbkeypass; }
            set { SDKConfig.sdbkeypass = value; }
        }
        public static string sdbPublicKey
        {
            get { return SDKConfig.sdbpublickey; }
            set { SDKConfig.sdbpublickey = value; }
        }

        public static string MasterID
        {
            get { return SDKConfig.masterId; }
            set { SDKConfig.masterId = value; }
        }

        public static string GATEWAYUrl
        {
            get { return SDKConfig.gatewayurl; }
            set { SDKConfig.gatewayurl = value; }
        }
        public static string BackUrl
        {
            get { return SDKConfig.backUrl; }
            set { SDKConfig.backUrl = value; }
        }
        public static string FrontUrl
        {
            get { return SDKConfig.frontUrl; }
            set { SDKConfig.frontUrl = value; }
        }
        public static string SignPFXPath
        {
            get { return SDKConfig.signpfxPath; }
            set { SDKConfig.signpfxPath = value; }
        }
        public static string SignCERPath
        {
            get { return SDKConfig.signcerPath; }
            set { SDKConfig.signcerPath = value; }
        }
    }
}
