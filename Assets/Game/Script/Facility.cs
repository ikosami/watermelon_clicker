using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Facility : MonoBehaviour
{
    [SerializeField] Image facilityImage;
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

    [SerializeField] GameObject _superPowerUp;

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

        if (facilityItem.sprite != null)
        {
            facilityImage.sprite = facilityItem.sprite;
        }
        nameText.text = facilityItem.name;

        UpdateView();
        SetLock(true);
    }

    public void SetLock(bool v)
    {
        lockObj.gameObject.SetActive(v);
    }

    private void UpdateView()
    {
        //descriptionText.text = facilityItem.description;
        costText.text = FormatBigNum.GetNumStr(facilityItem.GetCost());
        numText.text = "Lv" + facilityItem.GetNum().ToString();

        var value = facilityItem.GetPower() * GameData.Instance.nowMulti;
        var value2 = facilityItem.GetPower(1) * GameData.Instance.nowMulti;
        valueText.text = string.Format("{0}/s<color=#ff0000>(+{1})</color>", FormatBigNum.GetNumStr(value), FormatBigNum.GetNumStr(value2 - value));

        _superPowerUp.gameObject.SetActive(facilityItem.IsNextSuper());
    }
    public bool CheckLock(double value)
    {
        isLock = facilityItem.baseCost / 100 >= value;
        return isLock;
    }
}
