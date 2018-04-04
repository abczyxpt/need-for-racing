using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetButtonClick : MonoBehaviour
{
    private void Start()
    {
        MessageController.Get.AddEventListener((uint)ENotificationMsgType.ServerResponse, OperationResponse);
    }

    private void OnDestroy()
    {
        MessageController.Get.RemoveEvent((uint)ENotificationMsgType.ServerResponse, OperationResponse);
    }


    private void OperationResponse(Notification notification)
    {
        
        UserInfoNF userInfoNF = notification.parm as UserInfoNF;
        
        string str = "";
        switch (userInfoNF.msgType)
        {
            case ENotificationMsgType.Login:
                str = "登录";
                break;
            case ENotificationMsgType.Register:
                str = "注册";
                break;
        }

        if (userInfoNF.isTrueResponse)
        {
            if (str == "登录")
            {
                StartCoroutine(WiteForPlay());
                str += "欢迎回来," + PlayerInput.Get.AccountInput;
                //保存用户账号
                PlayerPrefs.SetString("PlayerName", PlayerInput.Get.AccountInput);
                //保存用户名称
                PlayerController.Get.SetCurPlayerName(PlayerInput.Get.AccountInput);
            }
            InfomationShow.Get.ShowLabel("成功" + str);
        }
        else
        {
            InfomationShow.Get.ShowLabel("失败" + str);
        }
    }


    /// <summary>
    /// 注册按钮
    /// </summary>
    public void OnRigectButtonClick()
    {
        //注册设定：
        //必须账号4位至8位
        //密码6位至8位
        string account = PlayerInput.Get.AccountInput;
        string password = PlayerInput.Get.PasswordInput;

        if (CorrectInput(account, password))
        {
            UserInfoNF userInfoNF = new UserInfoNF { userName = account, password = password, msgType = ENotificationMsgType.Register };
            MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.ClientRequest, userInfoNF);

        }

    }


    /// <summary>
    /// 登录按钮
    /// </summary>
    public void OnLoginButoonClick()
    {
        string account = PlayerInput.Get.AccountInput;
        string password = PlayerInput.Get.PasswordInput;

        print("login button click + accout = " + account + " psw = " + password);

        if (CorrectInput(account, password))
        {
            UserInfoNF userInfoNF = new UserInfoNF { userName = account, password = password, msgType = ENotificationMsgType.Login };
            MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.ClientRequest, userInfoNF);
        }
        
    }
	
    private IEnumerator WiteForPlay()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene((int)ScenceEnum.StartScence);
    }



    /// <summary>
    /// 判断是否是正确的输入
    /// </summary>
    /// <param name="account"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    private bool CorrectInput(string account,string password)
    {
        if (account.Length >= 4 && account.Length <= 8)
        {
            if (password.Length >= 6 && password.Length <= 8)
            {
                return true;
            }
            else
            {
                InfomationShow.Get.ShowLabel("请将密码控制在6-8位之内");
                return false;
            }
        }
        else
        {
            InfomationShow.Get.ShowLabel("请将账号控制在4-8位之内");
            return false;
        }
    }
}
