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
        // 문이 잠겼을 때 (마스크가 있는지 확인하는 코드 필요,
        // 문이 가까이 있어서 마스크 없이 탈출하는 경우 방지)
        if (animator.GetBool("locked") || isMaskHeld(inven.heldItems ,inven.CurrentSlot))
        {
            // 

            // 문을 연다.
            animator.SetBool("locked", false);
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
