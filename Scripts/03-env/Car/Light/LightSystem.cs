using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 灯光控制系统，如果没收到消息，灯会自动关闭
/// </summary>
public class LightSystem : MonoBehaviour {

    private GameObject lights;
    private bool isLighting;
    private Light lightrL;
    private Light lightRR;

    // Use this for initialization
    void Start () {
        lights = this.transform.Find("Lights").gameObject;
        lightrL = lights.transform.Find("LightRL").GetComponent<Light>();
        lightRR = lights.transform.Find("LightRR").GetComponent<Light>();
        MessageController.Get.AddEventListener((uint)ENotificationMsgType.CarLight, LightsGlowing);
        

    }

    private void OnDestroy()
    {
        MessageController.Get.RemoveEvent((uint)ENotificationMsgType.CarLight, LightsGlowing);
    }

    // Update is called once per frame
    void Update () {

        lights.SetActive(isLighting);
        isLighting = false;
	}



    /// <summary>
    /// 车灯发光控制,设置光的颜色
    /// </summary>
    /// <param name="notification"></param>
    private void LightsGlowing(Notification notification)
    {
        CarLightNF carLightNf = notification.parm as CarLightNF;
        if (carLightNf.curName != this.GetComponent<MoveController>().userName) return;
        isLighting = carLightNf.isLigting;
        lightrL.color = carLightNf.color;
        lightRR.color = carLightNf.color;
    }
}
