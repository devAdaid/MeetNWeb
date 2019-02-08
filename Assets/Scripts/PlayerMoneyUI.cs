using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoneyUI : MonoBehaviour
{
    public Text steminaText;
    public Text steminaTimeText;
    public Text goldText;
    public Text cashText;
    public Text timeText;

    public void SetSteminaUI(int value, int mx)
    {
        if (steminaText != null)
            steminaText.text = value + " / " + mx;
    }

    public void SetGoldUI(int value)
    {
        if (goldText != null)
            goldText.text = value.ToString();
    }

    public void SetCashUI(int value)
    {
        if (cashText != null)
            cashText.text = value.ToString();
    }

    public void SetTimeUI()
    {
        DateTime currentTime = PlayerDataManager.Instance.currentTime;
        timeText.text = currentTime.ToString("HH:mm");

        if (PlayerDataManager.Instance.IsSteminaFull())
        {
            steminaTimeText.text = "";
        }
        else
        {
            TimeSpan diffTime = PlayerDataManager.Instance.steminaChargeTime;
            diffTime -= currentTime - PlayerDataManager.Instance.SteminaUpdateTime;
            steminaTimeText.text = String.Format("{0:00}:{1:00}", diffTime.Minutes, diffTime.Seconds);
            //Debug.Log(diffTime);
        }
    }
}
