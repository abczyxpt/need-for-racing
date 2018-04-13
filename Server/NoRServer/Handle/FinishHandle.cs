using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;

namespace NoRServer.Handle
{
    class FinishHandle : BaseHandle
    {
        public FinishHandle()
        {
            this.eOperationCode = EOperationCode.GameFinish;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            Tools.DictTool.TryGetHandle(operationRequest.Parameters, (byte)EGameFinish.Win, out object isWin);

            LogInit.Log.Info("Game end + " + isWin);
            if((bool)isWin)
                Manager.UserManager.Get.ChangeCoin(peer.Username, 50);

            foreach (var foe in peer.FoePeer)
            {
                EventData eventData = new EventData
                {
                    Code = (byte)eOperationCode,
                    Parameters = new Dictionary<byte, object>
                     {
                        {(byte)EGameFinish.Win,isWin }
                     }
                };
                if(!(bool)isWin)
                {
                    eventData.Parameters.Add((byte)EGameFinish.PlayerName, peer.Username);
                }
                foe.SendEvent(eventData, foe.sendParameters);
            }
        }
    }
}
