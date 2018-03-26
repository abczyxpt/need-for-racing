using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour {

    private UISprite findGameInfo;
    private UILabel findGameLabel;
    private UIButton startButton;

    public bool isFindGame;        //找到匹配对象

    private int count;              //计数器
    private string textForAdd;      //被添加的字符串
    private string textForShow;     //用于被展示的字符串
    private string textForGameStart;//匹配结束游戏开始的时候


    private void Awake()
    {
        isFindGame = false;
        count = 0;
        textForAdd = ".";
    }

    private void Start()
    {
        findGameInfo = this.transform.Find("FindGameInfo").GetComponent<UISprite>();
        findGameLabel = this.transform.Find("FindGameInfo/Label").GetComponent<UILabel>();
        startButton = this.transform.Find("StartButton").GetComponent<UIButton>();

        HideFindGameInfo(false);

        startButton.onClick.Add(new EventDelegate(StartButtonOnClick));
    }

    private void Update()
    {
        //测试用，之后得删除
        TextForNextScence();
    }

    private void TextForNextScence()
    {
        StartCoroutine(TextForNext());
    }

    private IEnumerator TextForNext()
    {
        yield return new WaitForSeconds(5);
        isFindGame = true;
    }

    private void StartButtonOnClick()
    {
        HideFindGameInfo(true);
        ShowFindGameLabelText();
    }

    private void HideFindGameInfo(bool isHide)
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
        while (!isFindGame)
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
        yield return new WaitForSeconds(1);

        //打开下一个场景
        //TODO: *************************
        SceneManager.LoadScene((int)ScenceEnum.PlayScence);
    }

}
