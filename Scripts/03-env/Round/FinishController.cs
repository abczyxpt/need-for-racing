using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour {

    public int curTurn = 1;
    public int totalTurn = 2;
    public int count = 0;

    public UILabel trunLabel;

    private void Awake()
    {
        MessageController.Get.AddEventListener((uint)ENotificationMsgType.GameFinish, GameFinishFromServer);
    }
    private void OnDestroy()
    {
        MessageController.Get.RemoveEvent((uint)ENotificationMsgType.GameFinish, GameFinishFromServer);
    }

    private void GameFinishFromServer(Notification notification)
    {
        GameFinishNF nf = notification.parm as GameFinishNF;

    }

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
            count++;
            if (count == 2)
            {
                count = 0;
                curTurn++;
                if(curTurn == totalTurn)
                {
                    this.GetComponent<MeshRenderer>().enabled = true;
                }
                if (curTurn == totalTurn + 1)
                {
                    GameEnd(true);
                }
                trunLabel.text = curTurn + "/" + totalTurn;
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

    }

    /// <summary>
    /// 游戏结束控制
    /// </summary>
    /// <param name="isWin"></param>
    private void GameEnd(bool isWin)
    {
        //1.失去控制
        StartCoroutine(LostControlForOneSecond());
        //2.判断输赢

        //如果赢了，就发送胜利请求
        //如果是在线对战，就给服务器发送结束请求
        if (isWin)
        {
            if (PlayerController.Get.AllPlayerNameList.Count != 1)
                MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.GameFinish, new GameFinishNF { isWin = true });
        }
        //3.显示输赢
        GameFinishNF nf = new GameFinishNF
        {
            isWin = true
        };

        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.GameFinishDisplay, nf);

    }
}
