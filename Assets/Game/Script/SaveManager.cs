using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// セーブ管理
/// </summary>
public class SaveManager
{
    private static SaveManager instance;
    public static SaveManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SaveManager();
            }
            return instance;
        }
    }


    private Dictionary<string, object> saveData = new Dictionary<string, object>();

    /// <summary>
    /// int値
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetInt(string key, int value)
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
    public int GetInt(string key, int defaultValue = 0)
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
    public void AddInt(string key, int value)
    {
        SetInt(key, GetInt(key) + value);
    }



    public void SetDouble(string key, double value)
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

    public double GetDouble(string key, double defaultValue = 0)
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
    public void AddDouble(string key, double value)
    {
        SetDouble(key, GetDouble(key) + value);
    }

    public void SetString(string key, string value)
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

    public string GetString(string key, string defaultValue = "")
    {
        if (!saveData.ContainsKey(key))
        {
            return defaultValue;
        }
        var data = saveData[key];
        return data.ToString();
    }
    public DateTime GetDateTime(string key, DateTime defaultValue)
    {
        var data = GetString(key, "");
        DateTime.TryParse(data, out defaultValue);
        return defaultValue;
    }
    public TimeSpan GetTimeSpan(string key, TimeSpan defaultValue)
    {
        var data = GetString(key, "");
        TimeSpan.TryParse(data, out defaultValue);
        return defaultValue;
    }

    public void Save()
    {
        var data = MiniJSON.Json.Serialize(saveData);
        PlayerPrefs.SetString("save_data", data);
    }


    internal void Load()
    {
        saveData = MiniJSON.Json.Deserialize(PlayerPrefs.GetString("save_data", "{}")) as Dictionary<string, object>;
    }

    public void SaveDelete()
    {
        saveData.Clear();
    }
}
