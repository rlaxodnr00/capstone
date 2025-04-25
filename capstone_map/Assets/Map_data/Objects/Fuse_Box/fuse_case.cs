using UnityEngine;

public class fuse_case : Interactable
{
    private Animator animator;
    public BoxCollider handleCollider;

    public AudioSource audio;
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

        if (audio != null && clip != null)
        {
            audio.PlayOneShot(clip);
        }
    }
}
