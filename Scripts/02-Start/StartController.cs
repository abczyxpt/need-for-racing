using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour {

    private UISprite findGameInfo;
    private UILabel findGameLabel;
    private UIButton startButton;
    private UIPopupList popupList;

    public bool isMatching;        //正在寻找比赛
    public bool isShowFindLable;    //显示寻找比赛lable

    private int count;              //计数器
    private string textForAdd;      //被添加的字符串
    private string textForShow;     //用于被展示的字符串
    private string textForGameStart;//匹配结束游戏开始的时候
    private int curWantMatchPlayerCount = 1;    //当前想要匹配的对手的人数 

    private void Awake()
    {
        count = 0;
        textForAdd = ".";
    }

    private void Start()
    {
        findGameInfo = this.transform.Find("FindGameInfo").GetComponent<UISprite>();
        findGameLabel = this.transform.Find("FindGameInfo/Label").GetComponent<UILabel>();
        startButton = this.transform.Find("StartButton").GetComponent<UIButton>();
        popupList = this.transform.Find("MatchCountList").GetComponent<UIPopupList>();

        ShowFindGameInfo(false);

        startButton.onClick.Add(new EventDelegate(StartButtonOnClick));

        MessageController.Get.AddEventListener((uint)ENotificationMsgType.MacthingResponse, MatchingResponse);

    }

    private void OnDestroy()
    {
        MessageController.Get.RemoveEvent((uint)ENotificationMsgType.MacthingResponse, MatchingResponse);
    }

    private void MatchingResponse(Notification notification)
    {
        MatchingGameNF gameNF = notification.parm as MatchingGameNF;
        bool isSuccess = gameNF.isMatchingGame;
        if (isSuccess)
        {
            StartCoroutine(LoadScence());
            PlayerController.Get.SetFoePlayerName(gameNF.playerNameList);
            PlayerController.Get.SetPlayerCount(curWantMatchPlayerCount);
        }
        else
        {
            StartCoroutine(FailMatching());
        }
    }

    private IEnumerator LoadScence()
    {
        isShowFindLable = false;
        yield return new WaitForSeconds(1f);
        findGameLabel.text = "寻找比赛成功";
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene((int)ScenceEnum.PlayScence);
    }
    
    
    /// <summary>
    /// 点击开始匹配按钮之后
    /// </summary>
    private void StartButtonOnClick()
    {
        isShowFindLable = true;
        ShowFindGameInfo(true);
        ShowFindGameLabelText();
        isMatching = true;

        //判断玩家选择的游戏模式，如果是离线，那么就不对服务器通信
        //1.离线模式
        print(curWantMatchPlayerCount);
        if (curWantMatchPlayerCount == 1)
        {
            StartCoroutine(LoadScence());

            List<string> nameList = new List<string> { PlayerController.Get.CurPlayerName };
            PlayerController.Get.SetPlayerCount(curWantMatchPlayerCount);
        }
        //2.在线模式,对服务器进行通信
        else
        {
            MessageController.Get.PostDispatchEvent(
                (uint)ENotificationMsgType.MatchingGame,
                new MatchingGameNF { isMatchingGame = isMatching, msgType = ENotificationMsgType.MatchingGame, matchCount = curWantMatchPlayerCount });
            StartCoroutine(MatchingOverTime());
        }
    }


    public void PopupListChange()
    {
        switch (UIPopupList.current.value)
        {
            case "offline":
                curWantMatchPlayerCount = 1;
                break;
            case "1v1":
                curWantMatchPlayerCount = 2;
                break;
            case "1v1v1":
                curWantMatchPlayerCount = 3;
                break;
            case "1v1v1v1":
                curWantMatchPlayerCount = 4;
                break;
            default:
                curWantMatchPlayerCount = 1;
                break;
        }
    }

    /// <summary>
    /// 90秒后如果还未寻找到比赛，那么就停止寻找
    /// </summary>
    /// <returns></returns>
    private IEnumerator MatchingOverTime()
    {
        yield return new WaitForSeconds(90f);
        if (isMatching)
        {
            MessageController.Get.PostDispatchEvent(
                (uint)ENotificationMsgType.MatchingGame, 
                new MatchingGameNF { isMatchingGame = !isMatching , msgType = ENotificationMsgType.MatchingGame});
        }
        StartCoroutine(FailMatching());
    }

    private void ShowFindGameInfo(bool isHide)
    {
        findGameInfo.gameObject.SetActive(isHide);
    }
    
    private void ShowFindGameLabelText(string text = "匹配开始")
    {
        findGameLabel.text = text;
        StartCoroutine(ShowText(text));
    }

    private IEnumerator ShowText(string text)
    {
        textForShow = text;
        while (isShowFindLable)
        {
            yield return new WaitForSeconds(1);

            textForShow += textForAdd;
            count++;

            if(count == 4)
            {
                textForShow = text;
                count = 0;
            }

            findGameLabel.text = textForShow;
        }
        findGameLabel.text = textForShow;
        count = 0;
        //yield return new WaitForSeconds(1);

        ////打开下一个场景
        ////TODO: *************************
        //SceneManager.LoadScene((int)ScenceEnum.PlayScence);
    }

    private IEnumerator FailMatching()
    {
        isShowFindLable = false;
        yield return new WaitForSeconds(0.5f);
        findGameLabel.text = "寻找比赛失败";
        yield return new WaitForSeconds(2f);
        ShowFindGameInfo(false);
    }

}
