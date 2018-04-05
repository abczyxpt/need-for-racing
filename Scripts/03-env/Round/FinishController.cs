using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour {

    public int curTurn = 1;
    public int totalTurn = 2;

    public UILabel trunLabel;

    private void Start()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
        trunLabel.text = curTurn + "/" + totalTurn;
    }
    /// <summary>
    /// 汽车冲过了终点线,汽车一秒后失去控制
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.tag == "Car")
        {
            curTurn++;
            trunLabel.text = curTurn + "/" + totalTurn;
            if (curTurn == totalTurn)
            {
                this.GetComponent<MeshRenderer>().enabled = true;
                StartCoroutine(LostControlForOneSecond());
            }
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
        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.GameFinish, new GameFinishNF { isWin = true });
    }
}
