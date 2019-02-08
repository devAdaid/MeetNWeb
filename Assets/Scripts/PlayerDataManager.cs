using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : PersistentSingleton<PlayerDataManager>
{
    #region getseter
    public int Stemina
    {
        get { return _stemina; }
        set
        {
            _stemina = value;
            PlayerMoneyUI.SetSteminaUI(_stemina, MaxStemina);

            // 모든 스태미너 풀로 채워짐
            if (_stemina > MaxStemina)
            {
                _stemina = MaxStemina;
            }
        }
    }

    public int Gold
    {
        get { return _gold; }
        set
        {
            PlayerMoneyUI.SetGoldUI(_gold);
            _gold = value;
        }
    }

    public int Cash
    {
        get { return _cash; }
        set
        {
            PlayerMoneyUI.SetCashUI(_cash);
            _cash = value;
        }
    }

    public DateTime SteminaUpdateTime
    {
        get { return _steminaUpdateTime; }
        set
        {
            _steminaUpdateTime = value;
        }
    }

    public PlayerMoneyUI PlayerMoneyUI
    {
        get
        {
            if (_PlayerMoneyUI == null)
                _PlayerMoneyUI = FindObjectOfType<PlayerMoneyUI>();
            return _PlayerMoneyUI;
        }
    }
    #endregion

    public const int MaxStemina = 5;
    public DateTime currentTime;
    public TimeSpan steminaChargeTime;

    private int _stemina = 0;
    private int _gold = 0;
    private int _cash = 0;
    private DateTime _steminaUpdateTime;
    //public const long SteminaChargeTime = 100000000;
    private PlayerMoneyUI _PlayerMoneyUI = null;

    protected override void Initialize()
    {
        steminaChargeTime = new TimeSpan(0, 10, 0);
        FirstSetting();
        /*
        if(!ES3.KeyExists("FirstRun"))
        {
            FirstSetting();
            ES3.Save<bool>("FirstRun", true);
        }
        else
        {

        }
        */
    }

    private void FirstSetting()
    {
        Stemina = MaxStemina - 3;
        SteminaUpdateTime = DateTime.Now;
        Gold = 10000;
        Cash = 500;
    }

    private void Update()
    {
        currentTime = DateTime.Now;
        if (PlayerMoneyUI != null)
        {
            PlayerMoneyUI.SetTimeUI();
        }

        if (!IsSteminaFull())
        {
            TimeSpan diffTime = currentTime - SteminaUpdateTime;
            if (diffTime > steminaChargeTime)
            {
                Stemina += 1;
                SteminaUpdateTime += steminaChargeTime;
            }
            /*
            int diffHour = (int)((currentTime - steminaFirstConsumeTime).TotalHours);
            if (diffHour > MaxStemina)
            {
                diffHour = MaxStemina;
            }
            */
        }
    }

    public bool IsSteminaFull()
    {
        return _stemina == MaxStemina;
    }
}
