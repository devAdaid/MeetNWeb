using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public void DiffMoney(int diff)
    {
        PlayerDataManager.Instance.Gold += diff;
    }

    public void DiffRuby(int diff)
    {
        PlayerDataManager.Instance.Cash += diff;
    }

    public void Heart()
    {
        PlayerDataManager.Instance.ConsumeHeart();
    }
}
