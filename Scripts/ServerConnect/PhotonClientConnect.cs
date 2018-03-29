using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using System.Threading;

public class PhotonClientConnect:MonoBehaviour,IPhotonPeerListener{

    private static PhotonClientConnect instance = null;
    private PhotonPeer photonPeer;

    private void Awake()
    {
        //第一次如果为空，就设置为该对象的脚本
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        //如果instance不为空，并且不是指向本对象,删除该对象
        else if(instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }


    /// <summary>
    /// 开始的时候启动连接
    /// </summary>
    private void Start()
    {
        photonPeer = new PhotonPeer(this, ConnectionProtocol.Udp);
        Thread thread = new Thread(Connect);
        thread.Start();
    }

    private void Connect()
    {
        photonPeer.Connect("118.24.85.105:5055", "NoR");
    }


    private void Update()
    {
        //必须一直发送
        photonPeer.Service();
    }

    private void OnDestroy()
    {
        //如果关闭的时候没有断开连接
        if (photonPeer != null && photonPeer.PeerState != PeerStateValue.Disconnected)
            photonPeer.Disconnect();
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        
    }


    /// <summary>
    /// 服务器端直接向客户端发送信息
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEvent(EventData eventData)
    {
        throw new System.NotImplementedException();
    }


    /// <summary>
    /// 客户端向服务端发起请求被相应回来的结果
    /// </summary>
    /// <param name="operationResponse"></param>
    public void OnOperationResponse(OperationResponse operationResponse)
    {
        
    }


    public void OnStatusChanged(StatusCode statusCode)
    {
        print(statusCode);
    }
}
