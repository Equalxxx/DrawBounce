using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWall : MonoBehaviour
{
    public bool left;
    public Vector3 offset;

    private void Awake()
    {
        SetPosition();
    }

    private void OnValidate()
    {
        SetPosition();
    }

    void SetPosition()
    {
        Vector3 pos = Vector3.zero;

        pos.y = Screen.height / 2f;

        if (!left)
        {
            pos.x = Screen.width;
        }

        transform.position = Camera.main.ScreenToWorldPoint(pos) + offset;
    }
}
