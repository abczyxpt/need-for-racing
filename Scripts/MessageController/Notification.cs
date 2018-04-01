using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Notification{

    public GameObject sender;       //消息发送者
    public EventArgs parm;          //消息内容

    public Notification() { }
    public Notification(EventArgs parm) { this.parm = parm; }
    public Notification(GameObject sender, EventArgs parm) { this.sender = sender;this.parm = parm; }
    
}


//具体的消息

//汽车刹车
public class CarBrakeNf : EventArgs
{
    public bool isBraking;
}

//汽车跑
public class CarRunNF : EventArgs
{
    public float engineSoundPith;
}

//汽车灯光
public class CarLightNF : EventArgs
{
    public bool isLigting;
    public Color color;
}

//汽车控制
public class CarControlNF : EventArgs
{
    public bool isCanControl;
}

public class StartCountNF : EventArgs
{
    public bool isStartCount;
    public int countNum;
}

public class UserInfoNF : EventArgs
{
    public bool isTrueResponse;
    public string userName;
    public string password;
    public ENotificationMsgType msgType;
}
