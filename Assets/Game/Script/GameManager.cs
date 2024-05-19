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

    [SerializeField] private Button resetButton;
    [SerializeField] private Button rankingButton;
    [SerializeField] private Button tweetButton;
    [SerializeField] private Button adsButton;

    [SerializeField] private Image resetLockImage;
    [SerializeField] private TextMeshProUGUI resetText;
    [SerializeField] private TextMeshProUGUI adsText;
    [SerializeField] private TextMeshProUGUI tweetText;
    string adsTextDefault;
    string tweetTextDefault;

    [SerializeField] MainClicker mainClicker;
    [SerializeField] FamePopup famePopup;
    [SerializeField] Popup popup;
    private string clickTextStr = "";


    bool isInit = false;

    public void Awake()
    {


        //rankingButton.onClick.AddListener(() =>
        //{
        //    AudioManager.instance.PlaySE(1);
        //    naichilab.RankingLoader.Instance.SendScoreAndShowRanking(Math.Floor(power));
        //});

        adsButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySE(1);
            GameData.Instance.adsTime = DateTime.Now;
            UpdatePower();
            string[] urls = new string[] { "https://twitter.com/ikosami", "https://www.youtube.com/watch?v=PKvBM3qHMOU", "https://www.youtube.com/watch?v=QbatlrUUxFs" };
            string url = urls[UnityEngine.Random.Range(0, urls.Length)];

            //Twitter投稿画面の起動
            Application.OpenURL(url);
        });

        tweetButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySE(1);
            GameData.Instance.tweetTime = DateTime.Now;
            UpdatePower();
            //urlの作成
            string esctext = UnityWebRequest.EscapeURL(string.Format("「単位クリッカー」\n\n秒間収入が{0}になりました。\nhttps://unityroom.com/games/unit_clicker",
                FormatBigNum.GetNumStr(GameData.Instance.power)));
            string esctag = UnityWebRequest.EscapeURL("unity1week");
            string url = "https://twitter.com/intent/tweet?text=" + esctext + "&hashtags=" + esctag;

            //Twitter投稿画面の起動
            Application.OpenURL(url);
        });

        resetButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySE(1);
            double preValue = GetFame();
            if (preValue <= 0)
            {
                return;
            }
            famePopup.Init(resetLockImage.gameObject);
        });
    }

    /// <summary>
    /// 開始処理
    /// </summary>
    public void Start()
    {
        instance = this;

        adsTextDefault = adsText.text;
        tweetTextDefault = tweetText.text;

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
            AudioManager.instance.PlaySE(0);
            GameData.Instance.value += GameData.Instance.clickPower;
            SaveManager.Instance.AddDouble(SaveKey.ALLNum, GameData.Instance.clickPower);
            plusTextManager.SetText(clickTextStr);
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

    public double GetFame()
    {
        var allNum = SaveManager.Instance.GetDouble(SaveKey.ALLNum, 1);
        return Math.Floor(Math.Sqrt(Math.Sqrt(allNum / 1000000)));
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
        var addPower = GameData.Instance.power * mainClickerButton.GetMulti() * multi;
        powerText.text = FormatBigNum.GetNumStr(addPower) + "/s";


        var span = DateTime.Now - GameData.Instance.preUpdateTime;
        GameData.Instance.value += addPower * span.TotalSeconds;

        if (isView)
        {
            //経過時間
            var minus = (int)(DateTime.Now - GameData.Instance.preUpdateTime).TotalMinutes;
            var add = addPower * minus / 1000;
            GameData.Instance.value += add;
            if (add > 1)
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
                ViewPopup("放置ボーナス", string.Format("放置ボーナス {1} ({0})\n最大6時間分入手可能", timeStr, FormatBigNum.GetNumStr(add)));

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


        if ((DateTime.Now - GameData.Instance.adsTime).Minutes < 2)
        {
            var seconds = (new TimeSpan(0, 2, 0) - (DateTime.Now - GameData.Instance.adsTime)).TotalSeconds;
            adsText.text = string.Format("秒間収入3倍中 {0}", GetTime(seconds));
        }
        else if (adsText.text != adsTextDefault)
        {
            adsText.text = adsTextDefault;
            UpdatePower();
        }

        if ((DateTime.Now - GameData.Instance.tweetTime).Minutes < 10)
        {
            var seconds = (new TimeSpan(0, 10, 0) - (DateTime.Now - GameData.Instance.tweetTime)).TotalSeconds;
            tweetText.text = string.Format("秒間収入1.5倍中 {0}", GetTime(seconds));
        }
        else if (tweetText.text != tweetTextDefault)
        {
            tweetText.text = tweetTextDefault;
            UpdatePower();
        }

        if (resetLockImage.gameObject.activeSelf)
        {
            if (allNum > 1000000)
            {
                resetLockImage.gameObject.SetActive(false);
            }
        }
        else
        {
            resetText.text = string.Format("名声{0}", FormatBigNum.GetNumStr(GetFame()));
        }
    }

    public string GetTime(double seconds)
    {
        int m = (int)(seconds / 60);
        int s = (int)seconds % 60;
        return string.Format("{0}:{1:00}", m, s);
    }

    public void UpdatePower()
    {
        Debug.LogError("UpdatePower");
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
        Debug.LogError($"clickMulti:{clickMulti} clickPar:{clickPar} clickBoost:{clickBoost} allMulti:{allMulti} facilityNumMulti:{facilityNumMulti}");

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

        var fame = SaveManager.Instance.GetDouble("fame");
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


        clickTextStr = string.Format("+{0}", FormatBigNum.GetNumStr(GameData.Instance.clickPower));

        clickText.text = FormatBigNum.GetNumStr(GameData.Instance.clickPower) + "/click";

        basePowerText.text = FormatBigNum.GetNumStr(GameData.Instance.power);
        multiText.text = string.Format("+{0}%", multi / 100);
        fameText.text = string.Format("{0}(+{1}%)", FormatBigNum.GetNumStr(fame), FormatBigNum.GetNumStr(fame * 10));
    }
}
