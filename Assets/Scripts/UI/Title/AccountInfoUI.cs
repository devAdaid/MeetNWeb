using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using Google;

public class AccountInfoUI : MonoBehaviour
{
    public Text account;
    public Text userID;
    public Text token;

    private void OnEnable()
    {
        string acc = "";
        AccountType accountType = AccountInfo.PlayerAccountType;

        switch(accountType)
        {
            case AccountType.Facebook:
                acc = "페이스북 계정";
                break;
            case AccountType.Google:
                acc = "구글 계정";
                break;
            case AccountType.Normal:
                acc = "밋앤그릿 계정";
                break;
            case AccountType.Guest:
                acc = "게스트 로그인";
                break;
        }

        account.text = acc;
        userID.text = "계정 ID: " + AccountInfo.userId;
        token.text = AccountInfo.token;
    }
}
