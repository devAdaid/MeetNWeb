using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerResponse<T>
{
    public string statusCode;
    public string responseMessage;
    public T responseData;
}

public class GeneralServerManager:PersistentSingleton<GeneralServerManager>
{
    private string serverPath;
    public bool isWorking = false;
    public object result = null;

    public void GetResponse<T>(T response, string path) where T : struct
    {
        if (isWorking)
            return;

        isWorking = true;
        serverPath = ServerDefine.cmsServerRoot + path;
        StartCoroutine(UploadWithToken<T>());
    }

    IEnumerator UploadWithToken<T>() where T : struct
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();

        UnityWebRequest www = UnityWebRequest.Get(serverPath);

        //AccountInfo.token = "Bearer eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiI0Iiwicm9sZXMiOiJST0xFX0FETUlOIiwiaWF0IjoxNTUxMDcxNzk5LCJleHAiOjE1NTEzMzA5OTl9.c29CwZMrgfTKly3Ha8JlqiGy9TYe18SuA9X9knET36A";
        www.SetRequestHeader("Authentiation", AccountInfo.token);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            ServerResponse<T> response = new ServerResponse<T>();
            JsonUtility.FromJsonOverwrite(www.downloadHandler.text, response);
            result = response.responseData;
        }

        isWorking = false;
    }

    private void SetInfo<T>(T data)
    {

    }
}
