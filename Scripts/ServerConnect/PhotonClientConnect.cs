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

    private string udpAddress = ServerIpCfg.udpAdress;
    private string appName = ServerIpCfg.appName;

    private Dictionary<byte, object> dataDict;                          //从服务器收到的数据
    private Dictionary<EOperationCode, ClientRequest> operationDict;    //请求的相应的类别

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
        operationDict = new Dictionary<EOperationCode, ClientRequest>();
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
        dataDict = eventData.Parameters;
        switch (eventData.Code)
        {
            case (byte)EOperationCode.ConnectText:
                
                object data1;
                dataDict.TryGetValue((byte)ETextCode.One,out data1);
                object data2;
                dataDict.TryGetValue((byte)ETextCode.Two, out data2);

                print("data1 " + data1);
                print("data2 " + data2);
                break;

            default:
                break;
        }
        dataDict = new Dictionary<byte, object>();
    }


    /// <summary>
    /// 客户端收到向服务端发起请求被相应回来的结果
    /// </summary>
    /// <param name="operationResponse"></param>
    public void OnOperationResponse(OperationResponse operationResponse)
    {
        #region 测试用
        dataDict = operationResponse.Parameters;
        switch ((EOperationCode)operationResponse.OperationCode)
        {
            case EOperationCode.ConnectText:
                print("收到来自服务器的请求");
                object data1;
                object data2;
                dataDict.TryGetValue((byte)ETextCode.One, out data1);
                dataDict.TryGetValue((byte)ETextCode.Two, out data2);
                print(data1.ToString() + "\n" + data2.ToString());
                break;
                
            default:
                break;
        }

        dataDict = new Dictionary<byte, object>();
        #endregion

        ClientRequest clientRequest = null ;
        if (operationDict.TryGetValue((EOperationCode)operationResponse.OperationCode,out clientRequest))
        {
            clientRequest.OnOperationResponse(operationResponse);
        }
    }


    public void OnStatusChanged(StatusCode statusCode)
    {
        print(statusCode);
    }

    public void AddRequest(ClientRequest clientRequest)
    {
        operationDict.Add(clientRequest.eOperationCode, clientRequest);
    }

    public void RemoveRequest(ClientRequest clientRequest)
    {
        operationDict.Remove(clientRequest.eOperationCode);
    }
    
}
