using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace NoRServer
{
    /// <summary>
    /// 用于管理客户端的连接问题
    /// </summary>
    public class ClientPeer : Photon.SocketServer.ClientPeer
    {
        public ClientPeer(InitRequest initRequest) : base(initRequest)
        {

        }
        

        //服务器断开后工作
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            
        }

        //服务器启动工作
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            
        }
    }
}
