﻿using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class FacilityListData : ScriptableObject
{
    private static FacilityListData instance;
    public static FacilityListData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<FacilityListData>("Param/FacilityListData");
            }
            return instance;
        }
    }

    public List<PowerUpItem> powerUpItemList;
    public List<FacilityItem> facilityItemList;
}



[Serializable]
public class FacilityItem
{
    public string name;
    public string description;
    public int id;
    public double baseCost = 1;
    public double basePower = 1;
    private double nowCost = 0;


    /// <summary>
    /// 現在のコスト
    /// </summary>
    /// <returns>現在のコスト</returns>
    public double GetNowCost()
    {
        return nowCost;
    }

    public double GetCost()
    {
        var cost = Math.Floor(baseCost * Math.Pow(1.15, GetNum()));
        nowCost = cost;
        return cost;
    }
    public double GetPower()
    {
        return basePower * GetNum();
    }

    public int GetNum()
    {
        return SaveManager.Instance.GetInt("facility_item_" + id);
    }

    public void Buy(int v)
    {
        if (GameData.Instance.value < nowCost)
        {
            return;
        }

        // Unity1week用
        //if (id == 13 && GetNum() == 0)
        //{
        //    GameManager.Instance.ViewPopup("おめでとうございます！", "ついに正の工場にたどり着きました！\n");
        //}

        AudioManager.instance.PlaySE(1);
        GameData.Instance.value -= nowCost;
        SaveManager.Instance.SetInt("facility_item_" + id, GetNum() + v);
        GameManager.Instance.UpdatePower();
    }

}


public enum PowerUpKind
{
    None,
    Original,   //固有のもの     
    ALL,        //全倍率           総収入
    Click,      //クリック倍率     クリック回数
    ClickPar,      //クリックPar     クリック回数
    ClickBoost,      //クリックBoost     クリック回数
    Facility,   //施設倍率         施設数
    FacilityNum,      //施設ボーナス
}

[Serializable]
public class PowerUpItem
{
    public string name;
    //保存用
    public string id;
    public int idIndex;
    public int editNum;
    public double cost = 1;
    public string manual;

    //効果の対象
    public PowerUpKind kind = PowerUpKind.None;
    public double power = 1;
    //施設IDなど
    public int powerId = 0;

    //ロックの対象
    public PowerUpKind lockKind = PowerUpKind.None;
    public int lockId = 1;
    public double lockNum = 1;
    public bool isDebug = false;

    [NonSerialized]
    public bool isActive = false;

    private string GetID()
    {
        return id + "_" + idIndex;
    }

    public bool GetActive()
    {
#if !UNITY_EDITOR
        if (isDebug) return true;
#endif
        isActive = SaveManager.Instance.GetInt(GetID(), 0) == 1;
        return isActive;
    }

    public bool Buy()
    {
        if (!isActive && GameData.Instance.value < cost)
        {
            return false;
        }
        isActive = true;
        GameData.Instance.value -= cost;
        SaveManager.Instance.SetInt(GetID(), 1);
        GameManager.Instance.UpdatePower();
        return true;
    }

    public void SetSetting(Upgrade upgrade)
    {
        name = upgrade.Name;
        cost = upgrade.Cost;
        lockNum = upgrade.Cost * 0.8;
        power = upgrade.Effect;
    }
}
