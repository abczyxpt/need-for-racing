using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoLabelDisplay : MonoBehaviour {

    public UILabel nameLb;
    public UILabel coinLb;

	// Use this for initialization
	void Start () {
        nameLb.text = PlayerPrefs.GetString("PlayerName");

        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.CoinResponse, new CoinNF { count = 0 });
        MessageController.Get.AddEventListener((uint)ENotificationMsgType.CoinFromServer, CoinChange);
	}

    private void OnDestroy()
    {
        MessageController.Get.RemoveEvent((uint)ENotificationMsgType.CoinFromServer, CoinChange);
    }

    private void CoinChange(Notification notification)
    {
        CoinNF nF = notification.parm as CoinNF;
        //安全判断
        if (nF.count < 0)
        {
            throw new Exception("错误金币数量");
        }
        coinLb.text = nF.count.ToString();
        
    }

    // Update is called once per frame
    void Update () {
		
	}
}
