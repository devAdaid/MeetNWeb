using System.Text;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleSendData
{
    public string client_id;
    public string client_secret;
    public string grant_type;
    public string code;
}

public class GoogleResponseData
{
    public string access_token;
    public string token_type;
    public string expires_in;
    public string refresh_token;
}

public delegate void MyDelegate(string str);


public class ServerManager : MonoBehaviour
{
    public bool isTest = false;
    public AccountManager account;
    public Text log;
    public string googleAccessToken;
    private List<string> messages = new List<string>();
    private MyDelegate logFunc;

    void Start()
    {
        //StartCoroutine(Upload());
        if (isTest)
            logFunc = AddStatusText;
        else
            logFunc = account.AddStatusText;
    }

    public void SendFacebookToken(string token)
    {
        logFunc("페북 토큰 전송: " + token);
        StartCoroutine("Upload", "http://175.210.61.143:8080/social/login/facebook/" + token);
    }

    public void SendGoogleToken(string token)
    {
        logFunc("구글 토큰 전송: " + token);
        //AddStatusText("구글 토큰 전송: " + token);
        StartCoroutine("GetFromGoogle", token);
    }

    IEnumerator Upload(string path)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();

        UnityWebRequest www = UnityWebRequest.Post(path, formData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            logFunc(www.error);
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            logFunc("Response:" + www.downloadHandler.text);
            //AddStatusText("Response:" + www.downloadHandler.text);
        }
    }

    IEnumerator GetFromGoogle(string code)
    {
        WWWForm form = new WWWForm();
        form.AddField("client_id", "687830318986-7rnrk2f13m0h1jpsp1mm6logq0ur8lev.apps.googleusercontent.com");
        form.AddField("client_secret", "kPtrt3pxSygCSr6uOge0SjsX");
        form.AddField("grant_type", "authorization_code");
        form.AddField("code", code);


        UnityWebRequest www = UnityWebRequest.Post("https://www.googleapis.com/oauth2/v4/token", form);
        
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            logFunc(www.error);
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            logFunc("G Response:" + www.downloadHandler.text);
            //AddStatusText("G Response:" + www.downloadHandler.text);
            googleAccessToken = JsonUtility.FromJson<GoogleResponseData>(www.downloadHandler.text).access_token;

            yield return Upload("http://175.210.61.143:8080/social/login/google/" + googleAccessToken);
        }
    }


    void AddStatusText(string text)
    {
        if (messages.Count == 8)
        {
            messages.RemoveAt(0);
        }
        messages.Add(text);
        string txt = "";
        foreach (string s in messages)
        {
            txt += "\n" + s;
        }
        log.text = txt;
    }
}
