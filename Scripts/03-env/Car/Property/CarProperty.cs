using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarProperty{
    
    private static CarProperty instance = null;    //单例模式


    //默认无法更改的变量
    private float carSpeed = 0;                     //汽车速度
    private float maxSpeed = 140;
    private float minSpeed = -30;
    private float motorTorque = 3000;               //汽车马力
    private float brakeTorque = 6500;               //汽车制动
    private float steerAngle  = 30;                 //转动角度
    private float driftAngle  = 15;                 //允许漂移的车头
    private float driftSpeed  = 60;                 //允许漂移的车速


    //需要设置的变量
    private float wheelRadius = 0;                  //汽车轮胎半径
    private float wheelRpmFL  = 0;                  //前左轮的转速
    private float wheelRpmFR  = 0;                  //前右轮的转速
    private float wheelRpmRL  = 0;                  //后左轮的转速
    private float wheelRpmRR  = 0;                  //后右轮的转速
    private float wheelRpmAvg = 0;                  //轮胎的平均转速
    private float wheelRpmRAvg= 0;                  //后轮的平均转速

    private int[] gears =
        new int[] { 0, 20, 50, 70, 100, 140 };      //汽车挡位设置

    private float[] RearWheelRpmFromGrears;         //汽车挡位对应的转速

    private CarProperty() { }
	
    public static CarProperty Get
    {
        get { return instance ?? (instance = new CarProperty()); }
    }

    /// <summary>
    /// 获取汽车速度
    /// </summary>
    /// <returns></returns>
    public float CarSpeed
    {
        get { return carSpeed; }
    }

    /// <summary>
    /// 最大旋转角
    /// </summary>
    public float SteerAngle
    {
        get { return steerAngle; }
    }

    /// <summary>
    /// 汽车最大速度
    /// </summary>
    public float CarMaxSpeed
    {
        get { return maxSpeed; }
    }

    /// <summary>
    /// 获取汽车轮胎转速
    /// </summary>
    /// <returns></returns>
    public float WheelRpm
    {
        get { return wheelRpmAvg; }
    }

    /// <summary>
    /// 获取后轮的平均转速
    /// </summary>
    /// <returns></returns>
    public float WheelRpmRAvg
    {
        get { return wheelRpmRAvg; }
    }    


    /// <summary>
    /// 设置轮胎半径
    /// </summary>
    /// <param name="radius"></param>
    public void SetWheelRadius(float radius)
    {
        this.wheelRadius = radius;
    }


    /// <summary>
    /// 设置汽车轮胎转速，并且计算平均转速以及汽车的速度
    /// </summary>
    /// <param name="rpmFL">左前轮</param>
    /// <param name="rpmFR">右前轮</param>
    /// <param name="rpmRL">左后轮</param>
    /// <param name="rpmRR">右后轮</param>
    public void SetWheelRpm(float rpmFL, float rpmFR, float rpmRL, float rpmRR)
    {
        this.wheelRpmFL = rpmFL;
        this.wheelRpmFR = rpmFR;
        this.wheelRpmRL = rpmRL;
        this.wheelRpmRR = rpmRR;
        this.wheelRpmAvg = (wheelRpmRL + wheelRpmRR + wheelRpmFL + wheelRpmFR) / 4;
        this.wheelRpmRAvg = (this.wheelRpmRR + this.wheelRpmRL) / 2;
        carSpeed = (wheelRpmFL + wheelRpmFR) / 2 * 60 * (wheelRadius * 2 * Mathf.PI) / 1000;
    }



    /// <summary>
    /// 设置汽车的挡位
    /// </summary>
    /// <param name="gears"></param>
    private void SetCarGear()
    {
        RearWheelRpmFromGrears1 = new float[gears.Length];
        for (int i = 0; i < gears.Length; i++)
        {
            RearWheelRpmFromGrears1[i] = gears[i] * 1000 / (wheelRadius * 2 * Mathf.PI) / 60;
        }
    }

    /// <summary>
    /// 获取是否超速
    /// </summary>
    /// <returns></returns>
    public bool Ishypervelocity
    {
        get{ return (carSpeed > (maxSpeed - 5) || carSpeed < minSpeed); }
    }


    /// <summary>
    /// 获取制动力矩
    /// </summary>
    /// <returns></returns>
    public float BrakeTorque
    {
        get{ return brakeTorque; }
    }


    /// <summary>
    /// 获取汽车马力
    /// </summary>
    /// <returns></returns>
    public float MotorTorque
    {
        get { return motorTorque; }
    }


    /// <summary>
    /// 允许漂移车头旋转角度
    /// </summary>
    public float DriftAngle
    {
        get { return driftAngle; }
    }

    /// <summary>
    /// 允许漂移汽车速度
    /// </summary>
    public float DriftSpeed
    {
        get { return driftSpeed; }
    }

    public int[] Gears
    {
        get { return gears; }
        set { gears = value; }
        
    }

    public float[] RearWheelRpmFromGrears1
    {
        get { return RearWheelRpmFromGrears; }
        set { RearWheelRpmFromGrears = value; }
    }

    /// <summary>
    /// 获取当前的挡位
    /// </summary>
    /// <returns></returns>
    public int GetCarGear()
    {
        SetCarGear();
        for (int i = 1; i < Gears.Length; i++)
        {
            if (wheelRpmRAvg >= RearWheelRpmFromGrears1[i - 1] && wheelRpmRAvg < RearWheelRpmFromGrears1[i])
            {
                return i;
            }
            if(wheelRpmRAvg >= RearWheelRpmFromGrears1[Gears.Length - 1])
            {
                return Gears.Length - 1;
            }
        }
        return -1;
    }

    /// <summary>
    /// 获取所有的挡位对应的转速
    /// </summary>
    /// <returns></returns>
    public float[] GetAllGearsRpm()
    {
        return RearWheelRpmFromGrears1;
    }
}