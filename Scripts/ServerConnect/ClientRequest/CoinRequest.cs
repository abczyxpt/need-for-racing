using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class CoinRequest :ClientRequest {


    public override void OnEvent(EventData eventData)
    {
        object coinCount;
        eventData.Parameters.TryGetValue((byte)ECoin.Num, out coinCount);

        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.CoinFromServer, new CoinNF { count = int.Parse(coinCount.ToString()) });

    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        object coinCount;
        operationResponse.Parameters.TryGetValue((byte)ECoin.Num, out coinCount);
        
        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.CoinFromServer, new CoinNF { count = int.Parse(coinCount.ToString()) });

    }

    public override void PostRequest(Notification notification)
    {
        CoinNF nf = notification.parm as CoinNF;

        Dictionary<byte, object> dict = new Dictionary<byte, object>
        {
            {(byte)ECoin.Num,nf.count },
        };
        print("发送" + nf.count);
        PhotonClientConnect.PhotonPeer.OpCustom((byte)eOperationCode, dict, true);
    }
    
}
