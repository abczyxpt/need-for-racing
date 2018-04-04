using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartConrtoller : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.StartCount, new StartCountNF { countNum = 5, isStartCount = true });
        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.SyncPlayerResponse, new SyncPlayerNF { isFind = false });
	}
}
