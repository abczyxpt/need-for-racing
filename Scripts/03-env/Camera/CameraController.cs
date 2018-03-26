using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private float hight = 3;        //相机高度（Y轴）
    private float distance = 6;     //相机距离（Z轴）
    private float slipSpeed = 1;    //相机滑动速度

    private Transform targetCar;    //汽车位置

    private Vector3 targetForward;  //目标朝向
    private Vector3 currentForward; //当前朝向

	// Use this for initialization
	void Start () {
        targetCar = GameObject.FindGameObjectWithTag("Car").transform;

	}
	
	// Update is called once per frame
	void Update () {
        targetForward = targetCar.forward;
        targetForward.y = 0;            //Y轴不变

        currentForward = this.transform.forward;
        currentForward.y = 0;

        // 朝向变化 = 当前朝向的标准值 目标朝向的标准值 根据移动速度 线性插值运算
        Vector3 forward = Vector3.Lerp(currentForward.normalized, targetForward.normalized, slipSpeed * Time.deltaTime);

        Vector3 targetPos = targetCar.position + Vector3.up * hight - forward * distance;
        this.transform.position = targetPos;

        this.transform.LookAt(targetCar);
	}
}
