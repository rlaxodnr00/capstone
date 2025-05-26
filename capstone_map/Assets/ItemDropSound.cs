using UnityEngine;
using AiSoundDetect; // Sound_Emitter를 사용하기 위해 네임스페이스 추가

public class ItemDropSound : MonoBehaviour
{
    public AudioClip[] dropSounds; // 재생할 사운드 클립 배열

    private Sound_Emitter soundEmitter; // 아이템의 자식 오브젝트에 있는 Sound_Emitter 참조
    private AudioSource audioSource;   // SoundEmitter의 자식 오브젝트에 있는 AudioSource 참조
    private bool hasLanded = false;    // 착지 여부를 확인하는 변수

    void Awake()
    {
        // 자식 오브젝트 "SoundEmitter"를 찾고, 그 안에서 Sound_Emitter 컴포넌트와 AudioSource 컴포넌트를 찾습니다.
        Transform soundEmitterTransform = transform.Find("SoundEmitter");
        if (soundEmitterTransform != null)
        {
            soundEmitter = soundEmitterTransform.GetComponent<Sound_Emitter>();
            audioSource = soundEmitterTransform.GetComponentInChildren<AudioSource>();
        }

        // Sound_Emitter나 AudioSource가 없으면 경고 메시지 출력
        if (soundEmitter == null)
        {
            Debug.LogWarning("ItemDropSound: No Sound_Emitter found on 'SoundEmitter' child object. Please ensure the hierarchy is correct.", this);
        }
        if (audioSource == null)
        {
            Debug.LogWarning("ItemDropSound: No AudioSource found under 'SoundEmitter' child object. Please ensure the hierarchy is correct.", this);
        }

        // Sound_Emitter의 AudioMethod를 AudioSource로 설정
        if (soundEmitter != null)
        {
            soundEmitter.AudioMethod = Sound_Emitter.audioChoice.AudioSource; // Sound_Emitter가 AudioSource를 사용하도록 설정
            soundEmitter.objectEmitterSource = audioSource; // Sound_Emitter에 AudioSource 할당
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 한 번만 실행되도록 hasLanded 체크 및 사운드 클립이 있는지 확인
        if (!hasLanded && dropSounds != null && dropSounds.Length > 0)
        {
            hasLanded = true;
            PlayRandomDropSound();
        }
    }

    private void PlayRandomDropSound()
    {
        if (soundEmitter != null && audioSource != null)
        {
            // 등록된 사운드 클립 중 랜덤으로 하나 선택
            int randomIndex = Random.Range(0, dropSounds.Length);
            AudioClip selectedClip = dropSounds[randomIndex];

            // 선택된 사운드 클립을 AudioSource에 할당
            audioSource.clip = selectedClip;
            
            // Sound_Emitter를 통해 소리 재생 시작
            soundEmitter.ClipPlay(); // Sound_Emitter의 ClipPlay() 호출
        }
    }
}