using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public InputField nameInput;
    public ServerConnect server;

    private void OnEnable()
    {
        nameInput.text = PlayerDataManager.Instance.NickName;
    }

    public void SetName()
    {
        PlayerDataManager.Instance.NickName = nameInput.text;
    }
}
