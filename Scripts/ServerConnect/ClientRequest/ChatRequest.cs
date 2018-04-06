using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class ChatRequest : ClientRequest
{
    public override void OnEvent(EventData eventData)
    {
        object chatName,chatInfo;
        eventData.Parameters.TryGetValue((byte)EChat.ChatName, out chatName);
        eventData.Parameters.TryGetValue((byte)EChat.ChatInfo, out chatInfo);

        ChatInfoNF nf = new ChatInfoNF
        {
            chatName = chatName as string,
            chatStr = chatInfo as string,
        };
        print(chatName + " " + chatInfo + " " + nf.chatName + " " + nf.chatStr);
        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.ChatFromServer, nf);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {

    }

    public override void PostRequest(Notification notification)
    {
        ChatInfoNF nf = notification.parm as ChatInfoNF;

        Dictionary<byte, object> dict = new Dictionary<byte, object>
        {
            {(byte)EChat.ChatName,nf.chatName },
            {(byte)EChat.ChatInfo,nf.chatStr }
        };

        print(dict + " " + nf.chatName + " " + nf.chatStr);

        PhotonClientConnect.PhotonPeer.OpCustom((byte)eOperationCode, dict, false);
    }
}
