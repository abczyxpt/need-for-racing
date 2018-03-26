using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInput : MonoBehaviour {

    private static PlayerInput instance;
    public static PlayerInput Get { get { return instance; } }

    //public string AccountInput { get { return accountInputLabel.value.ToString() ?? "-1"; } }
    public string AccountInput { get { return accountInputLabel.value.ToString() == "" ? "1" : accountInputLabel.value.ToString(); } }
    public string PasswordInput {  get { return passwordInputLabel.value.ToString() == "" ? "1" : passwordInputLabel.value.ToString(); } }

    private UIInput accountInputLabel;
    private UIInput passwordInputLabel;

    private void Awake()
    {
        instance = this;
        accountInputLabel = this.transform.GetChild(0).GetComponent<UIInput>();
        passwordInputLabel = this.transform.GetChild(1).GetComponent<UIInput>();
    }

}
