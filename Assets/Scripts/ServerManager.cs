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

public class TokenData
{
    public int userNum;
    public string token;
}

public delegate void MyDelegate(string str);

public class ServerManager : MonoBehaviour
{
    //public string ServerDefine.authServerRoot;
    public bool isTest = false;
    public AccountManager account;
    public GameObject confirmUI;
    public GameObject notconfirmUI;
    public Text log;
    public string googleAccessToken;
    private List<string> messages = new List<string>();
    private MyDelegate logFunc;

    private string idStr;
    private string pwdStr;
    public string userID;
    private AccountType loginType = AccountType.None;

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
        loginType = AccountType.Facebook;
        StartCoroutine("EmptyBodyUpload", ServerDefine.authServerRoot + "/social/login/facebook/" + token);
    }

    public void SendGoogleToken(string token)
    {
        logFunc("구글 토큰 전송: " + token);
        loginType = AccountType.Google;
        //AddStatusText("구글 토큰 전송: " + token);
        StartCoroutine("GetFromGoogle", token);
    }

    public void SendGuestJoin()
    {
        logFunc("게스트 가입");
        StartCoroutine("EmptyBodyUpload", ServerDefine.authServerRoot + "/join/guest");
    }

    public void SendGuestLogin(int userNum)
    {
        logFunc("게스트 로그인: " + userNum);
        loginType = AccountType.Guest;
        StartCoroutine("GuestUpload", userNum);
    }

    public void SendNormalLogin(string id, string pwd)
    {
        idStr = id;
        pwdStr = pwd;
        StartCoroutine("NormalAccount", "login/account");
    }

    public void SendNormalJoin(string id, string pwd)
    {
        idStr = id;
        pwdStr = pwd;
        StartCoroutine("NormalAccount", "join/account");
    }

    IEnumerator EmptyBodyUpload(string path)
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

            if(path == ServerDefine.authServerRoot + "/join/guest")
            {
                int userNum = 0;
                int.TryParse(www.downloadHandler.text, out userNum);
                PlayerPrefs.SetInt("GuestID", userNum);
                PlayerPrefs.Save();
                confirmUI.SetActive(true);
            }
            else
            {
                SetToken(www.downloadHandler.text);
                AccountInfo.PlayerAccountType = loginType;
                AccountInfo.userId = userID;
            }
            account.SetButton();
            //AddStatusText("Response:" + www.downloadHandler.text);
            //토큰 설정
        }
    }

    IEnumerator GuestUpload(int userNum)
    {
        string body = "{\"guest_id\": \"" + userNum + "\"}";
        Debug.Log(body);

        var www = new UnityWebRequest(ServerDefine.authServerRoot + "/login/guest", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        //UnityWebRequest www = UnityWebRequest.Post(path, formData);
        //yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            logFunc(www.error);
            Debug.Log(www.error);
        }
        else
        {


            Debug.Log("Form upload complete!");
            logFunc("Response:" + www.downloadHandler.text);


            TokenData tokenData = JsonUtility.FromJson<TokenData>(www.downloadHandler.text);
            SetToken(tokenData.token);
            AccountInfo.PlayerAccountType = AccountType.Guest;
            AccountInfo.userId = userID;
            account.SetButton();
            //AddStatusText("Response:" + www.downloadHandler.text);
            //토큰 설정
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
            //logFunc("G Response:" + www.downloadHandler.text);
            //AddStatusText("G Response:" + www.downloadHandler.text);
            googleAccessToken = JsonUtility.FromJson<GoogleResponseData>(www.downloadHandler.text).access_token;

            yield return EmptyBodyUpload(ServerDefine.authServerRoot + "/social/login/google/" + googleAccessToken);
        }
    }

    IEnumerator NormalAccount(string path)
    {
        //WWWForm form = new WWWForm();
        //form.AddField("password", pwdStr);
        //form.AddField("username", idStr);

        string body = "{\"password\": \"" + pwdStr + "\",\"username\": \"" + idStr + "\"}";
        //Debug.Log(body);

        var www = new UnityWebRequest(ServerDefine.authServerRoot + "/" + path, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        /*
        UnityWebRequest www = UnityWebRequest.Post("http://175.210.61.143:8080/" + path, form);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        */

        if (www.isNetworkError || www.isHttpError)
        {
            logFunc(www.error);
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");

            if(path == "login/account")
            {
                TokenData tokenData = JsonUtility.FromJson<TokenData>(www.downloadHandler.text);
                SetToken(tokenData.token);
                AccountInfo.PlayerAccountType = AccountType.Normal;
                AccountInfo.userId = idStr;
                account.SetButton();
            }
            else if(path == "join/account")
            {
                if(www.downloadHandler.text=="success")
                {
                    logFunc("응답:" + www.downloadHandler.text);
                    confirmUI.SetActive(true);
                }
                else
                {
                    ServerResponse<string> response = JsonUtility.FromJson<ServerResponse<string>>(www.downloadHandler.text);
                    logFunc("응답:" + response.responseMessage);
                    notconfirmUI.SetActive(true);
                }
            }

            //토큰 설정
        }
    }

    private void SetToken(string token)
    {
        logFunc("토큰:" + token);
        AccountInfo.token = token;
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
