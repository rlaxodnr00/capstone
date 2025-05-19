using UnityEngine;

public class LobbyDoor : Door
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
    public LobbyDoor otherDoor;

    public override void Doorlock(bool locked)
    {
        base.Doorlock(locked);
    }

    public override void OnInteract()
    {
        // 문이 잠겼고 열쇠가 있으면
        if (animator.GetBool("locked") && isKeyHeld(inven.heldItems ,inven.CurrentSlot))
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
        }
    }

    public void InteractWith()
    {
        base.OnInteract();
    }

    public bool isKeyHeld(GameObject[] inven, int slot)
    {
        Key key = inven[slot].GetComponent<Key>();

        if (key != null) return true;
        return false;
    }
}
