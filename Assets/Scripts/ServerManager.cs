using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour {

    void Start()
    {
        //StartCoroutine(Upload());
    }

    public void SendAccessToken(string token)
    {
        StartCoroutine("Uproad", token);
    }

    IEnumerator Upload(string token)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();

        UnityWebRequest www = UnityWebRequest.Post("http://175.210.61.143:8080/" + token, formData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
}
