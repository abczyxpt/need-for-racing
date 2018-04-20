using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour {

    private AudioSource engineAudioSource;  //引擎声音
    private AudioSource brakeAudioSource;   //刹车声音

    private bool isBraking;                 //判断是否在刹车状态
    private bool isRunning;                 //判断是否在奔跑状态
    

    // Use this for initialization
    void Start() {

        engineAudioSource = this.transform.GetComponent<AudioSource>();
        
        brakeAudioSource = this.transform.Find("WheelFL/DiscBrakeFL").GetComponent<AudioSource>();

        MessageController.Get.AddEventListener((uint)ENotificationMsgType.CarBrake, BrakingSoundPlay);
        MessageController.Get.AddEventListener((uint)ENotificationMsgType.CarRun, EngineSoundPlay);
    }

    private void Update()
    {
        
        //刹车音效
        brakeAudioSource.enabled = isBraking;
        isBraking = false;
       
    }

    private void OnDestroy()
    {
        MessageController.Get.RemoveEvent((uint)ENotificationMsgType.CarBrake, BrakingSoundPlay);
        MessageController.Get.RemoveEvent((uint)ENotificationMsgType.CarRun, EngineSoundPlay);
    }

    /// <summary>
    /// 刹车声音播放委托(方式)
    /// </summary>
    /// <param name="notification"></param>
    public void BrakingSoundPlay(Notification notification)
    {
        CarBrakeNf carBrakeNf = notification.parm as CarBrakeNf;
        if (carBrakeNf.curName != this.GetComponent<MoveController>().userName) return;
        isBraking = carBrakeNf.isBraking;
    }


    /// <summary>
    /// 引擎声音播放
    /// </summary>
    /// <param name="notification"></param>
    public void EngineSoundPlay(Notification notification)
    {
        CarRunNF carRunNF = notification.parm as CarRunNF;
        if (carRunNF.curName != this.GetComponent<MoveController>().userName) return;
        engineAudioSource.pitch = carRunNF.engineSoundPith;
    }
    
}