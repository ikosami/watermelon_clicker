using UnityEngine;
using UnityEngine.UI;

public class TabButtons : MonoBehaviour
{
    [SerializeField] GameObject[] objs;
    [SerializeField] Button[] buttons;

    int nowSelect = 0;
    void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int i1 = i;
            buttons[i].onClick.AddListener(() =>
            {
                SetActive(i1);
            });
        }
        SetActive(nowSelect);
    }

    public void SetActive(int select)
    {
        objs[nowSelect].SetActive(false);
        buttons[nowSelect].transform.localScale = Vector3.one;
        nowSelect = select;
        objs[nowSelect].SetActive(true);
        buttons[nowSelect].transform.localScale = Vector3.one*1.1f;
    }
}
