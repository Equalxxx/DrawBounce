using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLFramework;

public class TestFadeScreen : MonoBehaviour {

    public bool fade;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            if(FadeScreen.Instance.StartFade(fade))
            {
                fade = !fade;
            }
        }
	}
}
