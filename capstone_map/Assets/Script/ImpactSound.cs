/*
 * �浹 ���� �ۼ� ��ũ��Ʈ
 */
using UnityEngine;

public class ImpactSound : MonoBehaviour
{
    [Header("�浹 ���� ����")]
    public AudioClip[] impactClips; // ���� ����Ʈ ���� Ŭ�� (�浹 ������ ���� �����ϰ� ����)
    public float minImpactForce = 2f; // �ּ� �浹 ����: �� �� �̸��̸� �Ҹ� ��� X
    public float maxImpactForce = 10f; // �浹 ������ �� �� �̻��̸� �ִ� �������� ���
    public float maxVolume = 1f; // �浹 ���� �ִ� ���� (0~1, AudioSource ����)

    [Header("�浹 ���� ���� ����")] // <<�ϴ� ���� �صΰ� ���߿� �������̵� ������ ����
    public float highImpactThreshold = 6f; // Ư�� �浹 ���� �̻��� �� ����� Ŭ�� ����
    public int highImpactStartIndex = 2; // �浹 ������ highImpactThreshold �̻��� ���� ����� �Ҹ� ���� ���� ����
    public int highImpactEndIndex = 4; // highImpactEndIndex ����

    // ���ο��� ����� AudioSource
    private AudioSource audioSource;

    void Start()
    {
        // AudioSource ������Ʈ�� ���ٸ� ��Ÿ�ӿ� �߰�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // �ɼ�: �ݺ����� �� ���� ����ǵ��� ����
        audioSource.loop = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // �浹 ���� ���: ��� �ӵ� ũ�⸦ ����
        float impactForce = collision.relativeVelocity.magnitude;

        // �浹 ������ �ʹ� ���ϸ� �Ҹ� ��� �� ��
        if (impactForce < minImpactForce) return;

        // �浹 ���� ����ȭ (minImpactForce -> 0, maxImpactForce -> 1)
        float normalizedForce = Mathf.InverseLerp(minImpactForce, maxImpactForce, impactForce);
        float volume = normalizedForce * maxVolume; //����ȭ �� * �ִ� ���� = ��� ���� (ex: ����ȭ ���� 0.5�� �Ҹ� ũ�� �������� ����)

        int clipIndex = 0;
        if (impactForce >= highImpactThreshold && impactClips.Length > 0)
        {
            // �浹 ������ ������ Ư�� ����(highImpactStartIndex ~ highImpactEndIndex) ������ ����
            // highImpactEndIndex�� �����ϱ� ���� +1�� ����
            int start = Mathf.Clamp(highImpactStartIndex, 0, impactClips.Length - 1);
            int end = Mathf.Clamp(highImpactEndIndex, start, impactClips.Length - 1);
            clipIndex = Random.Range(start, end + 1);
        }
        else if (impactClips != null && impactClips.Length > 0)
        {
            // ���� �浹�� ���, ��ü �迭���� ���� ����
            clipIndex = Random.Range(0, impactClips.Length);
        }
        else
        {
            return;
        }

        AudioClip clip = impactClips[clipIndex];
        audioSource.PlayOneShot(clip, volume);

        // ����� �α׷� �浹 ���⸦ Ȯ��
        Debug.Log("�浹 ����: " + impactForce +
                  ", ����ȭ: " + normalizedForce +
                  ", ��� ����: " + volume +
                  ", ���� Ŭ�� �ε���: " + clipIndex);
    }
}
