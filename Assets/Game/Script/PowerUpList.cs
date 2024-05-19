using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 施設一覧
/// </summary>
public class PowerUpList : MonoBehaviour
{

    private static PowerUpList instance;
    public static PowerUpList Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PowerUpList>();
            }
            return instance;
        }
    }
    [SerializeField] PowerUp powerUpPrefab;
    private PowerUp[] powerUps;

    /// <summary>
    /// 開始処理
    /// </summary>
    public void Init()
    {
        var transforms = transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < transforms.Length; i++)
        {
            if (transforms[i].gameObject == gameObject)
            {
                continue;
            }
            Destroy(transforms[i].gameObject);
        }

        var powerUpItems = FacilityListData.Instance.powerUpItemList;
        powerUps = new PowerUp[powerUpItems.Count];
        for (int i = 0; i < powerUps.Length; i++)
        {
            powerUps[i] = Instantiate(powerUpPrefab, transform);
            powerUps[i].SetItem(powerUpItems[i]);
        }
    }

    internal void ChangeLock()
    {
        for (int i = 0; i < powerUps.Length; i++)
        {
            if (powerUps[i] == null) continue;
            powerUps[i].CheckLock();
        }
    }
}
