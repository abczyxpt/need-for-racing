using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPlayerPostion : MonoBehaviour {

    public GameObject curPlayer;

	// Use this for initialization
	void Start () {
        InvokeRepeating("PostPostion", 2.0f, 0.3f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //为当前车辆的位置同步到服务器上
    private void PostPostion()
    {
        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.SyncPostionResponse, new SyncPostionNF { getPostion = true, postion = curPlayer.transform.position });
    }
}
