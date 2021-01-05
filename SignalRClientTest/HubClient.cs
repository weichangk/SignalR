using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRClientTest
{
    public class HubClient
    {
        public Action<string> ConnectionError;//连接异常委托
        public event EventHandler<RevMsgAllEventArgs> RevMsgAllEvent;//消息接收事件
        private readonly HubConnection hubConnection;
        private readonly IHubProxy hubProxy;
        public string UserId { get; set; }

        /// <summary>
        /// HubClient构造函数
        /// </summary>
        /// <param name="signalrUrl">SignalR服务地址</param>
        /// <param name="hubName">通信服务名称</param>
        /// <param name="queryString">通信客户端ID查询字符串，多客户端ID相同则同组获取信息</param>
        public HubClient(string signalrUrl, string hubName, string queryString)
        {
            hubConnection = new HubConnection(signalrUrl, new Dictionary<string, string>() { { "UserId", queryString } });
            hubProxy = hubConnection.CreateHubProxy(hubName);
            UserId = queryString;
            hubConnection.Error += (Exception) => { ConnectionError?.Invoke("HubConnection Fail. " + Exception.Message); };//连接异常   
        }

        /// <summary>
        /// 连接测试
        /// </summary>
        public bool StartTest()
        {
            try
            {
                this.hubConnection.Start().Wait();
                this.hubConnection.Dispose();//使用Stop()关闭连接在Start()和Stop()连续切换的时候可能异常
                return true;
            }
            catch (Exception ex)
            {
                ConnectionError?.Invoke("HubConnection Start Test Fail. " + ex.Message);//连接测试异常   
            }
            return false;
        }
        /// <summary>
        /// 开启连接
        /// </summary>
        public void Start()
        {
            if (this.hubConnection.State != ConnectionState.Connected)
            {
                if (StartTest())
                    hubConnection.Start();
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Stop()
        {
            if (this.hubConnection.State == ConnectionState.Connected)
                hubConnection.Dispose();//使用Stop()关闭连接在Start()和Stop()连续切换的时候可能异常
        }

        /// <summary>
        /// 调用hub方法
        /// </summary>
        /// <param name="methodName">SignalR服务方法</param>
        /// <param name="args">SignalR服务方法参数</param>
        public void CallMethod(string methodName, params object[] args)
        {
            if (this.hubConnection.State == ConnectionState.Connected)
                hubProxy.Invoke(methodName, args);
        }

        public void RevMsg()
        {
            this.hubProxy.On("RevMsg",
                  (string sendId, string msg, DateTime time, bool isSyaMsg) =>
                  {
                      RevMsgAllEvent?.Invoke(this, new RevMsgAllEventArgs(sendId, msg, time, isSyaMsg));
                  });
        }
    }

    public class RevMsgAllEventArgs : EventArgs
    {
        public RevMsgAllEventArgs(string sendId, string msg, DateTime time, bool isSyaMsg)
        {
            this.SendId = sendId;
            this.Msg = msg;
            this.Time = time;
            this.IsSysMsg = isSyaMsg;
        }
        public string SendId { get; private set; }
        public string Msg { get; private set; }
        public DateTime Time { get; private set; }
        public bool IsSysMsg { get; private set; }
    }
}
