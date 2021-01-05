using log4net;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System.Configuration;

namespace SignalRWindowsService
{
    //public class Startup
    //{
    //    /// <summary>
    //    /// 开启服务
    //    /// </summary>
    //    public static void Start()
    //    {
    //        //SignalRURI = @"https://localhost:43210/";      
    //        //try
    //        //{
    //        //    log4net.Config.XmlConfigurator.Configure();
    //        //    try
    //        //    {
    //        //        using (WebApp.Start(SignalRURI, builder =>
    //        //        {
    //        //            builder.Map("/signalr", map =>
    //        //            {
    //        //                // Setup the cors middleware to run before SignalR.
    //        //                // By default this will allow all origins. You can 
    //        //                // configure the set of origins and/or http verbs by
    //        //                // providing a cors options with a different policy.
    //        //                map.UseCors(CorsOptions.AllowAll);
    //        //                var hubConfiguration = new HubConfiguration
    //        //                {
    //        //                    // You can enable JSONP by uncommenting line below.
    //        //                    // JSONP requests are insecure but some older browsers (and some
    //        //                    // versions of IE) require JSONP to work cross domain
    //        //                    EnableJSONP = true
    //        //                };
    //        //                // Run the SignalR pipeline. We're not using MapSignalR
    //        //                // since this branch is already runs under the "/signalr"
    //        //                // path.
    //        //                map.RunSignalR(hubConfiguration);
    //        //            });
    //        //            builder.MapSignalR();

    //        //        }))
    //        //        {
    //        //            log.Info($"服务开启成功,运行在{SignalRURI}");
    //        //            //Console.ReadLine();
    //        //        }
    //        //    }
    //        //    catch (TargetInvocationException)
    //        //    {
    //        //        log.Error($"服务开启失败. 已经有一个服务运行在{SignalRURI}");
    //        //        //Console.ReadLine();
    //        //    }
    //        //}
    //        //catch (Exception ex)
    //        //{
    //        //    log.Error($"服务开启异常：{ex}");
    //        //    //Console.ReadLine();
    //        //}

    //        //if (!System.Diagnostics.EventLog.SourceExists("SignalRServiceChat"))
    //        //{
    //        //    System.Diagnostics.EventLog.CreateEventSource(
    //        //        "SignalRServiceChat", "Application");

    //        //    WebApp.Start<StartupConfig>(SignalRURI);
    //        //    log.Info($"服务开启成功,运行在{SignalRURI}");
    //        //}

    //    }
    //}

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            var hubConfiguration = new HubConfiguration
            {
                EnableJSONP = true
            };
            app.MapSignalR(hubConfiguration);
        }
    }
}
