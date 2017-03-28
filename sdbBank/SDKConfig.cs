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
