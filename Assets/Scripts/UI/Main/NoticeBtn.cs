using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeBtn : MonoBehaviour
{
    public Text titleText;

    public void SetTitle(string title)
    {
        titleText.text = title;
    }
}
