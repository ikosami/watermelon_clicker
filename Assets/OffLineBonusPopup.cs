using IkosamiSave;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OffLineBonusPopup : PopupBase
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI bodyText;
    [SerializeField] TextMeshProUGUI barText;
    [SerializeField] Button closeText;

    [SerializeField] Image barImage;

    public const int OfflineTimeHourMax = 3;
    public const int OfflineTimeMax = 60 * OfflineTimeHourMax;
    // Start is called before the first frame update
    void Start()
    {
        closeText.onClick.AddListener(() =>
        {
            AudioMgr.Instance.PlaySE(1);
            gameObject.SetActive(false);
        });
    }
    public void View(string title, string body)
    {
        titleText.text = title;
        bodyText.text = body;
        gameObject.SetActive(true);
    }

    public void SetTime(int minus)
    {
        int h = (int)minus / 60;
        int m = (int)minus % 60;
        string timeStr = "";
        if (h > 0)
        {
            timeStr = string.Format("{0}時間{1:00}分", h, m);
        }
        else
        {
            timeStr = string.Format("{0}分", m);
        }

        barText.text = $"{timeStr} / {OfflineTimeHourMax}時間00分";
        barImage.fillAmount = (float)minus / (60 * 6);
    }

    public void SetValue(double add)
    {
        GameData.Instance.value += add;
        SaveManager.Instance.AddDouble(SaveKey.ALLNum, add);
        GameData.Instance.Save();
        bodyText.text = "入手 " + FormatBigNum.GetNumStr(add);
    }
}
