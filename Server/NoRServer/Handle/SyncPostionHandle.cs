using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;

namespace NoRServer.Handle
{
    class SyncPostionHandle :BaseHandle
    {
        public SyncPostionHandle()
        {
            this.eOperationCode = EOperationCode.SyncPostionHandle;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {

            if (operationRequest.OperationCode != (byte)eOperationCode) return;

            Tools.DictTool.TryGetHandle(operationRequest.Parameters, (byte)EPostionInfo.IsSetPostion, out object isSetPostion);
            

            if (!((bool)isSetPostion))
            {
                return;
            }
            //获取三个位置信息
            Tools.DictTool.TryGetHandle(operationRequest.Parameters, (byte)EPostionInfo.FoeVertical, out object FoeVertical);
            Tools.DictTool.TryGetHandle(operationRequest.Parameters, (byte)EPostionInfo.FoeHorizontal, out object FoeHorizontal);
            Tools.DictTool.TryGetHandle(operationRequest.Parameters, (byte)EPostionInfo.FoeBrake, out object FoeBrake);

            //分发给他的敌人
            foreach (var foePeer in peer.FoePeer)
            {
                EventData eventData = new EventData((byte)eOperationCode)
                {
                    Parameters = new Dictionary<byte, object>
                    {
                        {(byte)EPostionInfo.FoeVertical,FoeVertical },
                        {(byte)EPostionInfo.FoeHorizontal,FoeHorizontal },
                        {(byte)EPostionInfo.FoeBrake,FoeBrake },
                        {(byte)EPostionInfo.PlayerName,peer.Username},
                        {(byte)EPostionInfo.IsGetPostion,true},

                    }
                };

                foePeer.SendEvent(eventData, foePeer.sendParameters);
            }
        }
    }
}
