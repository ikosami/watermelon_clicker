using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Facility : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] TextMeshProUGUI numText;

    [SerializeField] ClickerButton buyButton;
    [SerializeField] Image buttonImage;
    [SerializeField] GameObject lockObj;

    [SerializeField] Color redColor;

    private FacilityItem facilityItem;
    private bool isActive = true;
    [NonSerialized]
    public bool isLock = true;

    private void Start()
    {
        buyButton.onClick = () =>
        {
            facilityItem.Buy(1);
            UpdateView();
        };
        buyButton.Init();
    }

    private void Update()
    {
        if (isLock)
        {
            return;
        }
        if (!isActive && GameData.Instance.value >= facilityItem.GetNowCost())
        {
            buttonImage.color = Color.white;
            costText.color = Color.green;
            isActive = true;
        }
        else if (isActive && GameData.Instance.value < facilityItem.GetNowCost())
        {
            buttonImage.color = Color.gray;
            costText.color = redColor;
            isActive = false;
        }
    }

    /// <summary>
    /// 開始処理
    /// </summary>
    public void SetItem(FacilityItem facilityItem)
    {
        this.facilityItem = facilityItem;
        UpdateView();
        SetLock(true);
    }

    public void SetLock(bool v)
    {
        lockObj.gameObject.SetActive(v);
    }

    private void UpdateView()
    {
        nameText.text = facilityItem.name;
        descriptionText.text = facilityItem.description;
        costText.text = FormatBigNum.GetNumStr(facilityItem.GetCost());
        numText.text = facilityItem.GetNum().ToString();
    }
    public bool CheckLock(double value)
    {
        isLock = facilityItem.baseCost / 10 >= value;
        return isLock;
    }
}
