using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class SyncPostionRequest : ClientRequest {

    public override void OnEvent(EventData eventData)
    {

    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        
    }

    public override void PostRequest(Notification notification)
    {
        SyncPostionNF nF = notification.parm as SyncPostionNF;
        Dictionary<byte, object> dict = new Dictionary<byte, object>
        {
            { (byte)EPostionInfo.GetPostion,nF.getPostion },
            { (byte)EPostionInfo.Postion,nF.postion }
        };
        PhotonClientConnect.PhotonPeer.OpCustom((byte)this.eOperationCode, dict, true);

    }
    
}
