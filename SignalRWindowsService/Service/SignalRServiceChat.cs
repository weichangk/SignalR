using log4net;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using System.Configuration;
using System.ServiceProcess;

[assembly: OwinStartup(typeof(SignalRWindowsService.Startup))]
namespace SignalRWindowsService
{
    partial class SignalRServiceChat : ServiceBase
    {
        public static ILog log = LogManager.GetLogger("SignalR Server Log");
        public static string SignalRURI = ConfigurationManager.AppSettings["SignalRServerUrl"].ToString().Trim();
        public SignalRServiceChat()
        {
            InitializeComponent();
            base.ServiceName = "SignalRServiceChat";
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            WebApp.Start<Startup>(SignalRURI);
            log.Info($"服务开启成功,运行在{SignalRURI}");
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            log.Info($"服务关闭,服务地址：{SignalRURI}");
        }
    }
}
