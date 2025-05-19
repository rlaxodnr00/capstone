using UnityEngine;

public class fuse_case : Interactable, IInteractable
{
    private Animator animator;
    public BoxCollider handleCollider;

    public AudioSource audioSource;
    public AudioClip clip;
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

        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    
    public void Interact()
    {
        OnInteract();
    }
}
