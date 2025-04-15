using UnityEngine;

public class Door : Interactable
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public override void OnLookAt()
    {
        // base.OnLookAt();
        // Debug.Log("Door Look");
    }

    public override void OnLookAway()
    {
        // base.OnLookAway();
        // Debug.Log("Door Lookaway");
    }

    public override void OnInteract()
    {
        // base.OnInteract();
        // Debug.Log("Door Interacted");

        // 열려있는 문을 닫거나, 문이 잠기지 않았을 때
        if (animator.GetBool("open") || !animator.GetBool("locked"))
        {
            animator.SetBool("open", !animator.GetBool("open"));
        }
    }
}
