using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OffLineBonusPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI bodyText;
    [SerializeField] TextMeshProUGUI barText;
    [SerializeField] Button closeText;

    [SerializeField] Image barImage;
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

    internal void SetTime(int minus)
    {
        minus = Mathf.Min(minus, 60 * 6);
        int h = (int)minus / 60;
        int m = (int)minus % 60;
        string timeStr = "";
        if (h > 0)
        {
            timeStr = string.Format("{0}時間{1}分", h, m);
        }
        else
        {
            timeStr = string.Format("{0}分", m);
        }

        barText.text = timeStr + " / 6時間";
        barImage.fillAmount = (float)minus / (60 * 6);
    }

    internal void SetValue(double add)
    {
        bodyText.text = "入手 " + FormatBigNum.GetNumStr(add);
    }
}
