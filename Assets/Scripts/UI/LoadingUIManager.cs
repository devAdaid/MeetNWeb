using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingUIManager : PersistentSingleton<LoadingUIManager>
{
    public GameObject loadingUI;

    public void SetLodingUI(bool flag)
    {
        if (loadingUI != null)
            loadingUI.SetActive(flag);
    }

    public void WorkWithLoading(IEnumerator work)
    {
        StartCoroutine("WaitForWork", work);
    }

    public void MoveScene(string sceneName)
    {
        StartCoroutine("WaitForWork", LoadAsyncScene(sceneName));
    }

    IEnumerator WaitForWork(IEnumerator work)
    {
        SetLodingUI(true);
        yield return StartCoroutine(work);
        SetLodingUI(false);
    }

    IEnumerator LoadAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (loadingUI != null)
            loadingUI.SetActive(false);
        //DataManager.Instance.SetMoneyUI();
    }
}
