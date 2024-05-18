using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickerButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public Action onClick;
    private bool isDown = false;
    private bool isClick = false;
    [SerializeField] private float time = 0.5f;
    private float timer = 0;
    [SerializeField] private float nextTime = 1f;
    private float nextTimer = 0;
    bool isAction = false;

    /// <summary>
    /// 開始処理
    /// </summary>
    public virtual void Init()
    {
    }

    protected virtual void Update()
    {
        if (isDown)
        {
            if (nextTimer > 0)
            {
                nextTimer -= Time.deltaTime;
                return;
            }

            if (timer <= 0)
            {
                onClick.Invoke();
                timer = time;
            }
            timer = Mathf.Max(0, timer - Time.deltaTime);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isDown)
        {
            timer = time;
            nextTimer = nextTime;
        }
        isAction = false;
        isDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isAction)
        {
            onClick.Invoke();
        }
    }
}
