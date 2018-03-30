using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text : MonoBehaviour {
    	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendRequest();
        }
	}

    private void SendRequest()
    {
        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        dictionary.Add((byte)ETextCode.One, "abczyx");
        dictionary.Add((byte)ETextCode.Two, "你好");


        PhotonClientConnect.PhotonPeer.OpCustom((byte)EOperationCode.ConnectText, dictionary, true);
    }
}
