using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class MatchingRequest :ClientRequest {


    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        EResponse eResponse = (EResponse)operationResponse.ReturnCode;

        bool isTrueResponse = false;

        if (eResponse == EResponse.True)
            isTrueResponse = true;
        print("匹配服务器反馈 : " + isTrueResponse);

        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.MacthingResponse,new MatchingGameNF{ msgType = ENotificationMsgType.MatchingGame, isMatchingGame = isTrueResponse });

    }

    public override void PostRequest(Notification notification)
    {
        MatchingGameNF nF = notification.parm as MatchingGameNF;
        if (nF.msgType != ENotificationMsgType.MatchingGame) return;
        EMatchingGame eMatching = EMatchingGame.False;
        if (nF.isMatchingGame) eMatching = EMatchingGame.True;

        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        dictionary.Add(0, eMatching);

        PhotonClientConnect.PhotonPeer.OpCustom((byte)EOperationCode.MatchingGame, dictionary, true);
        print("发送");
    }
    
}
