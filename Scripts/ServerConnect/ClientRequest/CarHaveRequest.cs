using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class CarHaveRequest : ClientRequest {


    public override void OnEvent(EventData eventData)
    {
        
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        object carName;
        object isHave;
        object isBuy;
        operationResponse.Parameters.TryGetValue((byte)ECarHave.CarName, out carName);
        operationResponse.Parameters.TryGetValue((byte)ECarHave.IsHave, out isHave);
        operationResponse.Parameters.TryGetValue((byte)ECarHave.IsBuy, out isBuy);

        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.CarHaveResponse, new CarHanveNF { carName = carName.ToString(), isHave = (bool)isHave,isBuy = (bool)isBuy });
    }

    public override void PostRequest(Notification notification)
    {
        CarHanveNF nf = notification.parm as CarHanveNF;
        Dictionary<byte, object> dict;
        dict = new Dictionary<byte, object>
        {
            {(byte)ECarHave.CarName,nf.carName },
            {(byte)ECarHave.IsBuy,nf.isBuy },
        };
        print("发送购买汽车: " + nf.carName);
        PhotonClientConnect.PhotonPeer.OpCustom((byte)eOperationCode, dict, true);
    }
    
}
