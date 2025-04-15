using UnityEngine;

public class fuse_case : Interactable
{
    private Animator animator;
    public BoxCollider handleCollider;
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
        handleCollider.enabled = animator.GetBool("open");
    }
}
