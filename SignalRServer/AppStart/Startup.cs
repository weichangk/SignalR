using Microsoft.AspNet.SignalR;
using Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using System;
using System.Reflection;
using System.Configuration;
using log4net;

namespace SignalRServer
{
    public class Startup
    {
        public static ILog log = LogManager.GetLogger("SignalR Server Log");
        /// <summary>
        /// 开启服务
        /// </summary>
        public static void Start()
        {
            //string SignalRURI = @"https://localhost:44342/";
            string SignalRURI = ConfigurationManager.AppSettings["SignalRServerUrl"].ToString().Trim();
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                try
                {
                    using (WebApp.Start(SignalRURI, builder =>
                    {
                        builder.Map("/signalr", map =>
                        {
                            // Setup the cors middleware to run before SignalR.
                            // By default this will allow all origins. You can 
                            // configure the set of origins and/or http verbs by
                            // providing a cors options with a different policy.
                            map.UseCors(CorsOptions.AllowAll);
                            var hubConfiguration = new HubConfiguration
                            {
                                // You can enable JSONP by uncommenting line below.
                                // JSONP requests are insecure but some older browsers (and some
                                // versions of IE) require JSONP to work cross domain
                                EnableJSONP = true
                            };
                            // Run the SignalR pipeline. We're not using MapSignalR
                            // since this branch is already runs under the "/signalr"
                            // path.
                            map.RunSignalR(hubConfiguration);
                        });
                        builder.MapSignalR();

                    }))
                    {
                        Console.WriteLine("服务开启成功,运行在{0}", SignalRURI);
                        log.Info($"服务开启成功,运行在{SignalRURI}");
                        Console.ReadLine();
                    }
                }
                catch (TargetInvocationException)
                {
                    Console.WriteLine("服务开启失败. 已经有一个服务运行在{0}", SignalRURI);
                    log.Error($"服务开启失败. 已经有一个服务运行在{SignalRURI}");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("服务开启异常：{0}", ex.ToString());
                log.Error($"服务开启异常：{ex}");
                Console.ReadLine();
            }

        }
    }
}
