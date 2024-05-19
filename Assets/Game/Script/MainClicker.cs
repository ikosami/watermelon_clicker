using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainClicker : ClickerButton
{
    [SerializeField] GameObject cube;
    [SerializeField] Ball sphere;
    [SerializeField] GameObject[] kakeras;
    [SerializeField] Image bar;
    [SerializeField] TextMeshProUGUI barText;
    [SerializeField] TextMeshProUGUI barText2;



    private float speed = 1;
    private float multiTimer = 0;

    float bigTimer = 0;
    bool isEffect = false;

    public float clickerMulti = 1;
    public float maxPower = 200;
    int spawnCount = 0;

    float backSuikaTimer = 0;

    //Action<Ball> onUse;

    public override void Init()
    {
        base.Init();

        onClick += () =>
        {
            isEffect = true;

            multiTimer += 0.5f;
            multiTimer = Mathf.Min(multiTimer, maxPower / 10);
        };

        //onUse = Ball =>
        //{
        //    var power = Mathf.Pow(3, Ball.lv - 1);
        //    power *= clickerMulti;
        //    power *= GetMulti();
        //    Monster.Instance.Damage(power);
        //};

        //Monster.Instance.SetEnemy();
    }

    public float GetMulti()
    {
        return 1 + (multiTimer / (maxPower / 10) * maxPower / 100f);
    }


    /// <summary>
    /// 更新処理
    /// </summary>
    protected override void Update()
    {
        base.Update();

        if (multiTimer > 0)
        {
            multiTimer -= Time.deltaTime / 5;
            multiTimer = Mathf.Max(0, multiTimer);
            speed = 1 + multiTimer;

            var par = multiTimer / (maxPower / 10);
            bar.fillAmount = par;
            var str = string.Format("{0}%", Mathf.Floor(100 + par * maxPower));
            barText.text = str;
            barText2.text = str;

        }
        else
        {
            if (barText.text != string.Empty)
            {
                speed = 1;
                bar.fillAmount = 0;
                barText.text = string.Empty;
                var str = string.Format("{0}%", Mathf.Floor(100));
                barText.text = str;
            }
        }

        if (isEffect)
        {
            bigTimer = bigTimer + Time.deltaTime;
            if (bigTimer > 0.1f)
            {
                bigTimer = 0.1f;
                isEffect = false;
            }
        }
        else
        {
            bigTimer = Mathf.Max(0, bigTimer - Time.deltaTime * 2);
        }
        var size = 1 + bigTimer * 2;
        cube.transform.localScale = new Vector3(size, size, size);

        cube.transform.Rotate(new Vector3(
            24f * speed,
            35f * speed,
            3f
            ) * Time.deltaTime);

        backSuikaTimer += Time.deltaTime;
        if (backSuikaTimer > 0.15f)
        {
            backSuikaTimer = 0;
            for (int i = 0; i < 4; i++)
            {
                CreateSuikaPirs();
            }
        }

        //クリック位置にレイを飛ばして2D当たり判定の確認　Ballなら消す
        if (Input.GetMouseButtonDown(0))
        {
            //マウスのある位置を取得(スクリーン座標)
            Vector3 MousePoint = Input.mousePosition;
            //スクリーン座標をワールド座標に変換
            MousePoint = Camera.main.ScreenToWorldPoint(MousePoint);

            //マウスのある位置から、奥(0, 0, 1)に向かってRayを発射（ワールド座標）
            RaycastHit2D[] hit2D = Physics2D.RaycastAll(MousePoint, Vector3.forward);

            //Rayがhitしたオブジェクトに目的のオブジェクトがあるかチェック
            foreach (RaycastHit2D hit in hit2D)
            {
                //中身の確認処理 hit.transform.gameObject.tagとかが便利
                var ball = hit.collider.gameObject.GetComponent<Ball>();
                if (ball != null)
                {
                    //onUse?.Invoke(ball);
                    ball.isDestroy = true;
                    Destroy(ball.gameObject);
                }
            }
        }

    }

    private Ball CreateSuika()
    {
        //ランダムな向きで生成
        var randomRotate = Quaternion.Euler(0, 0, Random.Range(0, 360));
        var x = Random.Range(-2.5f, 2.5f);
        var obj = Instantiate(sphere, cube.transform.position + new Vector3(x, 3, 1), randomRotate);
        obj.SetBaseSize(0.55f);
        obj.SetLv(Random.Range(1, 4));
        return obj;

    }

    //var obj = CreateSuikaPirs();
    private GameObject CreateSuikaPirs()
    {
        //ランダムな向きで生成
        var randomRotate = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        var obj = Instantiate(kakeras[Random.Range(0, kakeras.Length)], cube.transform.position + new Vector3(0, 3, 0), randomRotate);
        obj.transform.localScale = Vector3.one * 0.5f;
        obj.transform.position += new Vector3(Random.Range(-2, 2f), 3, Random.Range(-4, 4f));
        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1), Random.Range(-1f, 1)) * 100);
        obj.transform.localScale = Vector3.one * 0.2f;
        return obj;
    }
}
