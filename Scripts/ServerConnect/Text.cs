using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text : MonoBehaviour {
    	
	// Update is called once per frame
	void Update () {
        
	}

    private void SendRequest()
    {
        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        dictionary.Add((byte)ETextCode.One, "abczyx2");
        dictionary.Add((byte)ETextCode.Two, "123");


        PhotonClientConnect.PhotonPeer.OpCustom((byte)EOperationCode.ConnectText, dictionary, true);
    }
}
