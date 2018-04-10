using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoLabelDisplay : MonoBehaviour {

    public UILabel nameLb;
    public UILabel coinLb;

	// Use this for initialization
	void Start () {
        nameLb.text = PlayerPrefs.GetString("PlayerName");
        MessageController.Get.PostDispatchEvent((byte)ENotificationMsgType.CoinResponse, new CoinNF { count = 0 });
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
