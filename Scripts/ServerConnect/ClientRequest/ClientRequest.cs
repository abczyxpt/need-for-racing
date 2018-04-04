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
    public ENotificationMsgType msgType;
    public abstract void OnOperationResponse(OperationResponse operationResponse);
    public abstract void PostRequest(Notification notification);
    public abstract void OnEvent(EventData eventData);
    private void Start()
    {
        PhotonClientConnect.Get.AddRequest(this);
        MessageController.Get.AddEventListener((uint)this.msgType, PostRequest);
    }
    
    private void OnDestroy()
    {
        PhotonClientConnect.Get.RemoveRequest(this);
        MessageController.Get.RemoveEvent((uint)this.msgType, PostRequest);
    }

}
