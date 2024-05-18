using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDestroy : MonoBehaviour
{
    float timer = 0;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 20)
        {
            Destroy(gameObject);
        }
    }
}
