using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCar : MonoBehaviour {

    public static GenerateCar instance = null;
    public static GenerateCar Get { get { return instance; } }
    
    public Transform postion1;
    public Transform postion2;
    public Transform postion3;
    public Transform postion4;
    public GameObject carPerfab;

    public int playerCount = 0;
    public List<string> playerList;
    private Dictionary<string, GameObject> playerDict;
    public Dictionary<string, GameObject> PlayerDict { get { return playerDict; } }

    public string curPlayerName;

    private void Awake()
    {
        instance = this;
        playerDict = new Dictionary<string, GameObject>();
    }

    // Use this for initialization
    void Start () {
        playerCount =  PlayerController.Get.PlayerCount;
        playerList = PlayerController.Get.AllPlayerNameList;
        curPlayerName = PlayerController.Get.CurPlayerName;
        GenerateCarMethod();
	}

    private void GenerateCarMethod()
    {
        //至少一个
        if(playerCount >= 1)
        {
            GenerateGo(playerList[0], postion1);
            //至少两个
            if (playerCount >= 2)
            {
                GenerateGo(playerList[1], postion2);
                //至少三个
                if (playerCount >= 3)
                {
                    GenerateGo(playerList[2], postion3);
                    //至少四个
                    if (playerCount >= 4)
                    {
                        GenerateGo(playerList[3], postion4);
                    }
                }
            }
        }
    }
    
    private void GenerateGo(string name,Transform pst)
    {
        GameObject go = GameObject.Instantiate(carPerfab, pst.position, pst.rotation);
        go.tag = "FoeCar";
        bool isLocal = false;
        //判断是否是本地
        if (name == curPlayerName)
        {
            isLocal = true;
            go.tag = "Car";
        }
        go.GetComponent<MoveController>().isLocalPlayer = isLocal;
        go.GetComponent<MoveController>().userName = name;

        playerDict.Add(go.GetComponent<MoveController>().userName, go);
    }

    // Update is called once per frame
    void Update () {
	        
	}

    public GameObject GetCurPlayer()
    {
        GameObject go;
        PlayerDict.TryGetValue(curPlayerName, out go);
        return go;
    }
}
