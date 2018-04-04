using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class RegisterRequest : ClientRequest
{
    
    public override void PostRequest(Notification notification)
    {
        UserInfoNF serverResponseNF = notification.parm as UserInfoNF;

        //判断是否是应该执行注册
        if (serverResponseNF.msgType != ENotificationMsgType.Register) return;

        Dictionary<byte, object> dictionary = new Dictionary<byte, object>
        {
            { (byte)EUserInfo.Username, serverResponseNF.userName },
            { (byte)EUserInfo.Password, serverResponseNF.password }
        };
        
        PhotonClientConnect.PhotonPeer.OpCustom((byte)EOperationCode.UserRegister, dictionary, true);
    }


    /// <summary>
    /// 客户端收到服务器相应的请求
    /// </summary>
    /// <param name="operationResponse"></param>
    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        EResponse eResponse = (EResponse)operationResponse.ReturnCode;
        
        bool isTrueResponse = false;

        if (eResponse == EResponse.True)
            isTrueResponse = true;

        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.ServerResponse, new UserInfoNF() { isTrueResponse = isTrueResponse,msgType = ENotificationMsgType.Register });
       
    }

    public override void OnEvent(EventData eventData)
    {

    }
}
