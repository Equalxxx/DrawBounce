using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPointer : MonoBehaviour
{
    public Transform effectTrans;
    public Transform cubeTrans;

    public Camera mainCam;
    public Camera uiCam;

    private void Update()
    {
        Vector3 pos = mainCam.WorldToScreenPoint(cubeTrans.position);
        effectTrans.position = uiCam.ScreenToWorldPoint(pos);
        
    }
}
