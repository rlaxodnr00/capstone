/*
 * using UnityEngine;

public class Player_Inventory : MonoBehaviour
{
    public Transform holdPoint; // �������� �� ��ġ
    public float throwForce = 10f;
    public ThrowableItem[] inventory = new ThrowableItem[2];
    private int currentSlot = 0;

    void Update()
    {
        // ������ (��Ŭ��)
        if (Input.GetMouseButtonDown(0) && inventory[currentSlot] != null)
        {
            ThrowItem(currentSlot);
        }

        // �κ��丮 ��ȯ (��: QŰ)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentSlot = (currentSlot + 1) % inventory.Length;
        }
    }

    public bool PickupItem(ThrowableItem item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = item;
                item.isHeld = true;
                item.transform.SetParent(holdPoint);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
                item.SetPhysics(false);
                return true;
            }
        }
        return false; // �κ��丮 ����
    }

    void ThrowItem(int slot)
    {
        ThrowableItem item = inventory[slot];
        if (item == null) return;

        item.transform.SetParent(null);
        item.SetPhysics(true);
        Rigidbody rb = item.GetComponent<Rigidbody>();

        // �������� �� ����
        Vector3 direction = transform.forward + Vector3.up * 0.5f;
        rb.mass = item.weight;
        rb.AddForce(direction * throwForce * item.weight, ForceMode.Impulse);

        item.isHeld = false;
        inventory[slot] = null;
    }
}*/