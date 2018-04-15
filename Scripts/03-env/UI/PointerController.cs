using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour {

    public float minRotation;      //速度为最小偏移角度
    public float maxRotation;      //速度为最大偏移角度

    public float currentZRotation;  //当前的旋转角度

    // Use this for initialization
    void Start() {
        minRotation = currentZRotation = this.transform.eulerAngles.z;
        this.transform.eulerAngles = new Vector3(0, 0, minRotation);
        maxRotation = minRotation - 270;
    }

    // Update is called once per frame
    void Update() {
        float carSpeed = Mathf.Abs(CarProperty.Get.CarSpeed);

        if (carSpeed <= 140 && carSpeed >=0)
        {
            currentZRotation = minRotation - carSpeed * (270 / 140f);
            this.transform.eulerAngles = new Vector3(0, 0, currentZRotation);
        }
    }
}
