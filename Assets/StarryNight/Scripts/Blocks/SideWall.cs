using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWall : MonoBehaviour
{
    public bool left;
    public Vector3 offset;
    public BoxCollider2D boxColl2D;

    private void Awake()
    {
        if (boxColl2D == null)
            boxColl2D = GetComponent<BoxCollider2D>();

        SetPosition();
    }

    private void OnValidate()
    {
        if (boxColl2D == null)
            boxColl2D = GetComponent<BoxCollider2D>();

        SetPosition();
    }

    void SetPosition()
    {
        Vector3 pos = Vector3.zero;

        if (!left)
        {
            pos.x = Screen.width;
        }

        pos.y = Screen.height/2f;

        pos = Camera.main.ScreenToWorldPoint(pos);

        transform.position = pos + offset;
    }
}
