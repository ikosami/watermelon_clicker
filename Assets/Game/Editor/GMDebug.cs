using UnityEditor;
using UnityEngine;

public class GMDebug : MonoBehaviour
{
    [MenuItem("GM/1時間経過")]
    static void Create()
    {
        Add(1);
    }
    [MenuItem("GM/5時間経過")]
    static void Add5()
    {
        Add(5);
    }
    [MenuItem("GM/24時間経過")]
    static void Add24()
    {
        Add(24);
    }
    [MenuItem("GM/100時間経過")]
    static void Add100()
    {
        Add(100);
    }

    static void Add(int hour)
    {
        var add = GameData.Instance.power * hour * 60 * 60;

        var offLineBonusPopup = PopupManager.Create<OffLineBonusPopup>();
        offLineBonusPopup.SetTime(hour * 60);
        offLineBonusPopup.SetValue(add);
        offLineBonusPopup.Open();
    }
}
