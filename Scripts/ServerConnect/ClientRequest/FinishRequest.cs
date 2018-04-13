using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class FinishRequest : ClientRequest
{
    public override void OnEvent(EventData eventData)
    {
        object isWin;
        object foeName;
        eventData.Parameters.TryGetValue((byte)EGameFinish.Win, out isWin);
        //1.有玩家跑到终点
        if (!eventData.Parameters.TryGetValue((byte)EGameFinish.PlayerName, out foeName))
        {
            GameFinishNF nf = new GameFinishNF
            {
                isWin = (bool)isWin
            };
            MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.GameFinish, nf);
        }
        //2.有玩家认输
        else
        {
            SurrenderNF nf = new SurrenderNF
            {
                isWin = (bool)isWin,
                foeName = foeName.ToString()
            };
            MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.FoeSurrender, nf);
        }
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
    }

    public override void PostRequest(Notification notification)
    {
        GameFinishNF nF = notification.parm as GameFinishNF;
        print("发送反馈" + nF.isWin);

        PhotonClientConnect.PhotonPeer.OpCustom((byte)EOperationCode.GameFinish, new Dictionary<byte, object> { { (byte)EGameFinish.Win, nF.isWin }},true);

    }
}