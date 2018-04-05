using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class SyncPostionRequest : ClientRequest {

    public override void OnEvent(EventData eventData)
    {
        object isGetPostion;
        Vector3 postion = Vector3.zero;
        object foePlayerName;
        if (eventData.Parameters.TryGetValue((byte)EPostionInfo.IsGetPostion, out isGetPostion))
        {
            if ((bool)isGetPostion)
            {
                object x, y, z;
                eventData.Parameters.TryGetValue((byte)EPostionInfo.FoeVertical, out x);
                eventData.Parameters.TryGetValue((byte)EPostionInfo.FoeHorizontal, out y);
                eventData.Parameters.TryGetValue((byte)EPostionInfo.FoeBrake, out z);
                
                eventData.Parameters.TryGetValue((byte)EPostionInfo.PlayerName, out foePlayerName);

                SyncPostionNF nf = new SyncPostionNF
                {
                    ctrName = foePlayerName as string,
                    foeVertical = (float)x,
                    foeHorizontal = (float)y,
                    foeBrake = (float)z
                };
                MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.CarControlFromServer,nf);
                print(postion);
            }
        }
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        
    }

    public override void PostRequest(Notification notification)
    {
        SyncPostionNF nF = notification.parm as SyncPostionNF;
        Dictionary<byte, object> dict = new Dictionary<byte, object>
        {
            { (byte)EPostionInfo.IsSetPostion,nF.getPostion },
            { (byte)EPostionInfo.FoeHorizontal,nF.foeHorizontal },
            { (byte)EPostionInfo.FoeVertical,nF.foeVertical },
            { (byte)EPostionInfo.FoeBrake,nF.foeBrake }
        };
        PhotonClientConnect.PhotonPeer.OpCustom((byte)this.eOperationCode, dict, true);
        
    }
    
}
