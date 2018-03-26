using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetButtonClick : MonoBehaviour
{

    public void OnRigectButtonClick()
    {
        InfomationShow.Get.ShowLabel("注册功能暂时关闭");
        //注册设定：
        //必须账号4位至8位
        //密码6位至8位

    }

    public void OnLoginButoonClick()
    {
        string account = PlayerInput.Get.AccountInput;
        int password = int.Parse(PlayerInput.Get.PasswordInput);

        if(account != "asd" || password != 123)
        {
            InfomationShow.Get.ShowLabel("账号或密码输入错误");
        }
        else
        {
            InfomationShow.Get.ShowLabel("欢迎回来，xxxx");
            StartCoroutine(WiteForPlay()); 
        }
    }
	
    private IEnumerator WiteForPlay()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene((int)ScenceEnum.StartScence);
    }
}
