using System.ServiceProcess;

namespace SignalRWindowsService
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                    new SignalRServiceChat()
            };
            ServiceBase.Run(ServicesToRun);
        }

    }

}
