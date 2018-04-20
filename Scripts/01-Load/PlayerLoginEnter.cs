using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerLoginEnter : MonoBehaviour {

    public GetButtonClick getButton;
    
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            getButton.OnLoginButoonClick();
        }	
	}
}
