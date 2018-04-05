using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPlayerPostion : MonoBehaviour {

    public GameObject curPlayer;

	// Use this for initialization
	void Start () {
        InvokeRepeating("PostPostion", 5.5f, 0.1f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //为当前车辆的位置同步到服务器上
    private void PostPostion()
    {
        curPlayer = GenerateCar.Get.GetCurPlayer();
        SyncPostionNF nf = new SyncPostionNF
        {
            getPostion = true,
            foeBrake = curPlayer.GetComponent<MoveController>().inputBrake,
            foeVertical = curPlayer.GetComponent<MoveController>().inputVertical,
            foeHorizontal = curPlayer.GetComponent<MoveController>().inputHorizontal
        };
        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.SyncPostionResponse, nf);
    }
}
