using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmView : MonoBehaviour
{
    public List<FarmViewData> farmViews;
    public int ID;
    private void Start()
    {
        GMAction.onFacilityChange += ViewUpdate;
        ViewUpdate();
    }
    private void ViewUpdate()
    {
        var facility = FacilityListData.Instance.GetData(ID);
        var level = facility.GetNum();

        // levelがneedLevel以上なら表示
        foreach (var farmView in farmViews)
        {
            farmView.obj.SetActive(farmView.needLevel <= level);
        }
    }
}

[Serializable]
public class FarmViewData
{
    public int needLevel;
    public GameObject obj;
}
