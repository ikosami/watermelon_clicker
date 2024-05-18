using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] TextMeshProUGUI powerText;

    [SerializeField] Button buyButton;
    private PowerUpItem item;
    private bool isActive = true;
    //条件を満たしていないので表示されていない状態
    private bool isLock = true;


    private void Awake()
    {
        buyButton.onClick.AddListener(() =>
        {
            if (!item.Buy())
            {
                return;
            }
            AudioManager.instance.PlaySE(1);

            if (item.kind == PowerUpKind.Original)
            {
                switch (item.powerId)
                {
                    case -1:
#if UNITY_EDITOR
                        PlayerPrefs.DeleteAll();
                        UnityEditor.EditorApplication.isPlaying = false;
#endif
                        break;
                    case -2:
                        GameData.Instance.value += 10000;
                        break;
                }
            }

            Destroy(gameObject);
        });
    }
    private void Update()
    {
        if (isLock)
        {
            return;
        }
        if (!isActive && GameData.Instance.value >= item.cost)
        {
            buyButton.image.color = Color.white;
            costText.color = Color.green;
            isActive = true;
        }
        else if (isActive && GameData.Instance.value < item.cost)
        {
            buyButton.image.color = Color.gray;
            costText.color = Color.red;
            isActive = false;
        }
    }
    public void SetItem(PowerUpItem powerUpItem)
    {
        item = powerUpItem;
        //既に有効なら消す
        if (item.GetActive())
        {
            Destroy(gameObject);
            return;
        }
        CheckLock();

        nameText.text = powerUpItem.name;
        costText.text = FormatBigNum.GetNumStr(powerUpItem.cost);
        powerText.text = string.Format(powerUpItem.manual, FormatBigNum.GetNumStr(powerUpItem.power));
    }

    public void CheckLock()
    {
        if (!isLock)
        {
            return;
        }

        switch (item.lockKind)
        {
            case PowerUpKind.None:
                isLock = false;
                break;
            case PowerUpKind.Click:
                if (SaveManager.Instance.GetInt(SaveKey.ClickNum, 1) >= item.lockNum)
                {
                    isLock = false;
                }
                break;
            case PowerUpKind.ALL:
                if (SaveManager.Instance.GetDouble(SaveKey.ALLNum, 1) >= item.lockNum)
                {
                    isLock = false;
                }
                break;
            case PowerUpKind.Facility:
                var facilityItems = FacilityListData.Instance.facilityItemList;
                if (facilityItems[item.lockId].GetNum() >= item.lockNum)
                {
                    isLock = false;
                }
                break;
        }

        if (isLock)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
