using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class MatchingRequest :ClientRequest {

    public override void OnEvent(EventData eventData)
    {

    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        EResponse eResponse = (EResponse)operationResponse.ReturnCode;

        bool isTrueResponse = false;

        if (eResponse == EResponse.True)
            isTrueResponse = true;
        print("匹配服务器反馈 : " + isTrueResponse);
        List<string> playerList = new List<string>();
        //如果匹配成功，获取玩家名字
        if (isTrueResponse)
        {
            object player0 = null;
            object player1 = null;
            object player2 = null;
            object player3 = null;

            if (operationResponse.Parameters.TryGetValue((byte)EPlayerInfo.Player0Name, out player0))
                if (operationResponse.Parameters.TryGetValue((byte)EPlayerInfo.Player1Name, out player1))
                    if (operationResponse.Parameters.TryGetValue((byte)EPlayerInfo.Player2Name, out player2))
                        if (operationResponse.Parameters.TryGetValue((byte)EPlayerInfo.Player3Name, out player3))
                        {
                            ;
                        }
            
            if(player0 != null)
            {
                playerList.Add(player0 as string);
                if (player1 != null)
                {
                    playerList.Add(player1 as string);
                    if (player2 != null)
                    {
                        playerList.Add(player2 as string);
                        if (player3 != null)
                        {
                            playerList.Add(player3 as string);
                        }
                    }
                }
            }

            print("玩家信息");
            foreach (var item in playerList)
            {
                print(item + " ");
            }

        }
        


        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.MacthingResponse,new MatchingGameNF{ msgType = ENotificationMsgType.MatchingGame, isMatchingGame = isTrueResponse,playerNameList = playerList });

    }

    public override void PostRequest(Notification notification)
    {
        MatchingGameNF nF = notification.parm as MatchingGameNF;
        if (nF.msgType != ENotificationMsgType.MatchingGame) return;
        EMatchingGame eMatching = EMatchingGame.False;
        if (nF.isMatchingGame) eMatching = EMatchingGame.True;

        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        dictionary.Add((byte)EMatchingType.IsMatchingGame, eMatching);
        dictionary.Add((byte)EMatchingType.PlayerCount, nF.matchCount);
        print(nF.matchCount);
        PhotonClientConnect.PhotonPeer.OpCustom((byte)EOperationCode.MatchingGame, dictionary, true);
        print("发送");
    }
    
}
