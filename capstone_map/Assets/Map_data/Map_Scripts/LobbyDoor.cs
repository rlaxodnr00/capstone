using UnityEngine;
using System.Collections;

public class LobbyDoor : Door
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
        animator.SetBool("locked", true);

        hp = player.GetComponent<HPController>();
    }
    public LobbyDoor otherDoor;

    public override void Doorlock(bool locked)
    {
        base.Doorlock(locked);
    }

    public override void OnInteract()
    {
        // 이미 클리어 상태면 무시
        if (hasCleared) return;

        // 문이 잠겼고 열쇠가 있으면
        if (animator.GetBool("locked") && isKeyHeld(inven.heldItems, inven.CurrentSlot))
        {
            // 열쇠를 소모하고?
            Debug.Log("문 열림");
            // 문을 연다.
            animator.SetBool("locked", false);
            // 양쪽을 같이 연다.
            otherDoor.animator.SetBool("locked", false);
        }
        // 문이 잠기지 않았거나 열쇠가 없으면 기존 코드로 동작
        else
        {
            base.OnInteract();
            otherDoor.InteractWith();

            // 문과 상호작용했을 때 문이 잠기지 않았다면 클리어
            if (!animator.GetBool("locked"))
            {
                //게임 클리어
                hasCleared = true;
                // 다른 문도 똑같이 적용
                otherDoor.hasCleared = true;
                StartCoroutine(DelayThenClear());
            }
        }
    }

    public void InteractWith()
    {
        base.OnInteract();
    }

    public bool isKeyHeld(GameObject[] inven, int slot)
    {
        if (inven[slot] == null) return false;
        Key key = inven[slot].GetComponent<Key>();

        if (key != null) return true;
        return false;
    }

    private IEnumerator DelayThenClear()
    {
        yield return null; // 다음 프레임까지 Animator가 상태 전환할 수 있도록 대기
        hp.CLEAR();
    }
}
