using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlusTextManager : MonoBehaviour
{

    private static PlusTextManager instance;
    public static PlusTextManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlusTextManager>();
            }
            return instance;
        }
    }

    [SerializeField] PlusText[] plusText;

    public void SetText(string str)
    {

        Vector3 pos = Input.mousePosition;
        pos.z = 1;
        // マウスのスクリーン座標をワールド座標に変換
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pos);
        var mousePosition = worldPosition;
        for (int i = 0; i < plusText.Length; i++)
        {
            if (!plusText[i].isActive)
            {
                plusText[i].Init(str);
                plusText[i].transform.position = mousePosition;
                plusText[i].transform.position += new Vector3(0, 0.1f, 0);
                return;
            }
        }
    }
}
