using UnityEngine;
namespace AiSoundDetect
{
    [AddComponentMenu("AiSoundDetect/Sound Emitter Manager")]
	public class SoundEmitterManager : MonoBehaviour
    {
	    public AudioSource[] waveSources;
	    public AudioSource[] clipSources;
	    public int waveIndex;
	    public int clipIndex;
        
        [SerializeField]private SO_SoundManagerContainer SoundManagerContainer; 
	    void Start()
	    {
	    	SoundManagerContainer.SoundManager = this;
	    }
        public void SetWaveIndex()
        {
            if (waveIndex >= waveSources.Length - 1) waveIndex = 0;

        }
	    public void SetClipIndex()
	    {
		    if (clipIndex >= clipSources.Length - 1) clipIndex = 0;

	    }
    }
}/*
using UnityEngine;
namespace AiSoundDetect
{
    [AddComponentMenu("AiSoundDetect/Sound Emitter Manager")]
    public class SoundEmitterManager : MonoBehaviour
    {
        public AudioSource[] waveSources;
        public AudioSource[] clipSources;
        public int waveIndex = 0;
        public int clipIndex = 0;

        [SerializeField] private SO_SoundManagerContainer SoundManagerContainer;
        [SerializeField] private int numberOfWaveSources = 5; // 생성할 waveSources 개수
        [SerializeField] private int numberOfClipSources = 5; // 생성할 clipSources 개수

        void Awake()
        {
            SoundManagerContainer.SoundManager = this;
            InitializeAudioSources();
        }

        void InitializeAudioSources()
        {
            waveSources = new AudioSource[numberOfWaveSources];
            for (int i = 0; i < numberOfWaveSources; i++)
            {
                GameObject go = new GameObject("WaveSource_" + i);
                go.transform.parent = transform;
                waveSources[i] = go.AddComponent<AudioSource>();
                // 필요에 따라 AudioSource 설정 (volume, spatialBlend 등)
            }

            clipSources = new AudioSource[numberOfClipSources];
            for (int i = 0; i < numberOfClipSources; i++)
            {
                GameObject go = new GameObject("ClipSource_" + i);
                go.transform.parent = transform;
                clipSources[i] = go.AddComponent<AudioSource>();
                // 필요에 따라 AudioSource 설정
            }
        }

        public void SetWaveIndex()
        {
            waveIndex++;
            if (waveIndex >= waveSources.Length) waveIndex = 0;
        }

        public void SetClipIndex()
        {
            clipIndex++;
            if (clipIndex >= clipSources.Length) clipIndex = 0;
        }
    }
}*/