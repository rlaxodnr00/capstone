using UnityEngine;

public class Closet_Door : Interactable
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnLookAt()
    {
        
    }

    public override void OnLookAway()
    {
        
    }
    public override void OnInteract()
    {
        base.OnInteract();
        if (animator != null)
        {
            animator.SetBool("open", !animator.GetBool("open"));
        }
    }
}
