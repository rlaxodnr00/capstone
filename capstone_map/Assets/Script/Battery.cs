/*
using UnityEngine;

public class Battery : MonoBehaviour, IInteractable
{
    //public float chargeAmount = 50f; // ���͸� 1���� ������

    public void Interact()
    {
        Flashlight flashlight = FindObjectOfType<Flashlight>(); // �� ���� �������� ã��

        if (flashlight != null && flashlight.HasFlashlight)
        {
            flashlight.ChargeBattery(); // ������ ���͸� ����
            Debug.Log("���͸� ȹ��! ������: ");
            Destroy(gameObject); // ���͸� ������ ����
        }
        else
        {
            Debug.LogWarning("�������� ã�� �� �����ϴ�.");
        }
    }
}
*/