/*
 * 충돌 사운드 작성 스크립트 (AudioSource 버전)
 */
using UnityEngine;

public class ImpactSound : MonoBehaviour
{
    [Header("충돌 사운드 세팅")]
    public AudioSource[] impactSources; // 여러 임팩트 사운드 소스 (충돌 강도에 따라 랜덤하게 선택)
    public float minImpactForce = 2f; // 최소 충돌 강도: 이 값 미만이면 소리 재생 X
    public float maxImpactForce = 10f; // 충돌 강도가 이 값 이상이면 최대 볼륨으로 재생
    [Range(0f, 1f)] public float maxVolume = 1f; // 충돌 사운드 최대 볼륨 (0~1, AudioSource 볼륨 곱셈)

    [Header("충돌 사운드 세부 설정")] // <<일단 대충 해두고 나중에 오버라이딩 구조로 변경
    public float highImpactThreshold = 6f; // 특정 충돌 강도 이상일 때 사용할 소스 범위
    public int highImpactStartIndex = 2; // 충돌 강도가 highImpactThreshold 이상일 때만 재생할 소리 조정 위한 변수
    public int highImpactEndIndex = 4; // highImpactEndIndex 포함

    void OnCollisionEnter(Collision collision)
    {
        // 충돌 강도 계산: 상대 속도 크기를 측정
        float impactForce = collision.relativeVelocity.magnitude;

        // 충돌 강도가 너무 약하면 소리 재생 안 함
        if (impactForce < minImpactForce) return;

        // 충돌 강도 정규화 (minImpactForce -> 0, maxImpactForce -> 1)
        float normalizedForce = Mathf.InverseLerp(minImpactForce, maxImpactForce, impactForce);
        float volumeMultiplier = normalizedForce * maxVolume; // 정규화 값 * 최대 볼륨 = 재생 볼륨 배수

        int sourceIndex = 0;
        if (impactForce >= highImpactThreshold && impactSources.Length > 0)
        {
            // 충돌 강도가 높으면 특정 범위(highImpactStartIndex ~ highImpactEndIndex) 내에서 선택
            // highImpactEndIndex를 포함하기 위해 +1로 설정
            int start = Mathf.Clamp(highImpactStartIndex, 0, impactSources.Length - 1);
            int end = Mathf.Clamp(highImpactEndIndex, start, impactSources.Length - 1);
            sourceIndex = Random.Range(start, end + 1);
        }
        else if (impactSources != null && impactSources.Length > 0)
        {
            // 약한 충돌일 경우, 전체 배열에서 랜덤 선택
            sourceIndex = Random.Range(0, impactSources.Length);
        }
        else
        {
            return;
        }

        AudioSource selectedSource = impactSources[sourceIndex];
        if (selectedSource != null && selectedSource.clip != null)
        {
            selectedSource.volume = selectedSource.volume * volumeMultiplier; // AudioSource 자체 볼륨에 곱함
            selectedSource.PlayOneShot(selectedSource.clip);
            selectedSource.volume = selectedSource.volume / volumeMultiplier; // 재생 후 볼륨 원상 복구

            // 디버그 로그로 충돌 세기를 확인
            Debug.Log("충돌 강도: " + impactForce +
                      ", 정규화: " + normalizedForce +
                      ", 재생 볼륨 배수: " + volumeMultiplier +
                      ", 사용된 소스 인덱스: " + sourceIndex +
                      ", 사용된 클립: " + selectedSource.clip.name +
                      ", 최종 재생 볼륨: " + selectedSource.volume * volumeMultiplier);
        }
        else
        {
            Debug.LogWarning("ImpactSound: 선택된 AudioSource가 null이거나 AudioClip이 할당되지 않았습니다.");
        }
    }
}