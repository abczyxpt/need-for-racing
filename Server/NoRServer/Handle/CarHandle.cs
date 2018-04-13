using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;

namespace NoRServer.Handle
{
    public class CarHandle : BaseHandle
    {
        public CarHandle()
        {
            this.eOperationCode = EOperationCode.CarHave;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            Tools.DictTool.TryGetHandle(operationRequest.Parameters, (byte)ECarHave.CarName, out object carName);
            Tools.DictTool.TryGetHandle(operationRequest.Parameters, (byte)ECarHave.IsBuy, out object isBuy);

            LogInit.Log.Info("收到的汽车名字" + carName.ToString());

            bool isHave = Manager.CarManager.Get.HaveCar(peer.Username, carName.ToString());

            //如果买车，那么处理买车
            if ((bool)isBuy)
            {
                isHave = Manager.CarManager.Get.BuyCar(peer.Username, carName.ToString());
                isBuy = isHave;
                if ((bool)isBuy)
                {
                    //如果购买成功，那么就反馈客户端金币改变
                    EventData data = new EventData
                    {
                        Code = (byte)EOperationCode.Coin,
                        Parameters = new Dictionary<byte, object>
                        {
                         {(byte)ECoin.Num,Manager.UserManager.Get.GetCoinByName(peer.Username) },
                        },
                    };
                    peer.SendEvent(data, peer.sendParameters);
                }
            }


            OperationResponse operation = new OperationResponse
            {
                OperationCode = (byte)this.eOperationCode,
                Parameters = new Dictionary<byte, object>
                 {
                    {(byte)ECarHave.CarName,carName },
                    {(byte)ECarHave.IsHave,isHave },
                    {(byte)ECarHave.IsBuy,isBuy }
                 },
            };
            peer.SendOperationResponse(operation, peer.sendParameters);            
        }
        
    }
}
