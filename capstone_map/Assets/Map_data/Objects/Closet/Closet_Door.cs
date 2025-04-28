using UnityEngine;

public class Closet_Door : Interactable
{
    Animator animator;
    public AudioSource audioS;
    public AudioClip open;
    public AudioClip close;

    public bool GetAudioCheck()
    {
        return audioS != null && open != null && close != null;
    }

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
        if (animator != null)
        {
            if (!animator.GetBool("hiding"))
            {
                animator.SetBool("open", !animator.GetBool("open"));

                if (GetAudioCheck())
                {
                    if (animator.GetBool("open")) audioS.PlayOneShot(open);
                    else audioS.PlayOneShot(close);
                }
            }
        }
    }
}
