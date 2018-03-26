using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCountLabel : MonoBehaviour {

    private bool isStartCount;
    private int countNumber; 


	// Use this for initialization
	void Start () {
        this.GetComponent<UILabel>().text = "";
        MessageController.Get.AddEventListener((uint)ENotificationMsgType.StartCount, StartTimeCount);
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