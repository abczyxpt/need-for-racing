using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoePlayerInfo{

    private string foeName = null;
    public string FoeName { get { return foeName; } }

    public FoePlayerInfo()
    {

    }

    public FoePlayerInfo(string foeName)
    {
        this.foeName = foeName;
    }

    private bool isWin = false;
    public bool IsWin { get { return isWin; } }
    
    public void SetFoeName(string name)
    {
        foeName = name;
    }
    
    public void FoeSetWin(bool isWin)
    {
        this.isWin = isWin;
    }

}
