using UnityEngine;

public class Memo : Interactable, IInteractable
{
    // 메모장 번호
    public int memoNumber;
    // 메모장 UI
    // 메모장 텍스트 등

    public override void OnLookAt()
    {

    }

    public override void OnLookAway()
    {
        // 메모장을 열어 두고 시선을 돌리면 메모장이 닫힘
    }

    public override void OnInteract()
    {
        // 메모장 열기
        Debug.Log("메모장 상호작용");
    }
    
    public void Interact()
    {
        OnInteract();
    }
}
