using IkosamiSave;
using UnityEngine;

/// <summary>
/// 施設一覧
/// </summary>
public class FacilityList : MonoBehaviour
{
    [SerializeField] Facility facilityPrefab;
    [SerializeField] Transform parent;
    private Facility[] facilitys;

    /// <summary>
    /// 開始処理
    /// </summary>
    public void Init()
    {
        foreach (Transform t in parent)
        {
            Destroy(t.gameObject);
        }

        var facilityItems = FacilityListData.Instance.facilityItemList;
        facilitys = new Facility[facilityItems.Count];
        for (int i = 0; i < facilitys.Length; i++)
        {
            facilitys[i] = Instantiate(facilityPrefab, parent);
            facilitys[i].SetItem(facilityItems[i]);
        }
        nowUnlock = 0;
    }

    int nowUnlock = 0;
    public void ChangeLock()
    {
        var value = SaveManager.Instance.GetDouble(SaveKey.ALLNum, 0);
        for (int i = nowUnlock; i < facilitys.Length; i++)
        {
            if (facilitys[i].isLock)
            {
                facilitys[i].SetLock(false);
                facilitys[i].isLock = false;
            }
            // 施設が表示解放されていない
            if (facilitys[i].CheckLock(value))
            {
                facilitys[i].isLock = false;
                break;
            }
            nowUnlock = i;
        }

    }
}
