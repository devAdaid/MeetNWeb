using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UserData
{
    public int uid;
    public int usernum;
    public string nickname;
    public string photo;
    public int pushonoff;
    public int gold;
    public int ruby;
    public int heart;
    public int reset;
    public string charge_at;

    public override string ToString()
    {
        return "gold: " + gold;
    }
}

public class UserDataManager : PersistentSingleton<UserDataManager>
{
    public UserData userData = new UserData();

    protected override void Initialize()
    {
        base.Initialize();
        GeneralServerManager.Instance.GetResponse<UserData>(userData, "/detail/");
        StartCoroutine("WaitForServer");
    }

    IEnumerator WaitForServer()
    {
        yield return new WaitWhile(() => !GeneralServerManager.Instance.isWorking);
        userData = (UserData)GeneralServerManager.Instance.result;
        Debug.Log(userData.gold);
    }
}
