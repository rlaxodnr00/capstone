using UnityEngine;

public class LobbyDoor : Door
{
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public LobbyDoor otherDoor;

    public override void Doorlock(bool locked)
    {
        base.Doorlock(locked);
    }

    public override void OnInteract()
    {
        // 문이 잠겼고 열쇠가 있으면
        if (animator.GetBool("locked") && "key" == "true")
        {
            // 열쇠를 소모하고?

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
}
