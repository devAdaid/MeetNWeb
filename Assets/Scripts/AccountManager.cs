using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using Google;

public enum AccountType
{
    None,
    Facebook,
    Google,
    Guest
};

public class AccountManager : MonoBehaviour
{
    public List<GameObject> accountBtns;
    public Text statusText;
    public AccountType accountType;
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

        TryAutoLogin(AccountType.Google);
        SetButton();
    }

    private void SetButton()
    {
        bool isLogined = (accountType != AccountType.None);

        accountBtns[0].SetActive(!isLogined);
        accountBtns[1].SetActive(!isLogined);
        accountBtns[2].SetActive(!isLogined);
        accountBtns[3].SetActive(isLogined);
    }

    private void TryAutoLogin(AccountType at)
    {
        accountType = AccountType.None;

        switch (at)
        {
            case AccountType.Facebook:
                break;
            case AccountType.Google:
                // 구글 로그인 시도
                GoogleSignIn.Configuration = configuration;
                GoogleSignIn.Configuration.UseGameSignIn = false;
                GoogleSignIn.Configuration.RequestIdToken = true;
                AddStatusText("구글 자동로그인 시도");
                GoogleSignIn.DefaultInstance.SignInSilently()
                      .ContinueWith(OnAuthenticationFinished);
                
                break;
        }
    }

    public void FacebookLogin()
    {
        LogOut();
        try
        {
            FB.LogInWithReadPermissions(null, FacebookHandleResult);
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
            server.SendAccessToken(AccessToken.CurrentAccessToken.TokenString);
            accountType = AccountType.Facebook;
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
        LogOut();

        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddStatusText("구글 로그인 시도");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
          OnAuthenticationFinished);
    }

    public void LogOut()
    {
        AddStatusText("로그아웃?");
        switch (accountType)
        {
            case AccountType.Facebook:
                FB.LogOut();
                AddStatusText("페이스북 로그아웃");
                break;
            case AccountType.Google:
                accountType = AccountType.None;
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

        accountType = AccountType.None;
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
            AddStatusText("구글 성공: " + task.Result.IdToken);
            server.SendAccessToken(task.Result.IdToken);
            accountType = AccountType.Google;
            SetButton();
            //ToastManager.Instance.ShowToastOnUiThread("구글 로그인 성공");
        }
    }
    
    private List<string> messages = new List<string>();
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
        statusText.text = txt;
    }
}
