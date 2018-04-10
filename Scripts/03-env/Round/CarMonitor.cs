﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMonitor : MonoBehaviour {

    private int count = 0;
    private List<string> str = new List<string>();

    public int moniterNumber;

    private void Start()
    {
        if (this.gameObject.name == "GameFinish")
        {
            moniterNumber = this.transform.parent.Find("MoniterGroup").childCount + 1;
        }
        else
        {
            string[] strs = this.gameObject.name.Split('_');
            moniterNumber = int.Parse(strs[1]);
        }

        MoniterController.Get.AddMoniter(this.gameObject.GetComponent<CarMonitor>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag == "Car")
        {
            count++;
            if(count == 2)
            {
                count = 0;
                MoniterController.Get.CarInRightForward(this.gameObject.GetComponent<CarMonitor>());
            }
        }
    }
}
