using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController {

    private static PlayerController instance = null;

    private int playerCount;
    public int PlayerCount { get { return playerCount; } }

    private string curPlayerName;
    public string CurPlayerName { get { return curPlayerName; } }

    private List<string> foePlayerNameList = new List<string>();
    public List<string> FoePlayerNameList { get { return foePlayerNameList; } }

    private List<string> allPlayerNameList = new List<string>();
    public List<string> AllPlayerNameList { get { return allPlayerNameList; } }

    public static PlayerController Get
    {
        get
        {
            return (instance ?? (instance = new PlayerController()));
        }
    }

    public void SetFoePlayerName(List<string> nameList)
    {
        allPlayerNameList = nameList;
        foreach (var name in nameList)
        {
            if (!foePlayerNameList.Contains(name))
            {
                if(name != curPlayerName)
                    foePlayerNameList.Add(name);
            }
        }
    }

    public void SetCurPlayerName(string name)
    {
        curPlayerName = name;
    }

    public void SetPlayerCount(int count)
    {
        playerCount = count;
    }
    
}