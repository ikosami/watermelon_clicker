using IkosamiSave;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// セーブ管理
/// </summary>
public class SaveManagerDic : SaveManager
{
    private Dictionary<string, object> saveData = new Dictionary<string, object>();

    public override void Initialize()
    {
        base.Initialize();
        //ロード
        saveData = MiniJSON.Json.Deserialize(PlayerPrefs.GetString("save_data", "{}")) as Dictionary<string, object>;
    }
    public override void DeleteAll()
    {
        base.DeleteAll();
        saveData.Clear();
    }

    /// <summary>
    /// int値
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public override void SetInt(string key, int value)
    {
        if (!saveData.ContainsKey(key))
        {
            saveData.Add(key, value);
        }
        else
        {
            saveData[key] = value;
        }
    }
    public override int GetInt(string key, int defaultValue = 0)
    {
        if (!saveData.ContainsKey(key))
        {
            return defaultValue;
        }
        var data = saveData[key];

        int value = defaultValue;
        int.TryParse(data.ToString(), out value);
        return value;
    }



    public override void SetDouble(string key, double value)
    {
        if (!saveData.ContainsKey(key))
        {
            saveData.Add(key, value);
        }
        else
        {
            saveData[key] = value;
        }
    }

    public override double GetDouble(string key, double defaultValue = 0)
    {
        if (!saveData.ContainsKey(key))
        {
            return defaultValue;
        }
        var data = saveData[key];

        double value = defaultValue;
        double.TryParse(data.ToString(), out value);
        return value;
    }

    public override void SetString(string key, string value)
    {
        if (!saveData.ContainsKey(key))
        {
            saveData.Add(key, value);
        }
        else
        {
            saveData[key] = value;
        }
    }

    public override string GetString(string key, string defaultValue = "")
    {
        if (!saveData.ContainsKey(key))
        {
            return defaultValue;
        }
        var data = saveData[key];
        return data.ToString();
    }
    public void AddDouble(string key, double value)
    {
        SetDouble(key, GetDouble(key) + value);
    }
    public override DateTime GetDateTime(string key, DateTime defaultValue)
    {
        var data = GetString(key, "");
        DateTime.TryParse(data, out defaultValue);
        return defaultValue;
    }
    public override TimeSpan GetTimeSpan(string key, TimeSpan defaultValue)
    {
        var data = GetString(key, "");
        TimeSpan.TryParse(data, out defaultValue);
        return defaultValue;
    }

    public override void Save()
    {
        var data = MiniJSON.Json.Serialize(saveData);
        PlayerPrefs.SetString("save_data", data);
    }

}
