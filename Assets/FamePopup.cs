using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FamePopup : MonoBehaviour
{
    [SerializeField] Button closeButton;
    [SerializeField] Button fameButton;

    // Start is called before the first frame update
    public void Init(GameObject resetLockImage)
    {
        closeButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySE(1);
            gameObject.SetActive(false);
        });

        fameButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySE(1);
            double preValue = GameManager.Instance.GetFame();
            var fame = SaveManager.Instance.GetDouble("fame", preValue);
            SaveManager.Instance.SaveDelete();
            SaveManager.Instance.AddDouble("fame", fame + preValue);
            SaveManager.Instance.Save();
            GameManager.Instance.Start();
            resetLockImage.SetActive(true);
            gameObject.SetActive(false);
        });

        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
