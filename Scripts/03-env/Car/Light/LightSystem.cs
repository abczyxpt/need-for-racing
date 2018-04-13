using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 灯光控制系统，如果没收到消息，灯会自动关闭
/// </summary>
public class LightSystem : MonoBehaviour {

    private GameObject lights;
    private bool isLighting;
    private Light lightRL;
    private Light lightRR;
    
    private GameObject brakeLights;
    private GameObject tailLights;
    private GameObject frontLights;
    private GameObject reverseLights;
    private GameObject leftIndicators;
    private GameObject rightIndicators;

    //左右转弯灯操作
    private bool indicator = false;
    private bool indicatorL = false;
    private bool indicatorR = false;
    private bool lightOnL = false;
    private bool lightOnR = false;
    private bool lightOnA = false;
    private float timerL;
    private float timerL2;
    private float timerR;
    private float timerR2;

    //刹车灯
    private bool brake = false;
    private bool reverse = false;

    // Use this for initialization
    void Start () {
        InitializeLigt();
        MessageController.Get.AddEventListener((uint)ENotificationMsgType.CarLight, LightsGlowing);
        
    }

    private void InitializeLigt()
    {
        switch (PlayerController.Get.CurplayerCar)
        {
            case "Catamount":
                lights = this.transform.Find("Lights").gameObject;
                lightRL = lights.transform.Find("LightRL").GetComponent<Light>();
                lightRR = lights.transform.Find("LightRR").GetComponent<Light>();
                break;
            case "SportCar":
                //lights = this.transform.Find("Lights").gameObject;
                brakeLights = this.transform.Find("Lights/BrakeLights").gameObject;
                tailLights = this.transform.Find("Lights/TailLights").gameObject;
                frontLights = this.transform.Find("Lights/FrontLights").gameObject;
                reverseLights = this.transform.Find("Lights/ReverseLights").gameObject;
                leftIndicators = this.transform.Find("Lights/LeftIndicators").gameObject;
                rightIndicators = this.transform.Find("Lights/RightIndicators").gameObject;

                tailLights.SetActive(false);
                frontLights.SetActive(false);
                leftIndicators.SetActive(false);
                rightIndicators.SetActive(false);

                CloseLight();
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        MessageController.Get.RemoveEvent((uint)ENotificationMsgType.CarLight, LightsGlowing);
    }

    // Update is called once per frame
    void Update () {
                
        if (PlayerController.Get.CurplayerCar == "SportCar")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                FrontLigt();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                LeftLight();
            }
            if(Input.GetKeyDown(KeyCode.E))
            {
                RightLight();
            }
            if(Input.GetKeyDown(KeyCode.R))
            {
                IndicatorLight();
            }

            //右转灯操作
            if (indicatorL)
            {
                //亮灯
                if(timerL > 0f)
                {
                    timerL -= Time.deltaTime;
                    leftIndicators.SetActive(true);
                    timerL2 = 0.5f;
                }
                if (timerL <= 0f)
                {
                    leftIndicators.SetActive(false);
                    timerL2 -= Time.deltaTime;
                    if (timerL2 <= 0f) timerL = 0.5f;
                }
            }
            else
            {
                leftIndicators.SetActive(false);
            }

            //左转灯操作
            if (indicatorR)
            {
                if (timerR >= 0f)
                {
                    timerR -= Time.deltaTime;
                    rightIndicators.SetActive(true);
                    timerR2 = 0.5f;
                }
                if (timerR <= 0f)
                {
                    rightIndicators.SetActive(false);
                    timerR2 -= Time.deltaTime;
                    if (timerR2 <= 0f) timerR = 0.5f;
                }
            }
            else
            {
                rightIndicators.SetActive(false);
            }

            //刹车与倒车
            if (brake)
            {
                brake = false;
                brakeLights.SetActive(true);
            }
            else
            {
                brakeLights.SetActive(false);
            }
            if (reverse)
            {
                reverse = false;
                reverseLights.SetActive(true);
            }
            else
            {
                reverseLights.SetActive(false);
            }
        }
        else
        {
            if (lights != null)
                lights.SetActive(isLighting);
            isLighting = false;
        }
	}

    /// <summary>
    /// 开前灯
    /// </summary>
    private void FrontLigt()
    {
        if (frontLights.activeSelf)
        {
            frontLights.SetActive(false);
            tailLights.SetActive(false);
        }
        else
        {
            frontLights.SetActive(true);
            tailLights.SetActive(true);
        }
    }

    /// <summary>
    /// 左转灯
    /// </summary>
    private void LeftLight()
    {
        timerL = 0.5f;
        indicatorL = false;
        indicatorR = false;
        if(lightOnL == false)
        {
            lightOnL = true;
            lightOnR = false;
            lightOnA = false;
            if (indicatorL == false)
            {
                indicatorL = true;
                indicatorR = false;
            }
        }
        else
        {
            lightOnL = false;
            indicatorL = false;
        }
    }

    /// <summary>
    /// 右转灯
    /// </summary>
    private void RightLight()
    {
        timerR = 0.5f;
        indicatorL = false;
        indicatorR = false;
        if (lightOnR == false)
        {
            lightOnL = false;
            lightOnR = true;
            lightOnA = false;
            if (indicatorR == false)
            {
                indicatorL = false;
                indicatorR = true;
            }
        }
        else
        {
            lightOnR = false;
            indicatorR = false;
        }
    }


    /// <summary>
    /// 左右灯同时亮
    /// </summary>
    private void IndicatorLight()
    {
        timerL = 0.5f;
        timerR = 0.5f;
        indicatorL = false;
        indicatorR = false;

        if(lightOnA == false)
        {
            lightOnL = false;
            lightOnR = false;
            lightOnA = true;
            
            if(indicatorL == false && indicatorR == false)
            {
                indicatorL = true;
                indicatorR = true;
            }
        }
        else
        {
            lightOnA = false;
            indicatorL = false;
            indicatorR = false;
        }
    }


    /// <summary>
    /// 车灯发光控制,设置光的颜色
    /// </summary>
    /// <param name="notification"></param>
    private void LightsGlowing(Notification notification)
    {
        CarLightNF carLightNf = notification.parm as CarLightNF;
        //判断是否是对当前车辆进行控制
        if (carLightNf.curName != this.GetComponent<MoveController>().userName) return;
        isLighting = carLightNf.isLigting;

        
        switch (PlayerController.Get.CurplayerCar)
        {
            case "Catamount":
                lightRL.color = carLightNf.color;
                lightRR.color = carLightNf.color;
                break;
            case "SportCar":
                //如果是刹车的红灯
                if (carLightNf.color == Color.red)
                {
                    brake = true;
                }
                else if (carLightNf.color == Color.white)
                {
                    reverse = true;
                }
                break;
            default:
                break;
        }
        
    }

    private void CloseLight()
    {
        switch (PlayerController.Get.CurplayerCar)
        {
            case "SportCar":
                brakeLights.SetActive(false);
                reverseLights.SetActive(false);
                break;
        }
    }
}
