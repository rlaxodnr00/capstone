using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("����ũ �⺻ ����")]
    public float shakeDuration = 0.4f;   // ��鸮�� �ð�
    public float shakeMagnitude = 0.1f;  // ��鸲 ����
    public float dampingSpeed = 3.0f;    // ���� �ӵ�
    

    private Vector3 initialPosition;


    void OnEnable()
    {
        // ���� ��ġ ����
        initialPosition = transform.localPosition;
    }

    // �ܺο��� ȣ���ϴ� ��鸲 ���� �Լ�
    public void TriggerShake()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0.0f;
        float updateRate = 0.02f; //��鸲 �ӵ� ����
    WaitForSeconds wait = new WaitForSeconds(updateRate);

        while (elapsed < shakeDuration)
        {
            // ������ ��ġ ��� (���� ���� ��)
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = initialPosition + randomOffset;

            elapsed += Time.deltaTime * dampingSpeed;
            yield return wait;
        }

        // ���� ��ġ�� ����
        transform.localPosition = initialPosition;
    }
}
