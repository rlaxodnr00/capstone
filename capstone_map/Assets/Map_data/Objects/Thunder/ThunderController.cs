using UnityEngine;
using System.Collections; // 코루틴 사용을 위해 추가

public class ThunderController : MonoBehaviour
{
    private Light[] tdLight; // 자식 Light 컴포넌트 배열
    public float duration = 0.5f;
    // private float timestamp; // 번개 효과 시작 시간 기록 (코루틴으로 대체되어 제거)

    private GameObject map; // "Generated Map" 오브젝트 참조
    private fuse_handle fuse; // fuse_handle 컴포넌트 참조

    [Header("Sound Settings")]
    public AudioClip thunderSound; // 천둥 소리 오디오 클립
    public float soundDelay = 0.3f; // 빛이 번쩍인 후 소리가 나기까지의 지연 시간 (초)
    private AudioSource audioSource; // 천둥 소리를 재생할 AudioSource

    [Header("Cooldown Settings")]
    public float minCooldown = 120f;
    public float maxCooldown = 180f;
    private float nextThundertime;

    private bool isThunderInProgress = false; // 현재 번개 효과가 진행 중인지 여부

    void Start()
    {
        tdLight = GetComponentsInChildren<Light>(true);
        if (tdLight == null || tdLight.Length == 0)
        {
            Debug.LogWarning("ThunderController: 자식 오브젝트에서 Light 컴포넌트를 찾을 수 없습니다.");
        }

        map = GameObject.Find("Generated Map");
        if (map != null)
        {
            fuse = map.GetComponentInChildren<fuse_handle>();
            if (fuse == null)
            {
                Debug.LogWarning("ThunderController: 'Generated Map' 오브젝트 또는 그 자식에서 'fuse_handle' 컴포넌트를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("ThunderController: 'Generated Map' 오브젝트를 찾을 수 없습니다. Fuse 로직이 정상적으로 동작하지 않을 수 있습니다.");
        }

        // AudioSource 컴포넌트 가져오기 또는 추가하기
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.LogWarning("ThunderController: AudioSource 컴포넌트가 없어 새로 추가했습니다. Inspector에서 3D Sound Settings를 설정해주세요 (Spatial Blend, Min/Max Distance 등).");
        }

        if (thunderSound == null)
        {
            Debug.LogWarning("ThunderController: thunderSound AudioClip이 Inspector에 할당되지 않았습니다.");
        }

        // AudioSource 기본 설정 (멀리서 들리는 효과를 위해)
        audioSource.spatialBlend = 1.0f; // 3D 사운드로 설정
        audioSource.minDistance = 50f;   // 이 거리 안에서는 소리가 최대 크기로 들림
        audioSource.maxDistance = 500f;  // 이 거리를 벗어나면 소리가 거의 들리지 않음
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic; // 거리에 따른 소리 감쇠 방식
        audioSource.playOnAwake = false; // 시작 시 자동 재생 방지

        SetNextRandomThunderTime();
    }

    // Thunderstrike 호출시 번개가 침
    public void Thunderstrike()
    {
        if (isThunderInProgress)
        {
            // Debug.Log("번개 효과가 이미 진행 중입니다.");
            return; // 이미 번개 효과가 진행 중이면 중복 실행 방지
        }
        StartCoroutine(ThunderSequenceCoroutine());
    }

    private IEnumerator ThunderSequenceCoroutine()
    {
        isThunderInProgress = true;

        // 1. 모든 이전 조명 끄기 및 무작위 조명 하나 켜기
        DisableThunder(); // 모든 조명을 먼저 끈다
        Light lightToFlash = null;
        if (tdLight != null && tdLight.Length > 0)
        {
            // tdLight 배열에서 무작위로 하나의 Light 컴포넌트를 선택
            int randomIndex = Random.Range(0, tdLight.Length);
            lightToFlash = tdLight[randomIndex];
            if (lightToFlash != null)
            {
                lightToFlash.enabled = true;
                // Debug.Log($"Thunderstrike: Light '{tdLight[randomIndex].gameObject.name}' at index {randomIndex} activated.");
            }
        }
        else
        {
            // Debug.LogWarning("ThunderController: 활성화할 Light가 없습니다.");
        }

        // 2. 퓨즈 상호작용 (빛과 동시에 발생)
        if (fuse != null)
        {
            if (!fuse.GetShutdown()) fuse.OnInteract();
        }
        else
        {
            Debug.LogWarning("ThunderController: 'fuse_handle'이 할당되지 않아 상호작용할 수 없습니다.");
        }

        // 3. 설정된 soundDelay만큼 대기
        yield return new WaitForSeconds(soundDelay);

        // 4. 천둥 소리 재생
        if (audioSource != null && thunderSound != null)
        {
            audioSource.PlayOneShot(thunderSound);
        }

        // 5. 빛이 켜진 총 시간(duration)에서 이미 기다린 soundDelay를 제외한 나머지 시간만큼 대기
        //    이렇게 함으로써 빛은 총 'duration'만큼 켜져 있게 됨
        float remainingLightDuration = duration - soundDelay;
        if (remainingLightDuration > 0)
        {
            yield return new WaitForSeconds(remainingLightDuration);
        }

        // 6. 켜졌던 조명 끄기
        if (lightToFlash != null)
        {
            lightToFlash.enabled = false;
        }

        SetNextRandomThunderTime();
        isThunderInProgress = false;
    }

    public void DisableThunder()
    {
        if (tdLight == null) return;

        foreach (Light light in tdLight) // 모든 tdLight 조명 비활성화
        {
            if (light != null)
            {
                light.enabled = false;
            }
        }
    }

    // 테스트를 위해 Update 메서드 추가
    void Update()
    {
        // 'T' 키를 누르면 번개 효과 발생
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     Debug.Log("Test key 'T' pressed. Triggering Thunderstrike.");
        //     Thunderstrike();
        // }

        if (Time.time >= nextThundertime && !isThunderInProgress)
        {
            Thunderstrike();
        }
    }

    private void SetNextRandomThunderTime()
    {
        float randomCooldown = Random.Range(minCooldown, maxCooldown);
        nextThundertime = Time.time + randomCooldown;
    }
}
