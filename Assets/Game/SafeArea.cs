using UnityEngine;
using UnityEngine.UI;

public class SafeArea : MonoBehaviour
{
    public enum PadType
    {
        Pixel,
        CanvasScale
    }

    public bool isAutoAdjust = true;

    public float PadTop;
    public float PadBottom;
    public float PadLeft;
    public float PadRight;


    private void Start()
    {
        if (isAutoAdjust)
        {
            Adjust();
            Log("Start");
        }
    }

    void OnEnable()
    {
        if (isAutoAdjust)
        {
            // Adjust();

            Log("Start");
        }
    }


    private void Log(string log)
    {
#if UNITY_EDITOR
        RectTransform rT = GetCanvasRect();
        Debug.Log($"{log} SafeArea padBottm:{PadBottom} witdh:{rT.rect.width}  height:{rT.rect.height}, Screen: width {Screen.width}, height {Screen.height} SafeArea{Screen.safeArea}, SafeArea.yMax:{Screen.safeArea.yMax}, ymin: {Screen.safeArea.yMin}, {(Screen.height - Screen.safeArea.yMax)}");

#endif
    }

    private RectTransform GetCanvasRect()
    {
        var t = transform;
        while (t.parent != null)
        {
            t = t.parent;
            if (t.GetComponent<CanvasScaler>())
            {
                break;
            }
        }

        return t as RectTransform;
    }

    /// <summary>
    /// 調整
    /// </summary>
    public void Adjust()
    {
        RectTransform canvasRect = GetCanvasRect();
        if (canvasRect == null) return;


        //設定値分補正
        var left = PadLeft;
        var right = PadRight;
        var top = PadTop;
        var bottom = PadBottom;
        //var bottom = GetAdsSize(50);

#if UNITY_EDITOR

        var safeArea = Screen.safeArea;

        //セーフエリア分補正
        left += AdjastHeight(canvasRect, safeArea.xMin);
        bottom += AdjastWidth(canvasRect, safeArea.yMin);
        right -= AdjastWidth(canvasRect, Screen.width - safeArea.xMax);
        top -= AdjastHeight(canvasRect, Screen.height - safeArea.yMax);

        RectTransform rT = transform as RectTransform;
        rT.offsetMin = new Vector2(left, bottom);
        rT.offsetMax = new Vector2(right, top);
#else

        //セーフエリア分補正
        left += Screen.safeArea.xMin;
        bottom += Screen.safeArea.yMin;
        // bottom = Mathf.Max(bottom, Screen.safeArea.yMin);
        right += (Screen.safeArea.xMax - Screen.width) / 2f;
        top += (Screen.safeArea.yMax - Screen.height) / 2f;

        var scaleRate = canvasRect.sizeDelta / new Vector2(Screen.width, Screen.height);
        RectTransform rT = transform as RectTransform;
        rT.offsetMin = new Vector2(left * scaleRate.x, bottom * scaleRate.y);
        rT.offsetMax = new Vector2(right * scaleRate.x, top * scaleRate.y);
#endif
    }

    private float AdjastWidth(RectTransform original, float value)
    {
        var result = value;
        result *= original.rect.width / Screen.width;
        return result;
    }

    private float AdjastHeight(RectTransform original, float value)
    {
        var result = value;
        result *= original.rect.height / Screen.height;
        return result;
    }
}
