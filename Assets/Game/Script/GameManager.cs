using IkosamiSave;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// ゲームメイン
/// </summary>
public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    [SerializeField] private MainClicker mainClickerButton;
    [SerializeField] private FacilityList facilityList;
    [SerializeField] private PowerUpList powerUpList;
    [SerializeField] private PlusTextManager plusTextManager;

    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private TextMeshProUGUI clickText;
    [SerializeField] private TextMeshProUGUI powerText;


    [SerializeField] private TextMeshProUGUI allNumText;
    [SerializeField] private TextMeshProUGUI basePowerText;
    [SerializeField] private TextMeshProUGUI multiText;
    [SerializeField] private TextMeshProUGUI clickNumText;
    [SerializeField] private TextMeshProUGUI playTimeText;
    [SerializeField] private TextMeshProUGUI fameText;


    [SerializeField] MainClicker mainClicker;
    [SerializeField] Popup popup;
    [SerializeField] OffLineBonusPopup offLineBonusPopup;


    bool isInit = false;

    public void Awake()
    {


    }

    /// <summary>
    /// 開始処理
    /// </summary>
    public void Start()
    {
        instance = this;

        GameData.Instance.Load();
        StartCoroutine(GameData.Instance.SaveIE());

        facilityList.Init();
        powerUpList.Init();
        UpdatePower();

        //アプリを閉じていた間は少し減らして入手
        AddPower(0.7f, true);

        ViewUpdate();

        mainClickerButton.onClick = () =>
        {
            AudioMgr.Instance.PlaySE(0);
            var value = GameData.Instance.clickPower * mainClicker.GetMulti();
            GameData.Instance.value += value;
            SaveManager.Instance.AddDouble(SaveKey.ALLNum, value);
            ;
            plusTextManager.SetText(string.Format("+{0}", FormatBigNum.GetNumStr(value)));
            ViewUpdate();
            SaveManager.Instance.AddInt(SaveKey.ClickNum, 1);
        };
        mainClickerButton.Init();

        if (!isInit)
        {
            StartCoroutine(AddCorutine());
        }
        isInit = true;
    }

    IEnumerator AddCorutine()
    {
        while (true)
        {
            AddPower();
            yield return new WaitForSeconds(0.1f);
        }
    }

    //増加
    private void AddPower(float multi = 1, bool isView = false)
    {
        var addPower = GameData.Instance.power * mainClickerButton.GetMulti();
        powerText.text = FormatBigNum.GetNumStr(addPower) + "/s";


        var span = DateTime.Now - GameData.Instance.preUpdateTime;
        GameData.Instance.value += addPower * span.TotalSeconds;

        if (isView)
        {
            //経過時間
            var minus = (int)(DateTime.Now - GameData.Instance.preUpdateTime).TotalMinutes;
            var add = addPower * minus * 60 * multi;
            GameData.Instance.value += add;
            if (add > 1)
            {
                offLineBonusPopup.SetTime(minus);
                offLineBonusPopup.SetValue(add);
                offLineBonusPopup.gameObject.SetActive(true);

                GameData.Instance.Save();

                SaveManager.Instance.AddDouble(SaveKey.ALLNum, add);
            }
        }
        GameData.Instance.playTime += span;

        GameData.Instance.preUpdateTime = DateTime.Now;
        ViewUpdate();
    }

    public void ViewPopup(string title, string body)
    {
        popup.View(title, body);
    }


    private void ViewUpdate()
    {
        valueText.text = FormatBigNum.GetNumStr(GameData.Instance.value);

        var allNum = SaveManager.Instance.GetDouble(SaveKey.ALLNum, 1);
        allNumText.text = FormatBigNum.GetNumStr(allNum);
        clickNumText.text = SaveManager.Instance.GetInt(SaveKey.ClickNum, 1).ToString();
        playTimeText.text = GameData.Instance.playTime.ToString(@"d\.hh\:mm\:ss");
        facilityList.ChangeLock();


        powerUpList.ChangeLock();
    }


    public void UpdatePower()
    {
        var powerListItem = FacilityListData.Instance.powerUpItemList;
        var facilityListItem = FacilityListData.Instance.facilityItemList;

        GameData.Instance.power = 0;
        GameData.Instance.clickPower = 1;

        float clickBoost = 200;
        //クリック倍率
        double clickMulti = 1;
        //クリック%
        double clickPar = 0;
        //全倍率
        double allMulti = 0;
        double facilityNumMulti = 0;

        double[] facilityMulti = new double[facilityListItem.Count];
        for (int i = 0; i < facilityMulti.Length; i++)
        {
            facilityMulti[i] = 1;
        }

        //パワーアップによる効果
        for (int i = 0; i < powerListItem.Count; i++)
        {
            var data = powerListItem[i];
            if (!data.isActive)
            {
                continue;
            }

            switch (data.kind)
            {
                case PowerUpKind.Click:
                    clickMulti *= data.power;
                    break;
                case PowerUpKind.ClickPar:
                    clickPar += data.power;
                    break;
                case PowerUpKind.ClickBoost:
                    clickBoost += (float)data.power;
                    break;
                case PowerUpKind.ALL:
                    allMulti += data.power;
                    break;
                case PowerUpKind.Facility:
                    facilityMulti[data.powerId] *= data.power;
                    break;
                case PowerUpKind.FacilityNum:
                    facilityNumMulti += data.power;
                    break;
            }
        }
        //Debug.LogError($"clickMulti:{clickMulti} clickPar:{clickPar} clickBoost:{clickBoost} allMulti:{allMulti} facilityNumMulti:{facilityNumMulti}");

        int facilityNum = 0;
        for (int i = 0, len = facilityListItem.Count; i < len; i++)
        {
            var data = facilityListItem[i];
            GameData.Instance.power += data.GetPower() * facilityMulti[i];
            facilityNum += data.GetNum();
        }
        facilityNumMulti *= facilityNum;
        //倍率
        double multi = 0;
        multi += Math.Floor(allMulti * 100 + facilityNumMulti);

        GameData.Instance.power *= 1 + (allMulti / 100.0);
        GameData.Instance.power *= 1 + (facilityNumMulti / 10000.0);

        var fame = SaveManager.Instance.GetDouble("fame", 0);
        GameData.Instance.power *= 1 + (fame / 10.0);


        if ((DateTime.Now - GameData.Instance.adsTime).Minutes < 2)
        {
            GameData.Instance.power *= 1.5f;
        }
        if ((DateTime.Now - GameData.Instance.tweetTime).Minutes < 10)
        {
            GameData.Instance.power *= 1.5f;
        }
        GameData.Instance.clickPower += GameData.Instance.power * (clickPar / 100.0);
        GameData.Instance.clickPower *= clickMulti;

        mainClickerButton.maxPower = clickBoost;



        clickText.text = FormatBigNum.GetNumStr(GameData.Instance.clickPower) + "/click";

        basePowerText.text = FormatBigNum.GetNumStr(GameData.Instance.power);
        multiText.text = string.Format("+{0}%", multi / 100);
        fameText.text = string.Format("{0}(+{1}%)", FormatBigNum.GetNumStr(fame), FormatBigNum.GetNumStr(fame * 10));
    }
}
