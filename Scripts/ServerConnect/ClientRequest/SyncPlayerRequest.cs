using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class SyncPlayerRequest : ClientRequest
{
    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        
    }

    public override void PostRequest(Notification notification)
    {
        SyncPlayerNF syncPlayerNF = notification.parm as SyncPlayerNF;
        if (syncPlayerNF.isFind == true) return;

        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        PhotonClientConnect.PhotonPeer.OpCustom((byte)this.eOperationCode, dictionary, true);
        
    }

    public override void OnEvent(EventData eventData)
    {
        
    }
}
