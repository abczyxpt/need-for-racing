
using System.Collections.Generic;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using NoRServer.Manager;
using NoRServer.Model;
using NoRServer.Handle;
using NoRServer.Tools;

namespace NoRServer
{
    /// <summary>
    /// 用于管理客户端的连接问题
    /// </summary>
    public class ClientPeer : Photon.SocketServer.ClientPeer
    {
        public string Username { get; set; }
        public string UserCar { get; set; }
        public int MatchingCount { get; set; }
        public SendParameters sendParameters;

        public List<ClientPeer> FoePeer { get; } //该玩家的对手

        public ClientPeer(InitRequest initRequest) : base(initRequest)
        {
            FoePeer = new List<ClientPeer>();
        }
        
        public void AddFoePeer(ClientPeer foePeer)
        {
            FoePeer.Add(foePeer);
        }


        //服务器断开后工作
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            LogInit.Log.Info("客户端断开链接");
            FoePeer.Clear();
            NoRServer.Get.PeerFindGame(this, MatchingCount);
            NoRServer.Get.peerList.Remove(this);
            //更改在线状态
            UserManager.Get.ChangeLoading(Username, false);
        }

        //服务器收到客户端请求
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            #region 测试用代码
            //switch (operationRequest.OperationCode)
            //{
            //    case (byte)EOperationCode.ConnectText:
            //        LogInit.Log.Info("收到请求");

            //        Dictionary<byte, object> dictionary = operationRequest.Parameters;
            //        object textOneData;
            //        dictionary.TryGetValue((byte)ETextCode.One, out textOneData);
            //        object textTwoData;
            //        dictionary.TryGetValue((byte)ETextCode.Two, out textTwoData);

            //        LogInit.Log.Info(ETextCode.One.ToString() + "的值为" + textOneData.ToString() + "\n");
            //        LogInit.Log.Info(ETextCode.Two.ToString() + "的值为" + textTwoData.ToString() + "\n");

            //        AddData(textOneData.ToString(), textTwoData.ToString());
            //        //回传数据
            //        OperationResponse operation = new OperationResponse((byte)EOperationCode.ConnectText);
            //        dictionary = new Dictionary<byte, object>
            //        {
            //            { (byte)ETextCode.One, "明白" },
            //            { (byte)ETextCode.Two, "OK" }
            //        };

            //        operation.Parameters = dictionary;
            //        SendOperationResponse(operation, sendParameters);

            //        EventData eventData = new EventData((byte)EOperationCode.ConnectText);
            //        dictionary = new Dictionary<byte, object>
            //        {
            //            { (byte)ETextCode.One, "这里是测试event" },
            //            { (byte)ETextCode.Two, "abczyx" }
            //        };
            //        eventData.Parameters = dictionary;
            //        SendEvent(eventData,new SendParameters());



            //        break;
            //    default:
            //        break;
            //}
            //private void AddData(string name,string psw)
            //{
            //    User user = new User() { Username = name, Password = psw };
            //    Manager.UserManager.Get.Add(user);
            //}
            #endregion
            this.sendParameters = sendParameters;
            DictTool.TryGetHandle(NoRServer.Get.handleDict, (EOperationCode)operationRequest.OperationCode, out BaseHandle handle);
            handle.OnOperationRequest(operationRequest, sendParameters, this);
        }

        
    }
}
