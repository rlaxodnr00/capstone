/*
 * using UnityEngine;

public class Player_Inventory : MonoBehaviour
{
    public Transform holdPoint; // 아이템을 들 위치
    public float throwForce = 10f;
    public ThrowableItem[] inventory = new ThrowableItem[2];
    private int currentSlot = 0;

    void Update()
    {
        // 던지기 (좌클릭)
        if (Input.GetMouseButtonDown(0) && inventory[currentSlot] != null)
        {
            ThrowItem(currentSlot);
        }

        // 인벤토리 전환 (예: Q키)
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
        return false; // 인벤토리 꽉참
    }

    void ThrowItem(int slot)
    {
        ThrowableItem item = inventory[slot];
        if (item == null) return;

        item.transform.SetParent(null);
        item.SetPhysics(true);
        Rigidbody rb = item.GetComponent<Rigidbody>();

        // 현실적인 힘 적용
        Vector3 direction = transform.forward + Vector3.up * 0.5f;
        rb.mass = item.weight;
        rb.AddForce(direction * throwForce * item.weight, ForceMode.Impulse);

        item.isHeld = false;
        inventory[slot] = null;
    }
}*/