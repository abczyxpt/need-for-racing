using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using System.Threading;

public class PhotonClientConnect:MonoBehaviour,IPhotonPeerListener{

    public static PhotonClientConnect Get { get { return instance; } }
    public static PhotonPeer PhotonPeer { get{ return photonPeer; } }


    private static PhotonClientConnect instance = null;
    private static PhotonPeer photonPeer;

    private string udpAddress = "118.24.85.105:5055";
    private string appName = "NoR";

    private Dictionary<byte, object> dataDct;

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
        photonPeer.Connect(udpAddress, appName);
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
        switch (eventData.Code)
        {
            case (byte)EOperationCode.ConnectText:
                dataDct = eventData.Parameters;
                object data1;
                dataDct.TryGetValue((byte)ETextCode.One,out data1);
                object data2;
                dataDct.TryGetValue((byte)ETextCode.Two, out data2);

                print("data1 " + data1);
                print("data2 " + data2);
                break;

            default:
                break;
        }
    }


    /// <summary>
    /// 客户端向服务端发起请求被相应回来的结果
    /// </summary>
    /// <param name="operationResponse"></param>
    public void OnOperationResponse(OperationResponse operationResponse)
    {
        switch (operationResponse.OperationCode)
        {
            case (byte)EOperationCode.ConnectText:
                print("收到来自服务器的请求");
                dataDct = operationResponse.Parameters;
                object data1;
                object data2;
                dataDct.TryGetValue((byte)ETextCode.One, out data1);                
                dataDct.TryGetValue((byte)ETextCode.Two, out data2);
                print(data1.ToString() + "\n" + data2.ToString());
                break;
            default:
                break;
        }
    }


    public void OnStatusChanged(StatusCode statusCode)
    {
        print(statusCode);
    }
}
