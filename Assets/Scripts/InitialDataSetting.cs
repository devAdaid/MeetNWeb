using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialDataSetting : MonoBehaviour
{
    private void Start()
    {
        //FirstSet();
        /*
        if (!PlayerPrefs.HasKey("FirstRun"))
        {
            PlayerPrefs.SetInt("FirstRun", 0);
            PlayerPrefs.Save();
        }
        */
    }

    private void FirstSet()
    {
        PlayerPrefs.SetInt("Stemina", 3);
        PlayerPrefs.SetString("SteminaTime", DateTime.Now.ToString());
        PlayerPrefs.SetInt("Gold", 1000);
        PlayerPrefs.SetInt("Cash", 500);
        PlayerPrefs.Save();
    }
}
