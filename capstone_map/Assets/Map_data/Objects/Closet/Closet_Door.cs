using UnityEngine;

public class Closet_Door : Interactable
{
    Animator animator;
    public AudioSource audio;
    public AudioClip open;
    public AudioClip close;

    public bool GetAudioCheck()
    {
        return audio != null && open != null && close != null;
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
                    if (animator.GetBool("open")) audio.PlayOneShot(open);
                    else audio.PlayOneShot(close);
                }
            }
        }
    }
}
