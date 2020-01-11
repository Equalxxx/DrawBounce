using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;

[System.Serializable]
public class TestSaveData
{
    public int idx;
    public string str;
}


public class TestSaveLoad : MonoBehaviour {

    public string fileName = "savedata.fun";
    public string filePath = "";

    public TestSaveData testSave;
    public TestSaveData testLoad;

    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.S))
        {
            SaveFileManager.Save<TestSaveData>(testSave, fileName, filePath);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            testLoad = SaveFileManager.Load<TestSaveData>(fileName, filePath);
        }
    }
}
