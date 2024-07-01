using IkosamiSave;
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
            AudioMgr.Instance.PlaySE(1);
            gameObject.SetActive(false);
        });

        fameButton.onClick.AddListener(() =>
        {
            AudioMgr.Instance.PlaySE(1);
            double preValue = GameData.Instance.GetFame();
            var fame = SaveManager.Instance.GetDouble("fame", preValue);
            SaveManager.Instance.DeleteAll();
            SaveManager.Instance.AddDouble("fame", fame + preValue);
            SaveManager.Instance.Save();
            GameManager.Instance.Start();
            resetLockImage.SetActive(true);
            gameObject.SetActive(false);
        });

        gameObject.SetActive(true);
    }
}
