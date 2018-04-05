using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class FinishRequest : ClientRequest
{
    public override void OnEvent(EventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        throw new System.NotImplementedException();
    }

    public override void PostRequest(Notification notification)
    {
        GameFinishNF nF = notification.parm as GameFinishNF;
        EGameFinish finish = EGameFinish.Lost;
        if (nF.isWin) finish = EGameFinish.Win;

        PhotonClientConnect.PhotonPeer.OpCustom((byte)EOperationCode.GameFinish, new Dictionary<byte, object> { { (byte)EGameFinish.Win, true },{ (byte)EGameFinish.Lost, false } },true);

    }
}
