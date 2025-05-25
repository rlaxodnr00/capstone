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
        }
        else
        {
            Debug.LogWarning("HPController 찾지 못함");
        }
    }
}
