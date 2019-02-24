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
    public MyAccountEvent uiEvent;

    public void CallBack()
    {
        string idStr = idInput.text;
        string pwdStr = pwdInput.text;
        uiEvent.Invoke(idStr, pwdStr);
    }

}
