using IkosamiSave;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScreenOther : MonoBehaviour
{
    [SerializeField] private Button resetButton;
    [SerializeField] private Button rankingButton;
    [SerializeField] private Button tweetButton;
    [SerializeField] private Button adsButton;

    [SerializeField] private Image resetLockImage;
    [SerializeField] private TextMeshProUGUI resetText;
    [SerializeField] private TextMeshProUGUI adsText;
    [SerializeField] private TextMeshProUGUI tweetText;

    [SerializeField] FamePopup famePopup;
    string adsTextDefault = "秒間収入が2分間 1.5倍になります。";
    string tweetTextDefault = "秒間収入が10分間 1.5倍になります。";


    // Start is called before the first frame update
    void Start()
    {

        //rankingButton.onClick.AddListener(() =>
        //{
        //    AudioMgr.Instance.PlaySE(1);
        //    naichilab.RankingLoader.Instance.SendScoreAndShowRanking(Math.Floor(power));
        //});

        adsButton.onClick.AddListener(() =>
        {
            AudioMgr.Instance.PlaySE(1);
            GameData.Instance.adsTime = DateTime.Now;
            GameManager.Instance.UpdatePower();
            string[] urls = new string[] { "https://twitter.com/ikosami", "https://www.youtube.com/watch?v=PKvBM3qHMOU", "https://www.youtube.com/watch?v=QbatlrUUxFs" };
            string url = urls[UnityEngine.Random.Range(0, urls.Length)];

            //Twitter投稿画面の起動
            Application.OpenURL(url);
        });

        tweetButton.onClick.AddListener(() =>
        {
            AudioMgr.Instance.PlaySE(1);
            GameData.Instance.tweetTime = DateTime.Now;
            GameManager.Instance.UpdatePower();
            //urlの作成
            string esctext = UnityWebRequest.EscapeURL(string.Format("「単位クリッカー」\n\n秒間収入が{0}になりました。\nhttps://unityroom.com/games/unit_clicker",
                FormatBigNum.GetNumStr(GameData.Instance.power)));
            string esctag = UnityWebRequest.EscapeURL("unity1week");
            string url = "https://twitter.com/intent/tweet?text=" + esctext + "&hashtags=" + esctag;

            //Twitter投稿画面の起動
            Application.OpenURL(url);
        });


        //名声ポップアップ
        resetButton.onClick.AddListener(() =>
        {
            AudioMgr.Instance.PlaySE(1);
            double preValue = GameData.Instance.GetFame();
            if (preValue <= 0)
            {
                return;
            }
            famePopup.Init(resetLockImage.gameObject);
        });

    }

    private void OnEnable()
    {
        ViewUpdate();
    }

    // Update is called once per frame
    void ViewUpdate()
    {

        var allNum = SaveManager.Instance.GetDouble(SaveKey.ALLNum, 1);

        var seconds = (new TimeSpan(0, 2, 0) - (DateTime.Now - GameData.Instance.adsTime)).TotalSeconds;
        if (seconds < 2 * 60 && seconds > 0)
        {
            adsText.text = string.Format("秒間収入3倍中 {0}", Tools.GetTime(seconds));
        }
        else if (adsText.text != adsTextDefault)
        {
            adsText.text = adsTextDefault;
            GameManager.Instance.UpdatePower();
        }

        seconds = (new TimeSpan(0, 10, 0) - (DateTime.Now - GameData.Instance.tweetTime)).TotalSeconds;
        if (seconds < 10 * 60 && seconds > 0)
        {
            tweetText.text = string.Format("秒間収入1.5倍中 {0}", Tools.GetTime(seconds));
        }
        else if (tweetText.text != tweetTextDefault)
        {
            tweetText.text = tweetTextDefault;
            GameManager.Instance.UpdatePower();
        }

        double lockNum = 1000000;
        resetLockImage.gameObject.SetActive(allNum < lockNum);
        if (resetLockImage.gameObject.activeSelf)
        {
            resetText.text = string.Format("名声{0}でリセット可能", FormatBigNum.GetNumStr(lockNum));
        }
        else
        {
            resetText.text = string.Format("名声{0}", FormatBigNum.GetNumStr(GameData.Instance.GetFame()));
        }

    }
}
