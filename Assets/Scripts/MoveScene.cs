using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    public void MoveTo(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void CheckTokenAndMove(string sceneName)
    {
        if(AccountInfo.token == null)
        {
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    public void SetPlayerDataManager()
    {
        PlayerDataManager.CreateInstance();
    }
}
