/*
 * �浹 ���� �ۼ� ��ũ��Ʈ (AudioSource ����)
 */
using UnityEngine;

public class ImpactSound : MonoBehaviour
{
    [Header("�浹 ���� ����")]
    public AudioSource[] impactSources; // ���� ����Ʈ ���� �ҽ� (�浹 ������ ���� �����ϰ� ����)
    public float minImpactForce = 2f; // �ּ� �浹 ����: �� �� �̸��̸� �Ҹ� ��� X
    public float maxImpactForce = 10f; // �浹 ������ �� �� �̻��̸� �ִ� �������� ���
    [Range(0f, 1f)] public float maxVolume = 1f; // �浹 ���� �ִ� ���� (0~1, AudioSource ���� ����)

    [Header("�浹 ���� ���� ����")] // <<�ϴ� ���� �صΰ� ���߿� �������̵� ������ ����
    public float highImpactThreshold = 6f; // Ư�� �浹 ���� �̻��� �� ����� �ҽ� ����
    public int highImpactStartIndex = 2; // �浹 ������ highImpactThreshold �̻��� ���� ����� �Ҹ� ���� ���� ����
    public int highImpactEndIndex = 4; // highImpactEndIndex ����

    void OnCollisionEnter(Collision collision)
    {
        // �浹 ���� ���: ��� �ӵ� ũ�⸦ ����
        float impactForce = collision.relativeVelocity.magnitude;

        // �浹 ������ �ʹ� ���ϸ� �Ҹ� ��� �� ��
        if (impactForce < minImpactForce) return;

        // �浹 ���� ����ȭ (minImpactForce -> 0, maxImpactForce -> 1)
        float normalizedForce = Mathf.InverseLerp(minImpactForce, maxImpactForce, impactForce);
        float volumeMultiplier = normalizedForce * maxVolume; // ����ȭ �� * �ִ� ���� = ��� ���� ���

        int sourceIndex = 0;
        if (impactForce >= highImpactThreshold && impactSources.Length > 0)
        {
            // �浹 ������ ������ Ư�� ����(highImpactStartIndex ~ highImpactEndIndex) ������ ����
            // highImpactEndIndex�� �����ϱ� ���� +1�� ����
            int start = Mathf.Clamp(highImpactStartIndex, 0, impactSources.Length - 1);
            int end = Mathf.Clamp(highImpactEndIndex, start, impactSources.Length - 1);
            sourceIndex = Random.Range(start, end + 1);
        }
        else if (impactSources != null && impactSources.Length > 0)
        {
            // ���� �浹�� ���, ��ü �迭���� ���� ����
            sourceIndex = Random.Range(0, impactSources.Length);
        }
        else
        {
            return;
        }

        AudioSource selectedSource = impactSources[sourceIndex];
        if (selectedSource != null && selectedSource.clip != null)
        {
            selectedSource.volume = selectedSource.volume * volumeMultiplier; // AudioSource ��ü ������ ����
            selectedSource.PlayOneShot(selectedSource.clip);
            selectedSource.volume = selectedSource.volume / volumeMultiplier; // ��� �� ���� ���� ����

            // ����� �α׷� �浹 ���⸦ Ȯ��
            Debug.Log("�浹 ����: " + impactForce +
                      ", ����ȭ: " + normalizedForce +
                      ", ��� ���� ���: " + volumeMultiplier +
                      ", ���� �ҽ� �ε���: " + sourceIndex +
                      ", ���� Ŭ��: " + selectedSource.clip.name +
                      ", ���� ��� ����: " + selectedSource.volume * volumeMultiplier);
        }
        else
        {
            Debug.LogWarning("ImpactSound: ���õ� AudioSource�� null�̰ų� AudioClip�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}