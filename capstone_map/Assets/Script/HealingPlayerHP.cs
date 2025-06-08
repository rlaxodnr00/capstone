using UnityEngine;

public class HealingPlayerHP : MonoBehaviour, IInventoryInteractable
{

    [SerializeField] private float healAmount = 20f; // 인스펙터 창에서 자신의 회복량 설정(기본 설정 20f)
    public void InventoryInteract(PlayerInventory inventory)
    {
        HPController hpController = inventory.GetComponentInParent <HPController> ();
        if(hpController != null)
        {
            hpController.TakeHeal(healAmount);
            Destroy(gameObject); // 아이템 삭제
            inventory.ClearSlot();
        }
        else
        {
            Debug.LogWarning("HPController 찾지 못함");
        }
    }
}
