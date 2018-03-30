
using System.Collections.Generic;
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
            LogInit.Log.Info("客户端断开链接");
        }

        //服务器收到客户端请求
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            switch (operationRequest.OperationCode)
            {
                case (byte)EOperationCode.ConnectText:
                    LogInit.Log.Info("收到请求");

                    Dictionary<byte, object> dictionary = operationRequest.Parameters;
                    object textOneData;
                    dictionary.TryGetValue((byte)ETextCode.One, out textOneData);
                    object textTwoData;
                    dictionary.TryGetValue((byte)ETextCode.Two, out textTwoData);

                    LogInit.Log.Info(ETextCode.One.ToString() + "的值为" + textOneData.ToString() + "\n");
                    LogInit.Log.Info(ETextCode.Two.ToString() + "的值为" + textTwoData.ToString() + "\n");

                    AddData();
                    //回传数据
                    OperationResponse operation = new OperationResponse((byte)EOperationCode.ConnectText);
                    dictionary = new Dictionary<byte, object>
                    {
                        { (byte)ETextCode.One, "明白" },
                        { (byte)ETextCode.Two, "OK" }
                    };

                    operation.Parameters = dictionary;
                    SendOperationResponse(operation, sendParameters);

                    EventData eventData = new EventData((byte)EOperationCode.ConnectText);
                    dictionary = new Dictionary<byte, object>
                    {
                        { (byte)ETextCode.One, "这里是测试event" },
                        { (byte)ETextCode.Two, "abczyx" }
                    };
                    eventData.Parameters = dictionary;
                    SendEvent(eventData,new SendParameters());



                    break;
                default:
                    break;
            }
        }

        private void AddData()
        {
            //IUserManager userManager = new UserManager();
            //User user = new User() { Username = "ybsb", Password = "ybsb" };
            //userManager.Add(user);
        }
    }
}
