using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools
{

    public static string GetTime(double seconds)
    {
        int m = (int)(seconds / 60);
        int s = (int)seconds % 60;
        return string.Format("{0}:{1:00}", m, s);
    }
}
