using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour {

    private int curTurn = 1;
    private int totalTurn = 2;
    private int count = 0;

    private UILabel trunLabel;
    private UISlider bgmusicSlider;
    private UISlider sdeffectSlider;
    private UIButton surrenderButton;
    private UIButton cancelButton;

    private GameObject gameMenu;
    private GameObject bgmLabel;
    private GameObject sdeffectLabel;
    private GameObject finishLabel;
    private GameObject errorLabel;
    private GameObject playerCar;

    private bool isShowGameMenu;

    private void Awake()
    {
        //一级
        gameMenu = GameObject.FindGameObjectWithTag("NGUI").transform.Find("GameMenu").gameObject;
        trunLabel = GameObject.FindGameObjectWithTag("NGUI").transform.Find("TurnDiplay/TurnLabel").GetComponent<UILabel>();
        finishLabel = GameObject.FindGameObjectWithTag("NGUI").transform.Find("GameFinishLabel").gameObject;
        errorLabel = GameObject.FindGameObjectWithTag("NGUI").transform.Find("ErrorForward").gameObject;

        //二级
        bgmLabel = gameMenu.transform.Find("BackgroundMusic").gameObject;
        sdeffectLabel = gameMenu.transform.Find("SoundEffects").gameObject;
        surrenderButton = gameMenu.transform.Find("SurrenderButton").gameObject.GetComponent<UIButton>();
        cancelButton = gameMenu.transform.Find("CancelButton").gameObject.GetComponent<UIButton>();
        surrenderButton.onClick.Add(new EventDelegate(OnSurrenderButtonClick));
        cancelButton.onClick.Add(new EventDelegate(ChangeMenuActive));

        //三级
        bgmusicSlider = bgmLabel.transform.Find("BackgroundSlider").gameObject.GetComponent<UISlider>();
        sdeffectSlider = sdeffectLabel.transform.Find("SoundEffectsSlider").gameObject.GetComponent<UISlider>();
        bgmusicSlider.onChange.Add(new EventDelegate(OnBgmSliderChange));
        sdeffectSlider.onChange.Add(new EventDelegate(OnSdemSliderChange));

        gameMenu.SetActive(false);
        finishLabel.SetActive(false);
        bgmLabel.SetActive(false);
        sdeffectLabel.SetActive(false);
        errorLabel.SetActive(false);

        isShowGameMenu = false;

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

    private void OnSurrenderButtonClick()
    {
        ChangeMenuActive();
        finishLabel.SetActive(true);
    }


    private void Start()
    {

        playerCar = GameObject.FindGameObjectWithTag("Car");
        this.GetComponent<MeshRenderer>().enabled = false;
        trunLabel.text = curTurn + "/" + totalTurn;
    }


    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //如果按了ESE，那么先出现是否退出的选项
            if (gameMenu.activeSelf)
            {
                ChangeMenuActive();
            }
            else
            {
                ChangeMenuActive();
            }
        }


        //初始化背景音乐与音效的滑块显示
        if (isShowGameMenu)
        {
            isShowGameMenu = false;
            sdeffectSlider.value = playerCar.GetComponent<AudioSource>().volume;
            bgmusicSlider.value = Camera.main.GetComponent<AudioSource>().volume;
        }
    }

    public void ChangeMenuActive()
    {
        if (gameMenu.activeSelf)
        {
            gameMenu.SetActive(false);
            bgmLabel.SetActive(false);
            sdeffectLabel.SetActive(false);
        }
        else
        {
            isShowGameMenu = true;
            gameMenu.SetActive(true);
            bgmLabel.SetActive(true);
            sdeffectLabel.SetActive(true);
        }
    }

    public void ChangeFinishLabelActive()
    {
        if (finishLabel.activeSelf)
        {
            finishLabel.SetActive(false);
        }
        else
        {
            finishLabel.SetActive(true);
        }
    }


    public void OnBgmSliderChange()
    {
        if(bgmusicSlider.value>=0 && bgmusicSlider.value <=1)
            Camera.main.GetComponent<AudioSource>().volume = bgmusicSlider.value;
    }

    public void OnSdemSliderChange()
    {
        if (sdeffectSlider.value >= 0 && sdeffectSlider.value <= 1)
        {
            playerCar.GetComponent<AudioSource>().volume = sdeffectSlider.value;
            playerCar.transform.Find("WheelFL/DiscBrakeFL").GetComponent<AudioSource>().volume = sdeffectSlider.value;
        }
    }

    public void YesButtonClick()
    {
        finishLabel.SetActive(false);
        //输掉比赛
        GameEnd(false);
    }
    public void NoButtonClick()
    {
        ChangeFinishLabelActive();
    }

    /// <summary>
    /// 汽车冲过了终点线,汽车一秒后失去控制
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.tag == "Car")
        {
            ////判断是否是正常撞线
            ////1.车速为正，车头也必须为正
            //if(CarProperty.Get.CarSpeed > 0 && other.transform.forward == this.gameObject.transform.forward)
            //{
            //    TurnOneOver();
            //}
            ////2.车速为负，车头也必须为负
            //if(CarProperty.Get.CarSpeed < 0 && other.transform.forward == -this.gameObject.transform.forward)
            //{
            //    TurnOneOver();
            //}
            
            count++;
            //因为初始汽车有两个碰撞器，但是新车只有一个，所以
            if (PlayerController.Get.CurplayerCar == CarEnum.SportCar.ToString())
            {
                count++;
            }
            if (count == 2)
            {
                count = 0;
                //赛道检测器:
                if (MoniterController.Get.IsOneRoundOver)
                {
                    TurnOneOver();
                    MoniterController.Get.IsOneRoundOver = false;
                }
                else
                {
                    if (errorLabel.activeSelf)
                    {
                        ;
                    }
                    else
                        errorLabel.SetActive(true);
                }
            }
         }
    }
    
    private void TurnOneOver()
    {
       
        curTurn++;


        if (curTurn == totalTurn)
        {
            this.GetComponent<MeshRenderer>().enabled = true;
        }
        if (curTurn == totalTurn + 1)
        {
            curTurn--;
            GameEnd(true);
        }
        trunLabel.text = curTurn + "/" + totalTurn;
        
    }

    /// <summary>
    /// 游戏结束控制
    /// </summary>
    /// <param name="isWin"></param>
    public void GameEnd(bool isWin,bool isPost = true)
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
