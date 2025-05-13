using UnityEngine;

public class Battery : MonoBehaviour, IInventoryInteractable
{
    //public float chargeAmount = 50f; // ���͸� 1���� ������

    //�κ��丮 ������ FŰ�� ��ȣ�ۿ��� �� ȣ��
    public void InventoryInteract(PlayerInventory inventory)
    {
        foreach (var item in inventory.heldItems)
        {
            if (item != null)
            {
                Flashlight flashLight = item.GetComponent<Flashlight>();
                if (flashLight != null)
                {
                    flashLight.ChargeBattery(); // ������ ���͸� ����
                    Debug.Log("���͸� ����.");
                    Destroy(gameObject); // ���͸� ������ ����
                    return;
                }
                else
                {
                    Debug.LogWarning("�������� ã�� �� �����ϴ�.");
                }
            }
        }
    }
}
