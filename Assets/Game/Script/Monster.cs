using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private static Monster instance;
    public static Monster Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Monster>();
            }
            return instance;
        }
    }

    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI enemyNameText;


    int enemyNum
    {
        get { return GameData.Instance.enemyNum; }
        set { GameData.Instance.enemyNum = value; }
    }
    double enemyHp
    {
        get { return GameData.Instance.enemyHp; }
        set { GameData.Instance.enemyHp = value; }
    }
    double enemyMaxHp
    {
        get { return GameData.Instance.enemyMaxHp; }
        set { GameData.Instance.enemyMaxHp = value; }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Damage(float power)
    {

        enemyHp -= power;


        PlusTextManager.Instance.SetText(FormatBigNum.GetNumStr(power));


        hpText.text = string.Format("{0}/{1}", FormatBigNum.GetNumStr(enemyHp), FormatBigNum.GetNumStr(enemyMaxHp));
        if (enemyHp <= 0)
        {
            GameData.Instance.value += Mathf.Pow(1.25f, enemyNum) * 1000;
            GameManager.Instance.UpdatePower();

            enemyNum++;
            SetEnemy();
        }
    }

    public void SetEnemy()
    {
        SetEnemy(enemyNum, 500 * Math.Pow(1.15f, enemyNum));
    }
    public void SetEnemy(int num, double hp)
    {
        enemyNum = num;
        enemyHp = hp;
        enemyMaxHp = hp;
        enemyNameText.text = string.Format("Enemy{0}", enemyNum);
        hpText.text = string.Format("{0}/{1}", FormatBigNum.GetNumStr(enemyHp), FormatBigNum.GetNumStr(enemyMaxHp));
    }
}

