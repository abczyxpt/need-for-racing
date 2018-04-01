using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class LoginRequest : ClientRequest
{

    /// <summary>
    /// 对服务器接受
    /// </summary>
    /// <param name="operationResponse"></param>
    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        EResponse eResponse = (EResponse)operationResponse.ReturnCode;

        bool isTrueResponse = false;

        if (eResponse == EResponse.True)
            isTrueResponse = true;
        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.ServerResponse, new UserInfoNF() { isTrueResponse = isTrueResponse, msgType = ENotificationMsgType.Login });

    }


    /// <summary>
    /// 对服务器发送
    /// </summary>
    /// <param name="notification"></param>
    public override void PostRequest(Notification notification)
    {
        UserInfoNF userInfoNF = notification.parm as UserInfoNF;

        //判断是否是登录操作
        if (userInfoNF.msgType != ENotificationMsgType.Login) return;

        Dictionary<byte, object> dictionary = new Dictionary<byte, object>
        {
            { (byte)EUserInfo.Username, userInfoNF.userName },
            { (byte)EUserInfo.Password, userInfoNF.password }
        };

        PhotonClientConnect.PhotonPeer.OpCustom((byte)EOperationCode.UserLogin, dictionary, true);
    }
}
