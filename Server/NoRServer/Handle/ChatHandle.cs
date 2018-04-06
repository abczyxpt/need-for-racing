using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;

namespace NoRServer.Handle
{
    public class ChatHandle : BaseHandle
    {
        public ChatHandle()
        {
            this.eOperationCode = EOperationCode.Chat;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            //服务器截取聊天信息
            Tools.DictTool.TryGetHandle(operationRequest.Parameters, (byte)EChat.ChatName, out object chatName);
            Tools.DictTool.TryGetHandle(operationRequest.Parameters, (byte)EChat.ChatInfo, out object chatInfo);

            foreach (var player in NoRServer.Get.peerList)
            {
                if (player != peer)
                {
                    EventData data = new EventData
                    {
                        Parameters = operationRequest.Parameters,
                        Code = (byte)eOperationCode
                    };
                    player.SendEvent(data, player.sendParameters);
                }
            }
        }
    }
}
