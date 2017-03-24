using System.Web.Configuration;
using System.Configuration;

namespace sdbBank
{
    public class SDKConfig
    {
        private static Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        private static string masterId = config.AppSettings.Settings["sdbmasterId"].Value;   //商户id
        private static string gatewayurl = config.AppSettings.Settings["sdbgatewayurl"].Value; //功能：读取配置文件获取前台交易地址
        private static string signCertPath = config.AppSettings.Settings["sdbsignCertpath"].Value;  //功能：读取配置文件获取签名证书路径
        private static string frontUrl = config.AppSettings.Settings["sdbfrontUrl"].Value;//功能：读取配置文件获取前台通知地址
        private static string backUrl = config.AppSettings.Settings["sdbbackUrl"].Value;//功能：读取配置文件获取前台通知地址

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
        public static string SignCertPath
        {
            get { return SDKConfig.signCertPath; }
            set { SDKConfig.signCertPath = value; }
        }
    }
}
