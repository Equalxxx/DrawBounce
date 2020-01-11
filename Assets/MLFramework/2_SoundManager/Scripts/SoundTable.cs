using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTable : ScriptableObject {
    [System.Serializable]
    public class SoundInfo
    {
        public int index;
        public string path;
        public string tag;
    }

    public List<SoundInfo> soundInfoList = new List<SoundInfo>();

    public SoundInfo GetSoundInfo(string soundTag)
    {
        SoundInfo soundInfo = soundInfoList.Find(x => x.tag.Equals(soundTag));

        return soundInfo;
    }
}
