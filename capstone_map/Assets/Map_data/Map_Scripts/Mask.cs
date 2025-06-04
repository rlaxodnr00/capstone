using UnityEngine;

public class Mask : MonoBehaviour, IInventoryInteractable, ICustomDrop
{
    public bool isEquipped { get; private set; } = false; // 마스크 착용 여부 저장

    public void InventoryInteract(PlayerInventory inventory)
    {

        isEquipped = !isEquipped; // 상태 전환

        if (isEquipped)
        {
            GameUIManager.Instance.EnableGasMaskOverlay(); //가스마스크 착용 시
            Debug.Log("가스마스크 착용");
        }
        else
        {
            GameUIManager.Instance.DisableGasMaskOverlay(); //가스마스크 해제 시
            Debug.Log("가스마스크 해제");
        }
    }

    public void CustomDrop() //사용 중 드롭 시 기능 해제
    {
        if (isEquipped)
        {
            GameUIManager.Instance.DisableGasMaskOverlay();
            isEquipped = false; //드롭 시 미착용 상태로 전환
        }
    }
}
