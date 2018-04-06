using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeCountLabel : MonoBehaviour {

    private bool isStartCount = false;
    private int countNumber = 0;


	// Use this for initialization
	void Start () {
        this.GetComponent<UILabel>().text = "";
        MessageController.Get.AddEventListener((uint)ENotificationMsgType.StartCount, StartTimeCount);
        MessageController.Get.AddEventListener((uint)ENotificationMsgType.GameFinishDisplay, FinishDisplay);
        
    }

    

    private void FinishDisplay(Notification notification)
    {
        GameFinishNF nf = notification.parm as GameFinishNF;
        if (nf.isWin)
        {
            this.GetComponent<UILabel>().text = "Game win";
        }
        else
        {
            this.GetComponent<UILabel>().text = "Game lost";
        }
        StartCoroutine(LoadScence());
    }

    private IEnumerator LoadScence()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene((int)ScenceEnum.StartScence);
    }
    

    private void OnDestroy()
    {
        MessageController.Get.RemoveEvent((uint)ENotificationMsgType.StartCount, StartTimeCount);
        MessageController.Get.RemoveEvent((uint)ENotificationMsgType.GameFinishDisplay, FinishDisplay);
    }

    // Update is called once per frame
    void Update () {
        if (isStartCount)
        {
            StartCoroutine(StartCount());
        }
	}

    private IEnumerator StartCount()
    {
        isStartCount = false;
        for (; countNumber > 0; countNumber--)
        {
            this.GetComponent<UILabel>().text = countNumber.ToString();
            yield return new WaitForSeconds(1);
            countNumber--;

            if (countNumber == 0)
                break;
            this.GetComponent<UILabel>().text = countNumber.ToString();
            yield return new WaitForSeconds(1);
        }

        this.GetComponent<UILabel>().text = "Start";
        yield return new WaitForSeconds(1);
        this.GetComponent<UILabel>().text = "";

        CarControlNF carControlNF = new CarControlNF
        {
            isCanControl = true
        };
        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.CarControl, carControlNF);
    }

    public void StartTimeCount(Notification notification)
    {
        StartCountNF startCount = notification.parm as StartCountNF;
        this.countNumber = startCount.countNum;
        isStartCount = startCount.isStartCount;
        print("startTime");
    }
}