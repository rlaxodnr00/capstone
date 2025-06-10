using UnityEngine;
using System.Collections;
public class BaseDoor : Door
{
    GameObject player;
    PlayerInventory inven;

    HPController hp;

    private bool hasCleared = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("WomanWarrior");
        inven = player.GetComponent<PlayerInventory>();

        hp = player.GetComponent<HPController>();

        // 주석 해제 하면 잠긴 상태로 시작
        // animator.SetBool("locked", true);

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

            // 문과 상호작용했을 때 문이 잠기지 않았다면 클리어
            if (!animator.GetBool("locked"))
            {
                //게임 클리어
                hasCleared = true;
                StartCoroutine(DelayThenClear());
            }
        }
    }
    
    // 마스크를 손에 든 상태여야 인지함
    public bool isMaskHeld(GameObject[] inven, int slot)
    {
        // 마스크도 똑같이 만들기
        Mask mask = inven[slot].GetComponent<Mask>();

        if (mask != null) return true;
        return false;
    }

    private IEnumerator DelayThenClear()
    {
        yield return null; // 다음 프레임까지 Animator가 상태 전환할 수 있도록 대기
        hp.CLEAR();
    }
}
