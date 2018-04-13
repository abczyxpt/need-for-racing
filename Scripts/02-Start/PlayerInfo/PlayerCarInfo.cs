using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarInfo : MonoBehaviour {

    //赛车
    private GameObject car1;
    private GameObject car2;
    //UI
    private UIButton carChangeButton;
    private UIButton yesBuyButton;
    private UIButton noBuyButton;
    private UISprite infoSplit;
    private UILabel infoLabel;
    //判定
    private bool playerHaveCar2;
    private GameObject cube;

    private int showCount = 0;

    private void Start()
    {
        //赛车初始寻找
        car1 = this.transform.Find(CarEnum.Catamount.ToString()).gameObject;
        car2 = this.transform.Find(CarEnum.SportCar.ToString()).gameObject;
        cube = this.transform.Find(CarEnum.Cube.ToString()).gameObject;

        //赛车初始设置
        car1.SetActive(true);
        car2.SetActive(false);
        cube.SetActive(false);

        //玩家初始判定
        playerHaveCar2 = false;


        //按钮查找与添加按键
        carChangeButton = GameObject.FindGameObjectWithTag("NGUI").transform.Find("ChangeCar").GetComponent<UIButton>();
        carChangeButton.onClick.Add(new EventDelegate(ChangeCar));
        yesBuyButton = GameObject.FindGameObjectWithTag("NGUI").transform.Find("ChangeCar/InfoLabel/Yes").GetComponent<UIButton>();
        yesBuyButton.onClick.Add(new EventDelegate(YesButtonClick));
        noBuyButton = GameObject.FindGameObjectWithTag("NGUI").transform.Find("ChangeCar/InfoLabel/No").GetComponent<UIButton>();
        noBuyButton.onClick.Add(new EventDelegate(NoButtonClick));

        //UI显示信息
        //一级
        infoSplit = GameObject.FindGameObjectWithTag("NGUI").transform.Find("ChangeCar/InfoLabel").GetComponent<UISprite>();
        infoSplit.gameObject.SetActive(false);
        //二级
        infoLabel = GameObject.FindGameObjectWithTag("NGUI").transform.Find("ChangeCar/InfoLabel/Info").GetComponent<UILabel>();

        //对服务器查询
        //查询是否玩家拥有二车
        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.CarHaveRequest, new CarHanveNF { carName = car2.name });

        //获取查询
        MessageController.Get.AddEventListener((uint)ENotificationMsgType.CarHaveResponse, CarHaveResponse);
    }

    private void OnDestroy()
    {

        //获取查询
        MessageController.Get.RemoveEvent((uint)ENotificationMsgType.CarHaveResponse, CarHaveResponse);
    }

    private void CarHaveResponse(Notification notification)
    {
        showCount++;
        CarHanveNF nF = notification.parm as CarHanveNF;

        switch (nF.carName)
        {
            case "SportCar":
                playerHaveCar2 = nF.isHave;
                break;
            default:
                break;
        }

        if (showCount > 1)
        {
            if (nF.isBuy)
            {
                yesBuyButton.gameObject.SetActive(false);
                noBuyButton.gameObject.SetActive(false);
                StartCoroutine(ShowLabel("购买成功"));
                cube.SetActive(false);
            }
            else
            {
                yesBuyButton.gameObject.SetActive(false);
                noBuyButton.gameObject.SetActive(false);
                StartCoroutine(ShowLabel("购买失败"));
            }
        }

        print("收到汽车拥有消息了" + playerHaveCar2);
    }

    private IEnumerator ShowLabel(string str)
    {
        infoLabel.text = str;
        yield return new WaitForSeconds(2);

        yesBuyButton.gameObject.SetActive(true);
        noBuyButton.gameObject.SetActive(true);
        infoLabel.text = "是否购买本车";
        infoSplit.gameObject.SetActive(false);
    }

    private void NoButtonClick()
    {
        infoSplit.gameObject.SetActive(false);
    }

    private void YesButtonClick()
    {
        //处理买车功能
        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.CarHaveRequest, new CarHanveNF { carName = CarEnum.SportCar.ToString(), isBuy = true });
        }

    private void ChangeCar()
    {
        bool isActive;
        if (car1.activeSelf)
        {
            //显示二车
            if (!playerHaveCar2)
            {
                cube.SetActive(true);
            }
            isActive = false;
        }
        else
        {
            //显示一车
            if (cube.activeSelf)
            {
                cube.SetActive(false);
            }
            isActive = true;
        }

        car1.SetActive(isActive);
        car2.SetActive(!isActive);
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 10);
            if (hit.collider == null)
                return;
            if(hit.collider.gameObject.name == cube.name)
            {
                infoSplit.gameObject.SetActive(true);
            }
        }
    }
}

