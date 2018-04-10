using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class CoinRequest :ClientRequest {


    public override void OnEvent(EventData eventData)
    {
       
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        Dictionary<byte, object> dict = operationResponse.Parameters;
        
    }

    public override void PostRequest(Notification notification)
    {
        CoinNF nf = notification.parm as CoinNF;

        Dictionary<byte, object> dict = new Dictionary<byte, object>
        {
            {(byte)ECoin.Num,nf.count },
        };

        PhotonClientConnect.PhotonPeer.OpCustom((byte)eOperationCode, dict, true);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
