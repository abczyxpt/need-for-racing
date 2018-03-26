using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfomationShow : MonoBehaviour {

    private static InfomationShow instance;

    public static InfomationShow Get { get { return instance; } }

    private UILabel infoLabel;
    
    private bool isHideLabel;

    private void Awake()
    {
        infoLabel = this.transform.Find("Label").GetComponent<UILabel>();

        HideLabel();
        isHideLabel = false;
        instance = this;
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (isHideLabel)
        {
            HideLabel();
            isHideLabel = false;
        }
	}


    public void HideLabel()
    {
        this.GetComponent<UISprite>().alpha = 0;
    }

    public void ShowLabel(string text)
    {
        infoLabel.text = "";
        this.GetComponent<UISprite>().alpha = 1;
        infoLabel.text = text;
        StartCoroutine(HideLabeTime());
    }

    private IEnumerator HideLabeTime()
    {
        yield return new WaitForSeconds(3);
        isHideLabel = true;
    }
}