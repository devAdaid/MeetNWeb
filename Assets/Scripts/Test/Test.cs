using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public void AddMoney()
    {
        PlayerDataManager.Instance.Gold += 1000;
    }
}
