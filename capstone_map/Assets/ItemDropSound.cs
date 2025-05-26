using UnityEngine;
using AiSoundDetect; // Sound_Emitter를 사용하기 위해 네임스페이스 추가
using System.Collections.Generic; // List를 사용하려면 이 네임스페이스가 필요합니다.

[RequireComponent(typeof(Sound_Emitter))]
public class ItemDropSound : MonoBehaviour
{
    public Sound_Emitter soundEmitter;       // 아이템에 부착된 Sound_Emitter
    public AudioSource[] itemAudioSources;   // ✨ 변경된 부분: 여러 AudioSource를 위한 배열

    // 주의: 만약 각 AudioSource마다 재생할 특정 AudioClip이 미리 정해져 있다면,
    // 이 스크립트에서 AudioClip을 별도로 받을 필요는 없습니다.
    // 기존 AudioSource 컴포넌트에 할당된 AudioClip이 자동으로 재생됩니다.

    private bool hasLanded = false;          // 착지 여부를 확인하는 변수

    private void Awake()
    {
        // Sound_Emitter가 없으면 자동으로 추가
        if (soundEmitter == null)
        {
            soundEmitter = gameObject.AddComponent<Sound_Emitter>();
            soundEmitter.AudioMethod = Sound_Emitter.audioChoice.AudioSource; // AudioSource 모드로 설정
        }

        // ✨ 변경된 부분: itemAudioSources 배열이 비어있으면,
        // 현재 오브젝트에 있는 모든 AudioSource를 자동으로 가져옵니다.
        // 이 부분은 유니티 에디터에서 직접 할당하는 경우를 대비해 유연하게 처리합니다.
        if (itemAudioSources == null || itemAudioSources.Length == 0)
        {
            itemAudioSources = GetComponents<AudioSource>(); // 현재 오브젝트에 있는 모든 AudioSource를 가져옴

            // 만약 그래도 AudioSource가 없으면 기본으로 하나 추가
            /*if (itemAudioSources.Length == 0)
            {
                itemAudioSources = new AudioSource[1]; // 배열 크기를 1로 설정
                itemAudioSources[0] = gameObject.AddComponent<AudioSource>();
                Debug.LogWarning("아이템에 AudioSource가 없어서 하나를 자동으로 추가했습니다.");
            }*/
            if (itemAudioSources.Length == 0)
            {
                itemAudioSources = new AudioSource[1]; // 배열 크기를 1로 설정
                itemAudioSources[0] = gameObject.AddComponent<AudioSource>();
                Debug.LogWarning("아이템에 AudioSource가 없어서 하나를 자동으로 추가했습니다.");
                AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
                newAudioSource.playOnAwake = false; // 새로 추가된 AudioSource의 PlayOnAwake를 false로 설정
                itemAudioSources[0] = newAudioSource;
                Debug.LogWarning(gameObject.name + ": 아이템에 AudioSource가 없어서 하나를 자동으로 추가하고 playOnAwake를 false로 설정했습니다.");
            }
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
        // ✨ 변경된 부분: 여러 AudioSource 중 어떤 것을 사용할지 결정해야 합니다.
        if (itemAudioSources != null && itemAudioSources.Length > 0)
        {
            // 예시 1: 첫 번째 AudioSource를 사용
            // AudioSource audioToPlay = itemAudioSources[0];

            // 예시 2: 무작위 AudioSource를 사용 (더 자연스러운 효과)
            AudioSource audioToPlay = itemAudioSources[Random.Range(0, itemAudioSources.Length)];

            if (audioToPlay != null)
            {
                // 사운드 재생
                soundEmitter.objectEmitterSource = audioToPlay; // SoundEmitter에 선택된 AudioSource 할당
                audioToPlay.Play(); // 선택된 AudioSource 재생
            }
            else
            {
                Debug.LogWarning("선택된 AudioSource가 null입니다. 확인해주세요.");
            }
        }
        else
        {
            Debug.LogWarning("할당된 AudioSource가 없습니다. ItemDropSound 스크립트를 확인해주세요.");
        }
    }
}