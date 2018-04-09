using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour {

    public int curTurn = 1;
    public int totalTurn = 2;
    public int count = 0;

    public UILabel trunLabel;

    public GameObject escLabel;

    private void Awake()
    {
        escLabel.SetActive(false);
        MessageController.Get.AddEventListener((uint)ENotificationMsgType.GameFinish, GameFinishFromServer);
    }
    private void OnDestroy()
    {
        MessageController.Get.RemoveEvent((uint)ENotificationMsgType.GameFinish, GameFinishFromServer);
    }

    private void GameFinishFromServer(Notification notification)
    {
        GameFinishNF nf = notification.parm as GameFinishNF;
        print("server + " + nf.isWin);
        GameEnd(!nf.isWin, false);
    }


    private void Start()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
        trunLabel.text = curTurn + "/" + totalTurn;
    }


    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //如果按了ESE，那么先出现是否退出的选项
            if (escLabel.activeSelf)
            {
                escLabel.SetActive(false);
            }
            else
            {
                escLabel.SetActive(true);
            }
        }
        
    }

    public void YesButtonClick()
    {
        escLabel.SetActive(false);
        //输掉比赛
        GameEnd(false);
    }
    public void NoButtonClick()
    {
        escLabel.SetActive(false);
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
    /// 游戏结束控制
    /// </summary>
    /// <param name="isWin"></param>
    private void GameEnd(bool isWin,bool isPost = true)
    {
        //1.失去控制
        StartCoroutine(LostControlForOneSecond());
        //2.判断输赢

        //如果赢了，就发送胜利请求
        //如果是在线对战，就给服务器发送结束请求
        if (PlayerController.Get.AllPlayerNameList.Count != 1 && isPost)
            MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.GameFinishResponse, new GameFinishNF { isWin = isWin });
        
        //3.显示输赢
        GameFinishNF nf = new GameFinishNF
        {
            isWin = isWin
        };

        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.GameFinishDisplay, nf);

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
