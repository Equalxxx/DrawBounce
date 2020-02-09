using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLFramework
{
    public class SoundManager : Singleton<SoundManager>
    {
        public ResourceManager.LinkType resLinkType = ResourceManager.LinkType.Resources;
        private Dictionary<string, AudioClip> soundDic;

		public static bool IsMuteBGM;
		public static bool IsMuteSE;

		private AudioSource seAudio;     //사운드 이펙트 AudioSource
		private AudioSource bgmAudio0;      //주플레이 AudioSource
		private AudioSource bgmAudio1;		//빠지는 플레이 AudioSource

        public float masterVolume = 1f;		//마스터 볼륨
        public float crossFadeTime = 5f;	//fade 시간

		private float seVolume = 1f;		//사운드 이펙트 볼륨
		public float seCustomVolume = 1f;	//사운드 이펙트 조절 볼륨
		public float seMaxVolume = 1f;      //사운드 이펙트 최대 볼륨

		private float bgmVolume0 = 0f;      //BGM AudioSource 0번 볼륨
		private float bgmVolume1 = 0f;		//BGM AudioSource 1번 볼륨
		public float bgmCustomVolume = 1f;	//BGM 조절 볼륨
		public float bgmMaxVolume = 1f;     //BGM 최대 볼륨

		private bool fadeAudio;

        //Sound Data
        [SerializeField]
        private string tablePath = "datatables";
        private SoundTable soundTable;

        private void Awake()
        {
            if(Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            soundDic = new Dictionary<string, AudioClip>();

            soundTable = ResourceManager.LoadAsset<SoundTable>(tablePath, "SoundTable", resLinkType);

			seAudio = this.gameObject.AddComponent<AudioSource>();
			bgmAudio0 = this.gameObject.AddComponent<AudioSource>();
			bgmAudio1 = this.gameObject.AddComponent<AudioSource>();

			bgmAudio0.loop = true;
            bgmAudio1.loop = true;

            seAudio.volume = 0f;
            bgmAudio0.volume = 0f;
            bgmAudio1.volume = 0f;

            DontDestroyOnLoad(this.gameObject);
        }

        private void Update()
        {
            //Sound가 플레이 중이라면..
            if (fadeAudio)
            {
                //주플레이 AudioSource 는 볼륨을 올린다.
                if (bgmVolume0 < bgmMaxVolume)
                {
                    bgmVolume0 += Time.deltaTime / crossFadeTime;
                    if (bgmVolume0 >= bgmMaxVolume)
                        bgmVolume0 = bgmMaxVolume;
                }

                //빠진는 AudioSource 는 볼륨을 내린다.
                if (bgmVolume1 > 0f)
                {
                    bgmVolume1 -= Time.deltaTime / crossFadeTime;
                    if (bgmVolume1 <= 0f)
                    {
                        bgmVolume1 = 0f;
                        bgmAudio1.Stop();
                        fadeAudio = false;
                    }
                }
            }

            seAudio.volume = seVolume * seCustomVolume * masterVolume;
			bgmAudio0.volume = bgmVolume0 * bgmCustomVolume * masterVolume;
            bgmAudio1.volume = bgmVolume1 * bgmCustomVolume * masterVolume;
        }

        private void OnDestroy()
        {
            ResourceManager.ReleaseAll();
        }

        ///<summary>
        ///Prepare Sound Asset
        ///</summary>
        public void PrepareAssets(string tag)
        {
            AudioClip soundClip = GetAudioClip(tag);
            if (soundClip == null)
            {
                Debug.LogError("Prepare Faild. Sound data : " + tag);
                return;
            }

            Debug.Log("Prepare Success. Sound data : " + tag);
        }

        ///<summary>
        ///2D Sound OneShot
        ///</summary>
        public void PlaySound2D(string tag, float volume = 1f, float volumeScale = 1f)
        {
            AudioClip soundClip = GetAudioClip(tag);
            if (soundClip == null)
            {
                Debug.LogError("Not found Sound data : " + tag);
                return;
            }

			seVolume = volume;
            seAudio.PlayOneShot(soundClip, volumeScale);
        }

        ///<summary>
        ///2D Sound Loop
        ///</summary>
        public void PlaySound2D(string tag, GameObject audioObject, float volume = 1f, bool loop = false)
        {
            if (audioObject == null)
            {
                Debug.LogError("Sound target is null : " + tag);
                return;
            }

            AudioClip soundClip = GetAudioClip(tag);
            if (soundClip == null)
            {
                Debug.LogError("Not found Sound data : " + tag);
                return;
            }

            AudioSource audio = audioObject.GetComponent<AudioSource>();
            if (audio == null)
            {
                audio = audioObject.AddComponent<AudioSource>();
            }

            audio.volume = volume;
            audio.loop = loop;
            audio.clip = soundClip;
            audio.Play();
        }

        ///<summary>
        ///3D Sound OneShot
        ///</summary>
        public void PlaySound3D(string tag, GameObject audioObject, float volume = 1f, float spatialBlend = 1f)
        {
            if (audioObject == null)
            {
                Debug.LogError("Sound target is null : " + tag);
                return;
            }

            AudioClip soundClip = GetAudioClip(tag);
            if (soundClip == null)
            {
                Debug.LogError("Not found Sound data : " + tag);
                return;
            }

            AudioSource spatialAudio = audioObject.GetComponent<AudioSource>();
            if (spatialAudio == null)
            {
                spatialAudio = audioObject.AddComponent<AudioSource>();
            }

            spatialAudio.volume = volume;
            spatialAudio.spatialBlend = spatialBlend;
            spatialAudio.PlayOneShot(soundClip);
        }

        ///<summary>
        ///3D Sound Loop
        ///</summary>
        public void PlaySound3D(string tag, GameObject audioObject, float volume = 1f,
            float spatialBlend = 1f, bool loop = false)
        {
            if (audioObject == null)
            {
                Debug.LogError("Sound target is null : " + tag);
                return;
            }

            AudioClip soundClip = GetAudioClip(tag);
            if (soundClip == null)
            {
                Debug.LogError("Not found Sound data : " + tag);
                return;
            }

            AudioSource spatialAudio = audioObject.GetComponent<AudioSource>();
            if (spatialAudio == null)
            {
                spatialAudio = audioObject.AddComponent<AudioSource>();
            }

            spatialAudio.volume = volume;
            spatialAudio.spatialBlend = spatialBlend;
            spatialAudio.loop = loop;
            spatialAudio.clip = soundClip;
            spatialAudio.Play();
        }

        ///<summary>
        ///BGM
        ///</summary>
        public void PlayMusic(string tag, bool fade = false)
        {
            AudioClip soundClip = GetAudioClip(tag);
            if (soundClip == null)
            {
                Debug.LogError("Not found Sound data : " + tag);
                return;
            }

            if (!fade)
            {
                bgmVolume0 = 1f;
                bgmAudio0.clip = soundClip;
                bgmAudio0.Play();
            }
            else
            {
                //기존에 플레이되는 것을 1 번으로
                AudioSource temp = bgmAudio0;
                bgmAudio0 = bgmAudio1;
                bgmAudio1 = temp;

                //볼륨값스왑
                float tempVolume = bgmVolume0;
                bgmVolume0 = bgmVolume1;
                bgmVolume1 = tempVolume;

                //클립에 새로운 오디오 클립 물린다
                bgmAudio0.clip = soundClip;
                bgmAudio0.Play();
                fadeAudio = true;
            }
        }

        ///<summary>
        ///Stop BGM
        ///</summary>
        public void StopAudio(bool fade = false)
        {
            if (fade)
            {
                //기존에 플레이되는 것을 1 번으로
                AudioSource temp = bgmAudio0;
                bgmAudio0 = bgmAudio1;
                bgmAudio1 = temp;

                //볼륨값스왑
                float tempVolume = bgmVolume0;
                bgmVolume0 = bgmVolume1;
                bgmVolume1 = tempVolume;

                bgmAudio0.Stop();
                fadeAudio = true;
            }
            else
            {
                if (bgmAudio0.isPlaying)
                    bgmAudio0.Stop();
                if (bgmAudio0.clip != null)
                    bgmAudio0.clip = null;

                if (bgmAudio1.isPlaying)
                    bgmAudio1.Stop();
                if (bgmAudio1.clip != null)
                    bgmAudio1.clip = null;
            }
        }

        ///<summary>
        ///Stop Target Audio
        ///</summary>
        public void StopAudio(GameObject targetObject)
        {
            if (targetObject == null)
            {
                Debug.LogError("Sound TargetObject is null");
                return;
            }

            AudioSource audio = targetObject.GetComponent<AudioSource>();
            audio.Stop();
        }

        AudioClip GetAudioClip(string tag)
        {
            if (!soundDic.ContainsKey(tag))
            {
                //Resources 경로 설정
                SoundTable.SoundInfo soundInfo = soundTable.GetSoundInfo(tag);
                if (soundInfo == null)
                {
                    Debug.LogError("Not found SoundInfo : " + tag);
                    return null;
                }

                //AssetBundle 에서 오디오클립 로드
                AudioClip newClip = ResourceManager.LoadAsset<AudioClip>(soundInfo.path, soundInfo.tag, resLinkType);

                if (newClip == null)
                {
                    Debug.LogError("Not found Audioclip : " + tag);
                    return null;
                }

                //Table 에 추가
                soundDic.Add(tag, newClip);
            }

            return soundDic[tag];
        }
    }
}