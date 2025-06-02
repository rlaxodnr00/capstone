using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ItemDropSound : MonoBehaviour
{
    [Header("소리 설정")]
    public AudioSource impactAudioSource;     // 충돌 시 사용할 오디오 소스 (외부에서 할당)
    public float minImpactVelocity = 1.5f;    // 소리 재생을 위한 최소 충돌 속도
    public float cooldownTime = 0.2f;         // 연속 충돌 방지를 위한 쿨타임

    private Rigidbody rb;
    private float lastSoundTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (impactAudioSource == null)
        {
            Debug.LogWarning($"[{gameObject.name}] impactAudioSource가 할당되지 않았습니다.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 오디오 소스가 없으면 재생 안 함
        if (impactAudioSource == null)
            return;

        // 너무 짧은 시간 간격이면 무시
        if (Time.time - lastSoundTime < cooldownTime)
            return;

        // 충돌 속도가 충분히 크면 소리 재생
        if (collision.relativeVelocity.magnitude >= minImpactVelocity)
        {
            if (!impactAudioSource.isPlaying) // 중복 재생 방지
            {
                impactAudioSource.Play();
                lastSoundTime = Time.time;
            }
        }
    }
}
