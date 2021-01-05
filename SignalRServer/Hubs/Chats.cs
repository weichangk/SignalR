using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Threading.Tasks;

namespace SignalRServer
{
    /// <summary>
    /// 即使通信服务(可供客户端调用的方法开头用小写)
    /// </summary>
    [HubName("ChatsHub")]
    public class Chats : Hub
    {
        #region 重载Hub方法
        /// <summary>
        /// 建立连接
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            AddOnline();
            return base.OnConnected();
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="stopCalled">是否是客户端主动断开：true是,false超时断开</param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            RemoveOnline();
            return base.OnDisconnected(stopCalled);
        }
        /// <summary>
        /// 重新建立连接
        /// </summary>
        /// <returns></returns>
        public override Task OnReconnected()
        {
            AddOnline();
            return base.OnReconnected();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 添加在线用户
        /// </summary>
        private void AddOnline()
        {
            string clientId = Context.ConnectionId;
            string userId = GetUserId();
            Groups.Add(clientId, userId);
            Console.WriteLine($"ClientId:{clientId} UserId:{userId} AddOnline");
            Startup.log.Info($"ClientId:{clientId} UserId:{userId} AddOnline");
        }
        /// <summary>
        /// 移除在线用户
        /// </summary>
        private void RemoveOnline()
        {
            string clientId = Context.ConnectionId;
            string userId = GetUserId();
            Groups.Remove(clientId, userId);
            Console.WriteLine($"ClientId:{clientId} UserId:{userId} RemoveOnline");
            Startup.log.Info($"ClientId:{clientId} UserId:{userId} RemoveOnline");
        }
        /// <summary>
        /// 获取登录用户Id
        /// </summary>
        /// <returns></returns>
        private string GetUserId()
        {
            string userId = "";
            if (Context.QueryString["UserId"] != null)
            {
                userId = Context.QueryString["UserId"];
            }
            return userId;
        }
        #endregion

        #region 客户端操作
        /// <summary>
        /// 根据接收方客户端id发送消息
        /// </summary>
        /// <param name="sendUserId">发送方Id</param>
        /// <param name="revUserId">接收方Id，多客户端Id相同则都收到信息</param>
        /// <param name="msg">消息</param>
        /// <param name="time">发送时间</param>
        /// <param name="isSysMsg">是否系统消息</param>
        public void SendMsgByUserId(string sendUserId, string revUserId, string msg, DateTime time, bool isSysMsg)
        {
            Clients.Group(revUserId).RevMsg(sendUserId, msg, time, isSysMsg);
            Console.WriteLine($"{time} isSysMsg:{isSysMsg} Id:{sendUserId} Send To Id:{revUserId} Msg:{msg}");
            Startup.log.Info($"{time} isSysMsg:{isSysMsg} Id:{sendUserId} Send To Id:{revUserId} Msg:{msg}");
        }
        /// <summary>
        /// 向所有客户端发送消息
        /// </summary>
        /// <param name="sendUserId">发送方Id</param>
        /// <param name="msg">消息</param>
        /// <param name="time">发送时间</param>
        /// <param name="isSysMsg">是否系统消息</param>
        public void SendMsgAll(string sendUserId, string msg, DateTime time, bool isSysMsg)
        {
            this.Clients.All.RevMsg(sendUserId, msg, time, isSysMsg);
            Console.WriteLine($"{time} isSysMsg:{isSysMsg} Id:{sendUserId} Send Msg:{msg}");
            Startup.log.Info($"{time} isSysMsg:{isSysMsg} Id:{sendUserId} Send Msg:{msg}");
        }
        #endregion
    }
}
