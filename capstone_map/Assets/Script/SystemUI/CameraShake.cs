using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("쉐이크 기본 설정")]
    public float shakeDuration = 0.4f;   // 흔들리는 시간
    public float shakeMagnitude = 0.1f;  // 흔들림 세기
    public float dampingSpeed = 3.0f;    // 감쇠 속도
    

    private Vector3 initialPosition;


    void OnEnable()
    {
        // 시작 위치 저장
        initialPosition = transform.localPosition;
    }

    // 외부에서 호출하는 흔들림 시작 함수
    public void TriggerShake()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0.0f;
        float updateRate = 0.02f; //흔들림 속도 조절
    WaitForSeconds wait = new WaitForSeconds(updateRate);

        while (elapsed < shakeDuration)
        {
            // 임의의 위치 계산 (원형 범위 내)
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = initialPosition + randomOffset;

            elapsed += Time.deltaTime * dampingSpeed;
            yield return wait;
        }

        // 원래 위치로 복귀
        transform.localPosition = initialPosition;
    }
}
