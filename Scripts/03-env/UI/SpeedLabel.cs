using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLabel : MonoBehaviour {
    
    private float speed = 0;            //汽车的速度
    private UILabel label;              //用于显示的label

	// Use this for initialization
	void Start () {
        label = this.transform.GetComponent<UILabel>();
        label.text = "0";           //设置初始值为0
	}
	
	// Update is called once per frame
	void Update () {
        speed = CarProperty.Get.CarSpeed;
        label.text = Mathf.Abs(Mathf.Round(speed)).ToString();

	}
    
}
