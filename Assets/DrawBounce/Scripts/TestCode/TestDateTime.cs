using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestDateTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		Debug.Log(DateTime.Now.ToLongDateString());
		Debug.Log(DateTime.Now.ToLongTimeString());
		Debug.Log(DateTime.Now.ToShortDateString());
		Debug.Log(DateTime.Now.ToShortTimeString());
		Debug.Log(DateTime.Now.ToString("yyyyMMddHHmmss"));
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
