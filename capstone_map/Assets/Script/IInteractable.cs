/*
 * 기본 상호작용 << Interact
 * 상호작용 후 인벤토리에 들어가는 아이템 중,
 *  인벤토리 내에서 상호작용 << IInventoryUsable
*/

public interface IInteractable //일반 상호작용
{
    void Interact(); //기본 E키 상호작용
}

public interface ICustomInteractable  //일반 상호작용에 아이템 별 추가 상호작용
{
    void CustomInteractable(); //Interact 상호작용에 추가 기능
}

public interface ICustomDrop // 아이템 드랍 시 아이템 별 추가 상호작용
{
    void CustomDrop();
}

public interface ILightSwitchable //조명 관련 토글 기능 << 지울 예정
{
    void ToggleLight(); // 조명을 켜고 끄는 기능
}

public interface IInventoryInteractable //인벤토리 내부 상호작용
{
    void InventoryInteract(PlayerInventory inventory);       // 기본 F키 상호작용
}



