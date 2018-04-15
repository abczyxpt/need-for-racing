using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController {

    private static PlayerController instance = null;

    private int playerCount;
    public int PlayerCount { get { return playerCount; } }

    private string curPlayerName;
    public string CurPlayerName { get { return curPlayerName; } }

    private List<PlayerList> foePlayerNameList = new List<PlayerList>();
    public List<PlayerList> FoePlayerNameList { get { return foePlayerNameList; } }

    private List<PlayerList> allPlayerNameList = new List<PlayerList>();
    public List<PlayerList> AllPlayerNameList { get { return allPlayerNameList; } }

    private string curPlayerCar;
    public string CurplayerCar { get { return curPlayerCar; } }

    public static PlayerController Get
    {
        get
        {
            return (instance ?? (instance = new PlayerController()));
        }
    }

    public void SetFoePlayerName(List<PlayerList> nameList)
    {
        allPlayerNameList = nameList;
        foreach (var name in nameList)
        {
            if (!foePlayerNameList.Contains(name))
            {
                if(name.PlayerName != curPlayerName)
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

    public void SetCurPlayerCar(string carName)
    {
        curPlayerCar = carName;
    }
    
}

public class PlayerList
{
    public virtual string PlayerName { set; get; }
    public virtual string PlayerCar { get; set; }

    public override string ToString()
    {
        return this.PlayerName + " " + this.PlayerCar;
    }
}