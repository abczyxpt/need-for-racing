using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveController : MonoBehaviour {

    public bool isLocalPlayer;      //是否是本地玩家
    public string userName;

    public float inputVertical;     //玩家输入
    public float inputHorizontal;
    public float inputBrake;

    public float foeVertical;       //服务器输入
    public float foeHorizontal;
    public float foeBrake;

    public int trunCount = 0;       //记录赛车的圈数

    private WheelCollider wheelColliderFL;   //前左轮
    private WheelCollider wheelColliderRL;   //后左轮
    private WheelCollider wheelColliderFR;   //前右轮
    private WheelCollider wheelColliderRR;   //后右轮

    private Transform wheelModeFL;   //前左轮
    private Transform wheelModeRL;   //后左轮
    private Transform wheelModeFR;   //前右轮
    private Transform wheelModeRR;   //后右轮

    private Transform wheelFL;      //前左轮   (用于控制轮子左右转弯)
    private Transform wheelRL;      //后左轮   (如果控制同一轮子，会发生旋转冲突)
    private Transform wheelFR;      //前右轮
    private Transform wheelRR;      //后右轮

    private bool isFinish;

    //汽车属性
    public float motorTorque;       //马力
    public float steerAngle;        //转动力度
    public float driftAngle;        //允许漂移的车头
    public float driftSpeed;        //允许漂移的车速
    private int[] gears =           //挡位
        new int[] { 0, 20, 50, 70, 100, 140 };              


    //用于查看的属性
    public int curGear;              //当前挡位
    public float[] carGears;         //所有挡位对应的转速
    public float curWheelRAvg;       //当前后轮平均的转速


    public float carSpeed;
    public float brakeTorque;
    public float faceDirection;

    public float motorTorqueRL;
    public float motorTorqueRR;

    public float brakeTorque1;


    // Use this for initialization
    void Start()
    {
        isFinish = true;
        wheelColliderFL = this.transform.Find("WheelFL/DiscBrakeFL/WheelColliderFL").GetComponent<WheelCollider>();
        wheelColliderRL = this.transform.Find("WheelRL/DiscBrakeRL/WheelColliderRL").GetComponent<WheelCollider>();
        wheelColliderFR = this.transform.Find("WheelFR/DiscBrakeFR/WheelColliderFR").GetComponent<WheelCollider>();
        wheelColliderRR = this.transform.Find("WheelRR/DiscBrakeRR/WheelColliderRR").GetComponent<WheelCollider>();
        
        wheelModeFL = this.transform.Find("WheelFL/DiscBrakeFL/WheelFL.2").transform;
        wheelModeFR = this.transform.Find("WheelFR/DiscBrakeFR/WheelFR.2").transform;
        wheelModeRL = this.transform.Find("WheelRL/DiscBrakeRL/WheelRL.2").transform;
        wheelModeRR = this.transform.Find("WheelRR/DiscBrakeRR/WheelRR.2").transform;

        wheelFL = this.transform.Find("WheelFL/DiscBrakeFL/").transform;
        wheelFR = this.transform.Find("WheelFR/DiscBrakeFR/").transform;
        wheelRL = this.transform.Find("WheelRL/DiscBrakeRL/").transform;

        //设置汽车固定属性
        CarProperty.Get.SetWheelRadius(wheelColliderFL.radius);
        
        motorTorque = CarProperty.Get.MotorTorque;
        driftAngle = CarProperty.Get.DriftAngle;
        driftSpeed = CarProperty.Get.DriftSpeed;
        brakeTorque = CarProperty.Get.BrakeTorque;

        //接受是否能控制汽车的消息
        MessageController.Get.AddEventListener((uint)ENotificationMsgType.CarControl, CarMovingControl);
        MessageController.Get.AddEventListener((uint)ENotificationMsgType.CarControlFromServer, CarControlFromServer);
    }

    private void CarControlFromServer(Notification notification)
    {
        if (isLocalPlayer)
            return;
        SyncPostionNF nF = notification.parm as SyncPostionNF;
        if(nF.ctrName == userName)
        {
            foeVertical = nF.foeVertical;
            foeHorizontal = nF.foeHorizontal;
            foeBrake = nF.foeBrake;
        }
    }

    private void OnDestroy()
    {
        MessageController.Get.RemoveEvent((uint)ENotificationMsgType.CarControl, CarMovingControl);
        MessageController.Get.RemoveEvent((uint)ENotificationMsgType.CarControlFromServer, CarControlFromServer);

    }

    // Update is called once per frame
    void Update () {


        if (!isFinish)
        {
            PlayerInputGet();


            CarControl();

            //引擎声音
            EngineSound();


            //汽车漂移
            Drift();

            //轮胎前进旋转动画
            WheelModeRotate();
            //前轮左右转弯动画
            WheelSteer();
        }

    }

    private void CarControl()
    {
        //移动
        CarMoving();
        //刹车
        BrakeTorque();
        //手刹（通过空格刹车）
        BrakeTorqueWithSpace();
    }


    /// <summary>
    /// 汽车前进与转弯控制
    /// </summary>
    private void CarMoving()
    {
        int threshold = 1;      //推力阈值
        //如果超速，就将推力设置为0
        if (CarProperty.Get.Ishypervelocity)
        {
            threshold = 0;
        }
        //后轮驱动
        motorTorqueRL =wheelColliderRL.motorTorque = inputVertical * motorTorque * threshold;
        motorTorqueRR = wheelColliderRR.motorTorque = inputVertical * motorTorque * threshold;
        //前轮转弯
        wheelColliderFL.steerAngle = inputHorizontal * steerAngle;
        wheelColliderFR.steerAngle = inputHorizontal * steerAngle;

        //汽车速度
        carSpeed = (wheelColliderFL.rpm + wheelColliderFR.rpm) / 2 * 60 * (CarProperty.Get.GetRadius() * 2 * Mathf.PI) / 1000; 
       
        //保存本地轮胎转速
        if(isLocalPlayer)
            CarProperty.Get.SetWheelRpm(wheelColliderFL.rpm, wheelColliderFR.rpm, wheelColliderRL.rpm, wheelColliderRR.rpm);

        //设置车灯，如果是倒车，就放白灯
        if (carSpeed < -0.0013)
            BrakeLight(Color.white);


        ////如果飞起来，就失去控制
        //if ((!wheelColliderFL.isGrounded && !wheelColliderFR.isGrounded) && (!wheelColliderRL.isGrounded || !wheelColliderRR.isGrounded))
        //{
        //    CarLostControl(true);
        //}

    }


    /// <summary>
    /// 汽车制动
    /// </summary>
    private void BrakeTorque()
    {
        //推力方向（如果与速度方向相反，就是判断刹车）
        faceDirection = inputVertical;
        //刹车
        if ((Mathf.Round(carSpeed) >0&& faceDirection <0) || (Mathf.Round(carSpeed) < 0 && faceDirection > 0))
        {
            wheelColliderRL.brakeTorque = brakeTorque * faceDirection;
            wheelColliderRR.brakeTorque = brakeTorque * faceDirection;

            BrakeLight(Color.red);
            BrakeSound(true);
        }
        //不刹车
        else
        {
            wheelColliderRL.brakeTorque = 0;
            wheelColliderRR.brakeTorque = 0;
        }
    }


    /// <summary>
    /// 通过空格进行刹车,手刹
    /// </summary>
    private void BrakeTorqueWithSpace()
    {

        faceDirection = Mathf.Abs(inputBrake);

        if (faceDirection != 0)
        {
            wheelColliderRL.brakeTorque = brakeTorque * faceDirection;
            wheelColliderRR.brakeTorque = brakeTorque * faceDirection;

            BrakeLight(Color.red);
            BrakeSound(true);

            if(Mathf.Round(carSpeed) == 0)
            {
                CarBrakeNf cb = new CarBrakeNf
                {
                    isBraking = false,
                    curName = this.userName
                };

                //给声音系统发送启动消息
                MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.CarBrake, cb);
            }
        }
        else
        {
            wheelColliderRL.brakeTorque = 0;
            wheelColliderRR.brakeTorque = 0;
            BrakeSound(false);
        }

    }


    /// <summary>
    /// 轮胎模型的前后转动
    /// </summary>
    private void WheelModeRotate()
    {
        wheelModeFL.Rotate(wheelColliderFL.rpm * 6 * Time.deltaTime * Vector3.right);
        wheelModeFR.Rotate(wheelColliderFR.rpm * 6 * Time.deltaTime * Vector3.right);
        wheelModeRL.Rotate(wheelColliderRL.rpm * 6 * Time.deltaTime * Vector3.right);
        wheelModeRR.Rotate(wheelColliderRR.rpm * 6 * Time.deltaTime * Vector3.right);
    }

    /// <summary>
    /// 前轮的左右转动(所有坐标都是基于汽车的本地坐标)
    /// </summary>
    private void WheelSteer()
    {
        Vector3 localEulerAngles = wheelFL.localEulerAngles;
        localEulerAngles.y = wheelColliderFL.steerAngle;        //Y为转动轴

        wheelFL.localEulerAngles = localEulerAngles;
        wheelFR.localEulerAngles = localEulerAngles;
    }


    /// <summary>
    /// 引擎的声音播放
    /// </summary>
    private void EngineSound()
    {
        curGear = Mathf.Abs(CarProperty.Get.GetCarGear());
        carGears = CarProperty.Get.GetAllGearsRpm();   // 1 (0-1) 2 (1-2) 3 (2-3)
        curWheelRAvg = CarProperty.Get.WheelRpmRAvg;
        float pitch = 0.15f + (Mathf.Abs(curWheelRAvg) - carGears[curGear - 1]) / (carGears[curGear] - carGears[curGear - 1]) * 0.8f;
        if (pitch > 2)
            pitch = 2;


        CarRunNF carRunNF = new CarRunNF
        {
            engineSoundPith = pitch,
            curName = this.userName,
        };
        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.CarRun, carRunNF);
    }

    /// <summary>
    /// 漂移控制
    /// </summary>
    private void Drift()
    {
        bool canDrift = false;
        //能够漂移
        if (carSpeed > driftSpeed && Mathf.Abs(wheelColliderFL.steerAngle) > driftAngle)
        {
            //播放刹车音效
            canDrift = true;
            BrakeLight(Color.red);
        }
        else
        {
            //关闭刹车音效
            canDrift = false;
        }

        //如果飞起来，就关闭
        if ((!wheelColliderFL.isGrounded && !wheelColliderFR.isGrounded) && (!wheelColliderRL.isGrounded || !wheelColliderRR.isGrounded))
        {
            canDrift = false;
        }
        BrakeSound(canDrift);
    }
    

    /// <summary>
    /// 控制是否能对汽车进行控制
    /// </summary>
    /// <param name="notification"></param>
    public void CarMovingControl(Notification notification)
    {
        CarControlNF carControlNF = notification.parm as CarControlNF;

        CarLostControl(!carControlNF.isCanControl);
    }

    private void CarLostControl(bool isLost)
    {
        if (isLost)
        {
            wheelColliderRL.brakeTorque = brakeTorque;
            wheelColliderRR.brakeTorque = brakeTorque;

            BrakeSound(true);
            BrakeLight(Color.red);
            isFinish = true;
        }
        else
        {
            wheelColliderRL.brakeTorque = 0;
            wheelColliderRR.brakeTorque = 0;

            BrakeSound(false);
            isFinish = false;
        }
    }



    /// <summary>
    /// 刹车声音播放
    /// </summary>
    private void BrakeSound(bool isPlay)
    {
        if (!isPlay) return;

        CarBrakeNf cb = new CarBrakeNf
        {
            isBraking = isPlay,
            curName = this.userName,
        };

        //给声音系统发送启动消息
        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.CarBrake, cb);
    }


    /// <summary>
    /// 刹车灯光播放
    /// </summary>
    /// <param name="isPlay"></param>
    private void BrakeLight(Color color)
    {
        //设置尾灯颜色
        CarLightNF carLightNF = new CarLightNF
        {
            isLigting = true,
            color = color,
            curName = this.userName,
        };
        //给灯光系统发送启动消息
        MessageController.Get.PostDispatchEvent((uint)ENotificationMsgType.CarLight, carLightNF);
    }


    public void PlayerInputGet()
    {
        if (isLocalPlayer)
        {
            inputVertical = Input.GetAxis("Vertical");
            inputHorizontal = Input.GetAxis("Horizontal");
            inputBrake = Input.GetAxis("Brake");
        }
        else
        {
            inputVertical = foeVertical;
            inputHorizontal = foeHorizontal;
            inputBrake = foeBrake;
        }
    }
}
