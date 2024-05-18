using System;
using System.Text;
using UnityEditor;
using UnityEngine;

public class PlayerPrefsController : MonoBehaviour
{

    [MenuItem("GM/セーブデータ削除")]
    static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("GM/ClearCache")]
    static void CleanPlayerContent()
    {
        Caching.ClearCache();
    }
}
