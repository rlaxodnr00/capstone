using UnityEngine;

public class BaseDoor : Door
{
    GameObject player;
    PlayerInventory inven;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("WomanWarrior");
        inven = player.GetComponent<PlayerInventory>();
        animator.SetBool("locked", true);

    }

    public override void OnInteract()
    {
        // Debug.Log("문 상태: " + animator.GetBool("locked") + "마스크 상태: "
        //    + isMaskHeld(inven.heldItems, inven.CurrentSlot));
        // 문이 잠겼고 마스크가 있으면
        if (animator.GetBool("locked") && isMaskHeld(inven.heldItems, inven.CurrentSlot))
        {
            Debug.Log("문 열림");
            // 문을 연다.
            animator.SetBool("locked", false);
        }
        // 문이 잠기지 않았거나 마스크가 없으면 기존 코드로 동작
        else
        {
            Debug.Log("기존 코드로 동작");
            base.OnInteract();
        }
    }
    
    public bool isMaskHeld(GameObject[] inven, int slot)
    {
        // 마스크도 똑같이 만들기
        Mask mask = inven[slot].GetComponent<Mask>();

        if (mask != null) return true;
        return false;
    }
}
