using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float width;
    public float t;

    private void FixedUpdate()
    {
        t += Time.fixedDeltaTime;

        Vector2 pos = transform.position;
        pos.x = Mathf.Lerp(-width, width, t);

        if(t >=1f)
        {
            t = 0f;
            width = -width;
        }

        transform.position = pos;
    }
}
