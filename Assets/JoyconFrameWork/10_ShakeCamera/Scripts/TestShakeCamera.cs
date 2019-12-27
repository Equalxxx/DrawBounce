using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoyconFramework;

public class TestShakeCamera : MonoBehaviour {
    
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ShakeCamera.ShakePosOrder();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ShakeCamera.ShakeRotOrder();
        }
    }
}
