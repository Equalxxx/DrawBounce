using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : DefaultBlock
{
    public float width;
    public float t;
    public float duration = 1f;

	private void FixedUpdate()
    {
        t += Time.fixedDeltaTime / duration;

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
