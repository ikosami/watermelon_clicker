using IkosamiSave;
using System;
using System.Collections;
using System.Collections.Generic;
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
    public TimeSpan playTimeAll;
    public DateTime preUpdateTime;

    public DateTime tweetTime = default(DateTime);
    public DateTime adsTime = default(DateTime);
    //public int enemyNum;
    //public double enemyHp;
    //public double enemyMaxHp;

    public void Load()
    {
        var saveManager = SaveManager.Instance;

        value = saveManager.GetDouble("value", 0);
        //開始時に計算される
        //clickPower = saveManager.GetDouble("click_power", 0);
        //power = saveManager.GetDouble("power", 0);

        playTime = saveManager.GetTimeSpan("play_time", new TimeSpan(0));
        playTimeAll = saveManager.GetTimeSpan("play_time_all", new TimeSpan(0));
        preUpdateTime = saveManager.GetDateTime("pre_update_time", DateTime.Now);

        //enemyNum = saveManager.GetInt("enemyNum", 0);
        //enemyHp = saveManager.GetDouble("enemyHp", 100);
        //enemyMaxHp = saveManager.GetDouble("enemyMaxHp", 100);
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
        saveManager.SetString("play_time_all", playTimeAll.ToString());
        saveManager.SetString("pre_update_time", preUpdateTime.ToString());

        //saveManager.SetInt("enemyNum", enemyNum);
        //saveManager.SetDouble("enemyHp", enemyHp);
        //saveManager.SetDouble("enemyMaxHp", enemyMaxHp);

        saveManager.Save();
    }

    public void DeleteLocal()
    {
        var saveManager = SaveManager.Instance;

        saveManager.Delete("value");
        saveManager.Delete("play_time");
        saveManager.Delete("pre_update_time");
        saveManager.Delete(SaveKey.ALLNum);

        var saveData = saveManager.GetAllData() as Dictionary<string, object>;
        RemoveKeysContaining(saveData, "facility_item_");
        RemoveKeysContaining(saveData, "power_up_");
    }


    public void AddTime(TimeSpan span)
    {
        playTime += span;
        playTimeAll += span;
    }

    //現状獲得可能な名声
    public double GetFame()
    {
        var allNum = SaveManager.Instance.GetDouble(SaveKey.ALLNum, 1);
        return Math.Floor(Math.Sqrt(Math.Sqrt(allNum / 1000000000)));
    }


    static void RemoveKeysContaining(Dictionary<string, object> dictionary, string keyword)
    {
        List<string> keysToRemove = new List<string>();

        foreach (var kvp in dictionary)
        {
            if (kvp.Key.Contains(keyword))
            {
                keysToRemove.Add(kvp.Key);
            }
        }

        foreach (var key in keysToRemove)
        {
            dictionary.Remove(key);
        }
    }
}
