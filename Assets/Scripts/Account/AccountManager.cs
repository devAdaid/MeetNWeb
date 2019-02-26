using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using Google;

public class AccountManager : MonoBehaviour
{
    public GameObject loginUI;
    public GameObject logoutUI;
    public GameObject guestJoinUI;
    public GameObject guestLoginUI;
    public Text guestUserNumText;
    public Text statusText;
    public string webClientId = "<your client id here>";
    public ServerManager server;

    private GoogleSignInConfiguration configuration;

    // Defer the configuration creation until Awake so the web Client ID
    // Can be set via the property inspector in the Editor.
    protected void Awake()
    {
        // 페이스북 초기화
        FB.Init();

        // 구글 초기화
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestIdToken = true
        };
    }

    private void Start()
    {
        //AccountType[] tryes = { AccountType.Facebook, AccountType.Google };
        /*
        try
        {
            TryAutoLogin();
        }
        catch
        {
            AddStatusText("자동로그인 실패");
            AccountInfo.PlayerAccountType = AccountType.NotLogin;
        }
        */
        AccountInfo.PlayerAccountType = AccountType.NotLogin;
        SetButton();
    }

    public void SetButton()
    {
        bool isLogined = (AccountInfo.PlayerAccountType != AccountType.NotLogin);

        loginUI.SetActive(!isLogined);
        logoutUI.SetActive(isLogined);
    }

    private void TryAutoLogin()
    {
        switch (AccountInfo.PlayerAccountType)
        {
            case AccountType.Google:
                // 구글 로그인 시도
                AddStatusText("구글 자동 로그인 시도");
                GoogleSignIn.Configuration = configuration;
                GoogleSignIn.Configuration.UseGameSignIn = false;
                GoogleSignIn.Configuration.RequestIdToken = true;
                GoogleSignIn.DefaultInstance.SignInSilently()
                      .ContinueWith(OnAuthenticationFinished);
                
                break;
        }
    }

    public void FacebookLogin()
    {
        //LogOut();
        try
        {
            FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email" }, FacebookHandleResult);
        }
        catch
        {
            AddStatusText("페이스북 왜안되");
        }
    }

    protected void FacebookHandleResult(IResult result)
    {
        if (result == null)
        {
            AddStatusText("페이스북 Null Response");
            return;
        }

        // Some platforms return the empty string instead of null.
        if (!string.IsNullOrEmpty(result.Error))
        {
            AddStatusText("페이스북 에러: " + result.Error);
            AddStatusText("페이스북 에러: " + result.RawResult);
        }
        else if (result.Cancelled)
        {
            AddStatusText("페이스북 취소: " + result.RawResult);
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            AddStatusText("페이스북 성공: " + AccessToken.CurrentAccessToken);
            server.userID = AccessToken.CurrentAccessToken.UserId;
            server.SendFacebookToken(AccessToken.CurrentAccessToken.TokenString);
            
            SetButton();
            //ToastManager.Instance.ShowToastOnUiThread("페이스북 로그인 성공");
        }
        else
        {
            AddStatusText("페이스북 Null Response");
        }
    }

    public void GoogleLogin()
    {
        //LogOut();

        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestAuthCode  = true;

        AddStatusText("구글 로그인 시도");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
          OnAuthenticationFinished);
    }

    public void GuestAccount()
    {
        int userNum = PlayerPrefs.GetInt("GuestID", 0);
        if (userNum == 0)
        {
            // 게스트 계정 생성
            guestJoinUI.SetActive(true);
        }
        else
        {
            // 게스트 로그인
            guestLoginUI.SetActive(true);
            guestUserNumText.text = "게스트 ID: " + userNum;
        }
    }

    public void GuestLogin()
    {
        int userNum = PlayerPrefs.GetInt("GuestID", 0);
        server.userID = "Guest " + userNum;
        server.SendGuestLogin(userNum);
    }

    public void SetServerToken(string str)
    {
        AccountInfo.token = str;
    }

    public void LogOut()
    {
        AddStatusText("로그아웃?");

        switch (AccountInfo.PlayerAccountType)
        {
            case AccountType.Facebook:
                FB.LogOut();
                AddStatusText("페이스북 로그아웃");
                break;
            case AccountType.Google:
                GoogleSignIn.DefaultInstance.SignOut();
                try
                {
                    AddStatusText("구글 로그아웃");
                }
                catch
                {
                    AddStatusText("구글 로그아웃 안되");
                }
                SetButton();
                break;
        }

        AccountInfo.PlayerAccountType = AccountType.NotLogin;
        SetButton();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<System.Exception> enumerator =
                    task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error =
                            (GoogleSignIn.SignInException)enumerator.Current;
                    AddStatusText("구글 로그인 에러: " + error.Status + " " + error.Message);
                }
                else
                {
                    AddStatusText("구글 로그인 예외" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            AddStatusText("구글 로그인 취소");
        }
        else
        {
            server.userID = task.Result.DisplayName;
            server.SendGoogleToken(task.Result.AuthCode);
            //accountType = AccountType.Google;
            AddStatusText("구글 성공: ");
            SetButton();
            //ToastManager.Instance.ShowToastOnUiThread("구글 로그인 성공");
        }
    }
    
    private List<string> messages = new List<string>();
    public void AddStatusText(string text)
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
        statusText.text = txt;
    }
}
