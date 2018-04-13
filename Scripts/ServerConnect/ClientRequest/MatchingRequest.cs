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
        List<PlayerList> playerList = new List<PlayerList>();
        //如果匹配成功，获取玩家名字
        if (isTrueResponse)
        {
            object player0Name = null;
            object player0Car = null;
            object player1Name = null;
            object player1Car = null;
            object player2Name = null;
            object player2Car = null;
            object player3Name = null;
            object player3Car = null;

            if (operationResponse.Parameters.TryGetValue((byte)EPlayerInfo.Player0Car, out player0Car))
                if (operationResponse.Parameters.TryGetValue((byte)EPlayerInfo.Player0Name, out player0Name))
                    if (operationResponse.Parameters.TryGetValue((byte)EPlayerInfo.Player1Car, out player1Car))
                        if (operationResponse.Parameters.TryGetValue((byte)EPlayerInfo.Player1Name, out player1Name))
                            if (operationResponse.Parameters.TryGetValue((byte)EPlayerInfo.Player2Car, out player2Car))
                                if (operationResponse.Parameters.TryGetValue((byte)EPlayerInfo.Player2Name, out player2Name))
                                    if (operationResponse.Parameters.TryGetValue((byte)EPlayerInfo.Player3Car, out player3Car))
                                        if (operationResponse.Parameters.TryGetValue((byte)EPlayerInfo.Player3Name, out player3Name))
                                        {
                                            ;
                                        }

            if (player0Car != null && player0Name != null)
            {
                playerList.Add(new PlayerList {
                    PlayerCar = player0Car.ToString(),
                    PlayerName = player0Name.ToString()
                }); 
                if (player1Car != null && player1Name != null)
                {
                    playerList.Add(new PlayerList
                    {
                        PlayerCar = player1Car.ToString(),
                        PlayerName = player1Name.ToString()
                    });
                    if (player2Car != null && player2Name != null)
                    {
                        playerList.Add(new PlayerList
                        {
                            PlayerCar = player2Car.ToString(),
                            PlayerName = player2Name.ToString()
                        });
                        if (player3Car != null && player3Name != null)
                        {
                            playerList.Add(new PlayerList
                            {
                                PlayerCar = player0Car.ToString(),
                                PlayerName = player0Name.ToString()
                            });
                        }
                    }
                }
            }

            print("玩家信息");
            foreach (var item in playerList)
            {
                print(item.ToString() + " ");
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

        Dictionary<byte, object> dictionary = new Dictionary<byte, object>
        {
            { (byte)EMatchingType.IsMatchingGame, eMatching },
            { (byte)EMatchingType.PlayerCount, nF.matchCount },
            {(byte)EMatchingType.CurPlayerCar,PlayerController.Get.CurplayerCar }
        };
        print(nF.matchCount);
        PhotonClientConnect.PhotonPeer.OpCustom((byte)EOperationCode.MatchingGame, dictionary, true);
        print("发送");
    }
    
}
