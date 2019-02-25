using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class MyAccountEvent: UnityEvent<string, string>
{

}

public class NormalAccountUI : MonoBehaviour
{
    public InputField idInput;
    public InputField pwdInput;
    public MyAccountEvent joinEvent;
    public MyAccountEvent loginEvent;

    public void Join()
    {
        string idStr = idInput.text;
        string pwdStr = pwdInput.text;
        joinEvent.Invoke(idStr, pwdStr);
    }

    public void Login()
    {
        string idStr = idInput.text;
        string pwdStr = pwdInput.text;
        loginEvent.Invoke(idStr, pwdStr);
    }
}
