using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// 客户端请求的抽象函数
/// </summary>
public abstract class ClientRequest : MonoBehaviour {
    public EOperationCode eOperationCode;   //判定操作

    public abstract void OnOperationResponse(OperationResponse operationResponse);
    public abstract void PostRequest(Notification notification);

    private void Start()
    {
        PhotonClientConnect.Get.AddRequest(this);
        MessageController.Get.AddEventListener((uint)ENotificationMsgType.RegisterAndLogin, PostRequest);
    }
    
    private void OnDestroy()
    {
        PhotonClientConnect.Get.RemoveRequest(this);
        MessageController.Get.RemoveEvent((uint)ENotificationMsgType.RegisterAndLogin, PostRequest);
    }

}
