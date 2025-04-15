using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableItem : MonoBehaviour, IInteractable
{
    [Header("이 아이템을 인벤토리에 전달할 대상")]
    public PlayerInventory playerInventory;

    public void Interact()
    {
        // 인벤토리가 연결되지 않았다면 경고 출력
        if (playerInventory == null)
        {
            Debug.LogWarning($"[InteractableItem] PlayerInventory가 연결되지 않았습니다: {gameObject.name}");
            return;
        }

        // 인벤토리에 아이템 전달
        playerInventory.TryPickup(gameObject);
    }
}
