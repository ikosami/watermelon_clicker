using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void SetText(string str, Vector3 mousePosition)
    {
        for (int i = 0; i < plusText.Length; i++)
        {
            if (!plusText[i].isActive)
            {
                plusText[i].Init(str);
                plusText[i].transform.position = mousePosition;
                return;
            }
        }
    }
}
