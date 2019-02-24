using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AccountType
{
    None,
    Facebook,
    Google,
    Email,
    Guest,
    NotLogin
};

public class AccountInfo
{
    public static AccountType PlayerAccountType
    {
        get
        {
            if(_playerAccountType == AccountType.None)
            {
                _playerAccountType = (AccountType) Enum.Parse(typeof(AccountType), PlayerPrefs.GetString("AccountType", "NotLogin"));
                PlayerPrefs.SetString("AccountType", _playerAccountType.ToString());
            }
            return _playerAccountType;
        }
        set
        {
            _playerAccountType = value;
            PlayerPrefs.SetString("AccountType", _playerAccountType.ToString());
            PlayerPrefs.Save();
        }
    }
    public static string token = null;

    private static AccountType _playerAccountType = AccountType.None;

}
