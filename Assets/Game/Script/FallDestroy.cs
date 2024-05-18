using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDestroy : MonoBehaviour
{
    float timer = 0;
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }

        timer += Time.deltaTime;

        if (timer > 20)
        {
            Destroy(gameObject);
        }
    }
}
