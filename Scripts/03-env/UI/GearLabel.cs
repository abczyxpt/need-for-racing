using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearLabel : MonoBehaviour {

    private int carGear;
    private UILabel gearLabel;

	// Use this for initialization
	void Start () {
        gearLabel = this.GetComponent<UILabel>();
	}
	
	// Update is called once per frame
	void Update () {
        carGear = CarProperty.Get.GetCarGear();
        if(carGear == -1)
            gearLabel.text = "R";
        else
            gearLabel.text = carGear.ToString();
	}
}
