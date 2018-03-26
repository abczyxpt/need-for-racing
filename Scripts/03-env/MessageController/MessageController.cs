using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//声明Notification消息委托
public delegate void NotificationDelegate(Notification notification);


public class MessageController {
    
    private static MessageController instance = null;

    //消息监听器，用于保存消息并且编号
    private Dictionary<uint, NotificationDelegate> msgListeners
        = new Dictionary<uint, NotificationDelegate>();     

    private MessageController()
    {

    }

    public static MessageController Get
    {
        get { return instance ?? (instance = new MessageController()); }
    }
    
    /// <summary>
    /// 判断该键值对应的时间接收器是否真的没有
    /// </summary>
    /// <param name="msgKey"></param>
    /// <returns></returns>
    private bool IsNoMsgListener(uint msgKey)
    {
        return !msgListeners.ContainsKey(msgKey);
    }

    /// <summary>
    /// 添加消息监听器，用于接收对应的事件
    /// </summary>
    /// <param name="msgKey"></param>
    /// <param name="listener"></param>
    public void AddEventListener(uint msgKey, NotificationDelegate listener)
    {
        if (IsNoMsgListener(msgKey))
        {
            //没有对应键值的接收器，那么就创建一个对应着空的键值
            NotificationDelegate dl = null;
            msgListeners[msgKey] = dl;
        }
        
        //注册消息接收器
        msgListeners[msgKey] += listener;
    }


    /// <summary>
    /// 移除特定键值的消息
    /// </summary>
    /// <param name="msgKey"></param>
    public void RemoveEventWithKey(uint msgKey)
    {
        if (IsNoMsgListener(msgKey))
            return;
        msgListeners.Remove(msgKey);
    }


    /// <summary>
    /// 移除特定键值的特定事件
    /// </summary>
    /// <param name="eventkey"></param>
    /// <param name="listener"></param>
    public void RemoveEvent(uint eventkey, NotificationDelegate listener)
    {
        if (IsNoMsgListener(eventkey))
            return;
        msgListeners[eventkey] -= listener;

        //如果是键值对应的所有都被移除，那么就移除这个键值
        if (msgListeners[eventkey] == null)
            msgListeners.Remove(eventkey);
    }


    /// <summary>
    /// 全域广播事件
    /// </summary>
    /// <param name="msgKey"></param>
    /// <param name="eventArgs"></param>
    public void PostDispatchEvent(uint msgKey, EventArgs param)
    {
        if (IsNoMsgListener(msgKey))
            return;
        msgListeners[msgKey](new Notification(param));
    }


    /// <summary>
    /// 需要知道具体发送者的消息
    /// </summary>
    /// <param name="msgKey"></param>
    /// <param name="sender"></param>
    /// <param name="param"></param>
    public void PostDispatchEvent(uint msgKey,GameObject sender,EventArgs param)
    {
        if (IsNoMsgListener(msgKey))
            return;
        msgListeners[msgKey](new Notification(sender, param));
    }


    /// <summary>
    /// 不需要知道具体发送者情况的消息
    /// </summary>
    /// <param name="msgKey"></param>
    /// <param name="notification"></param>
    public void PostDispatchEvent(uint msgKey,Notification notification)
    {
        if (IsNoMsgListener(msgKey)) return;
        msgListeners[msgKey].Invoke(notification);
    }
}

