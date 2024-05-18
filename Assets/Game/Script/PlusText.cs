
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlusText : MonoBehaviour
{
    public static readonly Vector2 up = new Vector2(0, 0.5f);

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI text;
    public bool isActive = false;

    private float timer = 0;
    private float timerMax = 0.5f;
    private float range = 70;

    RectTransform rect;

    private void Awake()
    {
        rect = transform as RectTransform;
    }

    public void Init(string str)
    {
        rect.anchoredPosition = new Vector2(Random.Range(-range, range), Random.Range(-range, range) - 20);
        timer = timerMax;
        if (text.text != str)
        {
            text.text = str;
        }
        canvasGroup.alpha = 1;
        isActive = true;
    }

    /// <summary>
    /// çXêVèàóù
    /// </summary>
    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        rect.anchoredPosition += up;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            isActive = false;
            return;
        }
        var par = timer / timerMax;
        canvasGroup.alpha = par;
    }
}
