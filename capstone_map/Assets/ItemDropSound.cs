using UnityEngine;
using AiSoundDetect; // Sound_Emitter를 사용하기 위해 네임스페이스 추가

public class ItemDropSound : MonoBehaviour
{
    public Sound_Emitter soundEmitter; // 아이템에 부착된 Sound_Emitter
    // public AudioClip dropSound;       // 더 이상 필요 없음
    public AudioSource itemAudioSource; // 아이템에 부착된 AudioSource

    private bool hasLanded = false;   // 착지 여부를 확인하는 변수

    private void Awake()
    {
        // Sound_Emitter가 없으면 자동으로 추가
        if (soundEmitter == null)
        {
            soundEmitter = gameObject.AddComponent<Sound_Emitter>();
            // Sound_Emitter 설정 (필요한 경우)
            soundEmitter.AudioMethod = Sound_Emitter.audioChoice.AudioSource; // AudioSource 모드로 설정
        }

        // AudioSource가 없으면 자동으로 추가
        if (itemAudioSource == null)
        {
            itemAudioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 한 번만 실행되도록 hasLanded 체크
        if (!hasLanded)
        {
            hasLanded = true;
            PlayDropSound();
        }
    }

    private void PlayDropSound()
    {
        // 사운드 재생
        soundEmitter.objectEmitterSource = itemAudioSource; // SoundEmitter에 AudioSource 할당
        itemAudioSource.Play();
    }
}