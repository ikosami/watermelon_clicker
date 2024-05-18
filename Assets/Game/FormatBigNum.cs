using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormatBigNum : MonoBehaviour
{

    public static string GetNumStr(double current, int taniStart = 0)
    {
        var baseCurrent = current;
        if (double.IsInfinity(current) || double.IsNaN(current))
        {
            return "Infinity";
        }

        string[] str = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc", "Ud", "Dd", "Td" };
        int tani = 0;

        while (current >= 1000)
        {
            current /= 1000;
            tani++;
        }

        if (tani <= taniStart)
        {
            baseCurrent = Math.Floor(baseCurrent);
            return baseCurrent.ToString("F0");
        }
        else if (tani < str.Length)
        {
            if (tani <= 0)
            {
                current = Math.Floor(current);
                return current.ToString("F0") + str[tani]; // 整数の場合、小数点以下を表示しない
            }
            else
            {
                current = Math.Floor(current * 1000) / 1000;
                return current.ToString("F3") + str[tani]; // 小数点以下1桁までを表示
            }
        }
        else
        {
            current = Math.Floor(current * 1000) / 1000;
            return current.ToString("F3") + "e+" + (3 * tani); // 指数表記
        }
    }
}
