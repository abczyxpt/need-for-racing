using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FoeController : MonoBehaviour {

    private static FoeController instance = null;
    public static FoeController Get { get { return instance; } }

    private List<FoePlayerInfo> foeList;
    public List<FoePlayerInfo> FoeList { get { return foeList; } }

    private List<FoePlayerInfo> foeLostList;
    public List<FoePlayerInfo> FoeLostList { get { return foeLostList; } }
    private void Awake()
    {
        instance = this;
        foeList = new List<FoePlayerInfo>();
        foeLostList = new List<FoePlayerInfo>();
    }

    // Use this for initialization
    void Start () {
        MessageController.Get.AddEventListener((uint)ENotificationMsgType.FoeSurrender, FoeSurrender);
	}

    private void OnDestroy()
    {
        MessageController.Get.RemoveEvent((uint)ENotificationMsgType.FoeSurrender, FoeSurrender);
    }

    private void FoeSurrender(Notification notification)
    {
        SurrenderNF nF = notification.parm as SurrenderNF;

        FoePlayerInfo foePlayer = null;

        foreach (FoePlayerInfo foe in foeList)
        {
            if(foe.FoeName == nF.foeName)
            {
                foePlayer = foe;
                break;
            }
        }
        
        foeList.Remove(foePlayer as FoePlayerInfo);
        foeLostList.Add(foePlayer as FoePlayerInfo);

        print("比赛情况" + foeList.Count);

        //如果敌人为0，那么就结束游戏,赢得这次比赛
        if(foeList.Count == 0)
        {
            GameObject finishCtr = this.transform.parent.Find("GameFinish").gameObject;
            finishCtr.GetComponent<FinishController>().GameEnd(true, true);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void AddFoe(FoePlayerInfo foe)
    {
        if (foeList.Contains(foe))
            return;
        foeList.Add(foe);
    }

    
}
