using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerConnect: MonoBehaviour
{
    public bool isWorking = false;
    public object result = null;
    private string serverPath;
    private string contentStr;

    public void Put(string method, string body)
    {
        if (isWorking)
            return;

        isWorking = true;
        serverPath = ServerDefine.cmsServerRoot + method;
        StartCoroutine(PutWithToken(body));
    }

    public void Post(string method, string body)
    {
        if (isWorking)
            return;

        isWorking = true;
        serverPath = ServerDefine.cmsServerRoot + method;
        StartCoroutine(PostWithToken(body));
    }

    public void GetResponse<T>(string method) where T : struct
    {
        if (isWorking)
            return;

        isWorking = true;
        serverPath = ServerDefine.cmsServerRoot + method;
        StartCoroutine(GetResponseWithToken<T>());
    }

    IEnumerator GetResponseWithToken<T>() where T : struct
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

    IEnumerator PostWithToken(string body)
    {
        var www = new UnityWebRequest(serverPath, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Authentiation", AccountInfo.token);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
        isWorking = false;
    }

    IEnumerator PutWithToken(string body)
    {
        bool isNetworkError = false;
        bool isHttpError = false;
        string response = "";

        if (body == "")
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();

            UnityWebRequest www = UnityWebRequest.Post(serverPath, formData);
            yield return www.SendWebRequest();

            isNetworkError = www.isNetworkError;
            isHttpError = www.isHttpError;

            if(isNetworkError || isHttpError)
            {
                response = www.error;
            }
            else
            {
                response = www.downloadHandler.text;
            }
        }
        else
        {
            var www = new UnityWebRequest(serverPath, "PUT");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authentiation", AccountInfo.token);
            yield return www.SendWebRequest();

            isNetworkError = www.isNetworkError;
            isHttpError = www.isHttpError;

            if (isNetworkError || isHttpError)
            {
                response = www.error;
            }
            else
            {
                response = www.downloadHandler.text;
            }
        }
        
        Debug.Log(response);

        isWorking = false;
    }
}
