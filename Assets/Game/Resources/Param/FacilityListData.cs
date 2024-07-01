using IkosamiSave;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
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

    [Multiline]
    public string アップグレード;
    [Multiline]
    public string 施設;

    [Button("値反映")]
    public void ParseData()
    {
        var csvData = アップグレード.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
        csvData.RemoveAt(0);
        for (int i = 0; i < csvData.Count; i++)
        {
            string line = csvData[i];
            var columns = line.Split('\t');

            if (i >= powerUpItemList.Count)
            {
                Debug.LogError($"powerUpItemListの数が足りません ({i}:{csvData.Count}/{powerUpItemList.Count})");
                return;
            }
            var item = powerUpItemList[i];

            item.name = columns[0];
            item.id = columns[1];
            item.idIndex = Parse.ToInt(columns[2], 0);
            item.editNum = Parse.ToInt(columns[3], 0);
            item.cost = Parse.ToDouble(columns[4], 1);
            item.manual = columns[6];
            item.kind = (PowerUpKind)Enum.Parse(typeof(PowerUpKind), columns[7]);
            item.power = Parse.ToDouble(columns[8], 1);
            item.powerId = Parse.ToInt(columns[9], 0);
            item.lockKind = (PowerUpKind)Enum.Parse(typeof(PowerUpKind), columns[10]);
            item.lockId = Parse.ToInt(columns[11], 1);
            item.lockNum = Parse.ToDouble(columns[12], 1);
            item.isDebug = Parse.ToBool(columns[13], false);

        }
    }

    [Button("施設")]
    public void ParseFacility()
    {
        var csvData = 施設.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
        csvData.RemoveAt(0);
        for (int i = 0; i < csvData.Count; i++)
        {
            string line = csvData[i];
            var columns = line.Split('\t');

            if (i >= facilityItemList.Count)
            {
                Debug.LogError($"facilityItemListの数が足りません ({i}:{csvData.Count}/{facilityItemList.Count})");
                return;
            }
            var item = facilityItemList[i];

            item.name = columns[0];
            item.description = columns[1];
            item.id = Parse.ToInt(columns[2], 0);
            item.baseCost = Parse.ToDouble(columns[3], 1);
            item.basePower = Parse.ToDouble(columns[4], 1);
        }
    }

    //[Button("出力 PowerUpItem")]
    void ToStringPowerUpItem()
    {
        string str = "";
        for (int i = 120; i < powerUpItemList.Count; i++)
        {
            PowerUpItem item = powerUpItemList[i];
            str += item + "\n";
        }
        Debug.Log(str);
    }
    //[Button("出力 FacilityItem")]
    void ToStringFacilityItem()
    {
        string str = "";
        for (int i = 0; i < facilityItemList.Count; i++)
        {
            FacilityItem item = facilityItemList[i];
            str += item + "\n";
        }
        Debug.Log(str);
    }

    public FacilityItem GetData(int iD)
    {
        return facilityItemList.Find(x => x.id == iD);
    }
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

    public override string ToString()
    {
        return $"{name}\t{description}\t{id}\t{baseCost}\t{basePower}\t{nowCost}";
    }

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
        return SaveManager.Instance.GetInt("facility_item_" + id, 0);
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

        AudioMgr.Instance.PlaySE(1);
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
    public override string ToString()
    {
        return $"{name}\t{id}\t{idIndex}\t{editNum}\t{cost}\t{manual}\t{kind}\t{power}\t{powerId}\t{lockKind}\t{lockId}\t{lockNum}\t{isDebug}";
    }


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
