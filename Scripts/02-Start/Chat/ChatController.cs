using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatController : MonoBehaviour {
    
    public UITextList chatBox;
    public UIInput chatInput;

    private void Start()
    {
        MessageController.Get.AddEventListener((uint)ENotificationMsgType.ChatFromServer, ChatFromServer);
    }

    private void OnDestroy()
    {
        MessageController.Get.RemoveEvent((uint)ENotificationMsgType.ChatFromServer, ChatFromServer);
    }

    private void ChatFromServer(Notification notification)
    {
        ChatInfoNF nf = notification.parm as ChatInfoNF;
        print(nf.chatName + " " + nf.chatStr);
        ChatDisplay(nf.chatName, nf.chatStr);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            EnterInput();
        }
    }

    public void EnterInput()
    {
        if (!string.IsNullOrEmpty(chatInput.value))
        {
            ChatDisplay("[8bddfc]" + PlayerController.Get.CurPlayerName, chatInput.value);
            ChatInfoNF nf = new ChatInfoNF
            {
                chatName = PlayerController.Get.CurPlayerName,
                chatStr = chatInput.value
            };
            MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.ChatResponse, nf);
            chatInput.value = "";
        }
    }

    public void ChatDisplay(string name,string info)
    {
        chatBox.Add(name + ":[-] " + info);
    }
    
    
}
