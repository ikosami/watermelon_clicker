﻿using IkosamiSave;
using System;
using System.Collections;
using UnityEngine;

public class GameData
{
    private static GameData instance;
    public static GameData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameData();
            }
            return instance;
        }
    }

    public double value = 0;
    public double clickPower = 1;
    public double power = 0;



    public TimeSpan playTime;
    public DateTime preUpdateTime;

    public DateTime tweetTime = default(DateTime);
    public DateTime adsTime = default(DateTime);
    public int enemyNum;
    public double enemyHp;
    public double enemyMaxHp;

    public void Load()
    {
        var saveManager = SaveManager.Instance;

        value = saveManager.GetDouble("value", 0);
        //開始時に計算される
        //clickPower = saveManager.GetDouble("click_power", 0);
        //power = saveManager.GetDouble("power", 0);

        playTime = saveManager.GetTimeSpan("play_time", new TimeSpan(0));
        preUpdateTime = saveManager.GetDateTime("pre_update_time", DateTime.Now);

        enemyNum = saveManager.GetInt("enemyNum", 0);
        enemyHp = saveManager.GetDouble("enemyHp", 100);
        enemyMaxHp = saveManager.GetDouble("enemyMaxHp", 100);
    }

    public IEnumerator SaveIE()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            Save();
        }
    }

    public void Save()
    {
        var saveManager = SaveManager.Instance;
        saveManager.SetDouble("value", value);
        //開始時に計算される
        //saveManager.SetDouble("click_power", clickPower);
        //saveManager.SetDouble("power", power);

        saveManager.SetString("play_time", playTime.ToString());
        saveManager.SetString("pre_update_time", preUpdateTime.ToString());

        saveManager.SetInt("enemyNum", enemyNum);
        saveManager.SetDouble("enemyHp", enemyHp);
        saveManager.SetDouble("enemyMaxHp", enemyMaxHp);

        saveManager.Save();
    }




    //現状獲得可能な名声
    public double GetFame()
    {
        var allNum = SaveManager.Instance.GetDouble(SaveKey.ALLNum, 1);
        return Math.Floor(Math.Sqrt(Math.Sqrt(allNum / 1000000)));
    }
}
