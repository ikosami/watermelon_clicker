using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI bodyText;
    [SerializeField] Button closeText;
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
}
