using UnityEngine;

public class Drawers : Interactable
{
    private Animator animator;

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
        animator.SetBool("open", !animator.GetBool("open"));
    }
}
