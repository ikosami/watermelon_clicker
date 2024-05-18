using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int lv = 1;
    public bool isDestroy = false;
    [SerializeField] GameObject _ui;
    [SerializeField] TextMeshPro _text;
    [SerializeField] TextMeshPro _powerText;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] CircleCollider2D _collider;
    float baseSize = 0.25f;

    [SerializeField] Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _ui.transform.rotation = Quaternion.identity;
        _ui.transform.position = transform.position + new Vector3(0, 0, -1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDestroy)
        {
            return;
        }

        var ball = collision.collider.gameObject.GetComponent<Ball>();
        if (ball != null && ball.gameObject != null)
        {
            if (ball.isDestroy || ball.lv != lv)
            {
                return;
            }

            var newBall = Instantiate(this, (transform.position + ball.transform.position) / 2, Quaternion.identity);
            newBall.SetBaseSize(baseSize);
            newBall.SetLv(lv + 1);

            isDestroy = true;
            ball.isDestroy = true;
            Destroy(gameObject);
            Destroy(ball.gameObject);
        }
    }

    public void SetLv(int p)
    {
        lv = p;
        _text.text = lv.ToString();

        _spriteRenderer.sprite = sprites[lv % sprites.Length];

        var power = Mathf.Pow(3, lv - 1);
        _powerText.text = FormatBigNum.GetNumStr(power);
        var size = baseSize * (1 + (lv % 10) * 0.025f);
        transform.localScale = new Vector3(size, size, size);
    }
    public void SetBaseSize(float v)
    {
        baseSize = v;
        _collider.radius = 0.75f;
    }
}
