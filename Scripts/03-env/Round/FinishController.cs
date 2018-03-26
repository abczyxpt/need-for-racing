using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour {


    /// <summary>
    /// 汽车冲过了终点线,汽车一秒后失去控制
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.tag == "Car")
        {
            StartCoroutine(LostControlForOneSecond());
         }
    }

    /// <summary>
    /// 一秒后，发送一个汽车失去控制消息
    /// </summary>
    /// <returns></returns>
    private IEnumerator LostControlForOneSecond()
    {
        yield return new WaitForSeconds(1);
        CarControlNF carControlNF = new CarControlNF
        {
            isCanControl = false
        };

        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.CarControl, carControlNF);

    }
}
