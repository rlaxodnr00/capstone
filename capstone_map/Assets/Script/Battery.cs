using UnityEngine;

public class Battery : MonoBehaviour, IInventoryInteractable
{
    //public float chargeAmount = 50f; // 배터리 1개당 충전량

    //인벤토리 내에서 F키로 상호작용할 때 호출
    public void InventoryInteract(PlayerInventory inventory)
    {
        foreach (var item in inventory.heldItems)
        {
            if (item != null)
            {
                Flashlight flashLight = item.GetComponent<Flashlight>();
                if (flashLight != null)
                {
                    flashLight.ChargeBattery(); // 손전등 배터리 충전
                    Debug.Log("배터리 충전.");
                    Destroy(gameObject); // 배터리 아이템 삭제
                    return;
                }
                else
                {
                    Debug.LogWarning("손전등을 찾을 수 없습니다.");
                }
            }
        }
    }
}
