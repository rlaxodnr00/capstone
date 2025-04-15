using UnityEngine;

public class fuse_handle : Interactable
{
    private Animator animator;
    private Breaker breaker;

    private void Start()
    {
        animator = GetComponent<Animator>();
        breaker = GetComponent<Breaker>();
    }

    public override void OnLookAt()
    {
        
    }

    public override void OnLookAway()
    {
        
    }

    public override void OnInteract()
    {
        animator.SetBool("shutdown", !animator.GetBool("shutdown"));
        if (animator.GetBool("shutdown"))
        {
            breaker.TurnOffBreaker();
        } else
        {
            breaker.TurnOnBreaker();
        }
    }
}
