using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;

namespace NoRServer.Handle
{
    class CoinHandle : BaseHandle
    {
        public CoinHandle()
        {
            this.eOperationCode = EOperationCode.Coin;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            Tools.DictTool.TryGetHandle(operationRequest.Parameters, (byte)ECoin.Num, out object coinCount);

            int coinCountServer = Manager.UserManager.Get.GetCoinByName(peer.Username);
            if (int.Parse(coinCount.ToString()) != 0)
            {
                //TODO:金币的操作
                coinCountServer -= int.Parse(coinCount.ToString());
                if(coinCountServer >= 0)
                {
                    Manager.UserManager.Get.ChangeCoin(peer.Username, coinCountServer);
                }                
            }

            OperationResponse operation = new OperationResponse((byte)eOperationCode)
            {
                Parameters = new Dictionary<byte, object>
                 {
                     {(byte)ECoin.Num,coinCountServer },
                 },
            };

            peer.SendOperationResponse(operation, peer.sendParameters);

        }
    }
}
