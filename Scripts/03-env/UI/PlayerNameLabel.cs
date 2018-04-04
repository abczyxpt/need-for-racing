using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameLabel : MonoBehaviour {

    private UILabel playerLabel1;
    private UILabel playerLabel2;
    private UILabel playerLabel3;
    private UILabel playerLabel4;
    

    // Use this for initialization
    void Start () {

        FindLable();
        SetLable();

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
        string str = "玩家 ：";
        //设置自己名称
        playerLabel1.text = str + PlayerController.Get.CurPlayerName;
        print(foePlayer);
        //设置敌人名称
        if(foePlayer != 0)
        {
            if (foePlayer >= 1)
            {
                playerLabel2.text = str + PlayerController.Get.FoePlayerNameList[0];
                if(foePlayer >= 2)
                {
                    playerLabel3.text = str + PlayerController.Get.FoePlayerNameList[1];
                    if(foePlayer == 3)
                    {
                        playerLabel4.text = str + PlayerController.Get.FoePlayerNameList[2];
                    }
                }
            }
        }

    }
}
