using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLFramework;

public class TestSceneControl : Singleton<TestSceneControl> {
    
    public string[] sceneNames;
    public int curSceneIndex;

    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        ResourceManager.ReleaseAll();
        SceneControl.LoadNextScene(sceneNames[curSceneIndex]);
    }

    void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            curSceneIndex++;
            if (curSceneIndex >= sceneNames.Length)
                curSceneIndex = 0;

            SceneControl.LoadNextScene(sceneNames[curSceneIndex]);
        }
	}
}
