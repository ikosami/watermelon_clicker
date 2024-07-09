using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Facility : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    //[SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] TextMeshProUGUI numText;
    [SerializeField] TextMeshProUGUI valueText;

    [SerializeField] ClickerButton buyButton;
    [SerializeField] Image buttonImage;
    [SerializeField] GameObject lockObj;

    [SerializeField] Color redColor;
    [SerializeField] Color defaultColor;

    private FacilityItem facilityItem;
    private bool isActive = true;
    [NonSerialized]
    public bool isLock = true;
    [SerializeField] ParticleSystem _particleSystem;

    private void Start()
    {
        buyButton.onClick = () =>
        {
            bool isBuy = facilityItem.Buy(1);
            if (isBuy)
            {
                _particleSystem.Play();
                UpdateView();
            }
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
            costText.color = defaultColor;
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
        //descriptionText.text = facilityItem.description;
        costText.text = FormatBigNum.GetNumStr(facilityItem.GetCost());
        numText.text = "x" + facilityItem.GetNum().ToString();

        valueText.text = string.Format("{0}/s", FormatBigNum.GetNumStr(facilityItem.GetPower()));
    }
    public bool CheckLock(double value)
    {
        isLock = facilityItem.baseCost / 100 >= value;
        return isLock;
    }
}
