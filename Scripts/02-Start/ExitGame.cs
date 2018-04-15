using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour {

    private UIButton exitButton;
    private UIButton cancelButton;

    private GameObject exitGame;

    private void Awake()
    {
        exitGame = this.transform.Find("ExitGame").gameObject;
        exitButton = this.transform.Find("ExitGame/ExitButton").GetComponent<UIButton>();
        cancelButton = this.transform.Find("ExitGame/CancelButton").GetComponent<UIButton>();

        exitButton.onClick.Add(new EventDelegate(OnExitButtonClick));
        cancelButton.onClick.Add(new EventDelegate(OnCancelButtonClick));
        ChangeCurActive();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeCurActive();
        }
	}

    private void OnExitButtonClick()
    {
        Application.Quit();
    }

    private void OnCancelButtonClick()
    {
        ChangeCurActive();
    }

    private void ChangeCurActive()
    {
        if (exitGame.activeSelf)
        {
            exitGame.SetActive(false);
        }
        else
        {
            exitGame.SetActive(true);
        }
    }
}
