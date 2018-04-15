using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameLabel : MonoBehaviour {

    private UILabel playerLabel1;
    private UILabel playerLabel2;
    private UILabel playerLabel3;
    private UILabel playerLabel4;
    public int playerCount;
    public int foeCount;

    private List<FoePlayerInfo> loseList;
    private void Awake()
    {
        loseList = new List<FoePlayerInfo>();
    }

    // Use this for initialization
    void Start () {
        
        playerCount = 0;
        FindLable();
        SetLable();

    }

    private void Update()
    {
        foeCount = FoeController.Get.FoeList.Count;
        if (FoeController.Get.FoeList.Count != playerCount)
        {
            foreach (var loseFoe in FoeController.Get.FoeLostList)
            {
                if (!loseList.Contains(loseFoe))
                {
                    loseList.Add(loseFoe);
                    RemoveFoeName(loseFoe.FoeName);
                    playerCount--;
                }
            }
        }
    }

    private void FindLable()
    {
        playerLabel1 = this.transform.Find("PlayerLabel1").GetComponent<UILabel>();
        playerLabel2 = this.transform.Find("PlayerLabel2").GetComponent<UILabel>();
        playerLabel3 = this.transform.Find("PlayerLabel3").GetComponent<UILabel>();
        playerLabel4 = this.transform.Find("PlayerLabel4").GetComponent<UILabel>();
        playerLabel1.text = playerLabel2.text = playerLabel3.text = playerLabel4.text = "";
    }

    private void SetLable()
    {
        int foePlayer = PlayerController.Get.FoePlayerNameList.Count;
        string str = "玩家 ： ";
        //设置自己名称
        playerLabel1.text = str + PlayerController.Get.CurPlayerName;
        //print(foePlayer);
        //设置敌人名称
        if(foePlayer != 0)
        {
            if (foePlayer >= 1)
            {
                playerLabel2.text = str + PlayerController.Get.FoePlayerNameList[0].PlayerName;
                playerCount++;
                if(foePlayer >= 2)
                {
                    playerLabel3.text = str + PlayerController.Get.FoePlayerNameList[1].PlayerName;
                    playerCount++;
                    if(foePlayer == 3)
                    {
                        playerLabel4.text = str + PlayerController.Get.FoePlayerNameList[2].PlayerName;
                        playerCount++;
                    }
                }
            }
        }
    }

    private void RemoveFoeName(string name)
    {
        print("name" + name + "playerLabel2.text" + playerLabel2.text + "playerLabel3.text" + playerLabel3.text + "playerLabel4.text" + playerLabel4.text);
        string str2 = playerLabel2.text;
        string str3 = playerLabel3.text;
        string str4 = playerLabel4.text;
        str2 = str2.Split(' ')[str2.Split(' ').Length - 1];
        str3 = str3.Split(' ')[str3.Split(' ').Length - 1];
        str4 = str4.Split(' ')[str4.Split(' ').Length - 1];

        if (str2== name)
        {
            playerLabel2.text = playerLabel3.text;
            playerLabel3.text = playerLabel4.text;
            playerLabel4.text = "";
        }
        else if(str3 == name)
        {
            playerLabel3.text = playerLabel4.text;
            playerLabel4.text = "";
        }
        else if(str4 == name)
        {
            playerLabel4.text = "";
        }
    }
}
