using UnityEngine;

public class Door : Interactable
{
    private Animator animator;

    public AudioSource audio;
    public AudioClip open;
    public AudioClip close;
    public AudioClip locked;
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
        
        // 소리 재생
        if (audio != null && open != null && close != null && locked != null)
        {
            if (animator.GetBool("locked")) audio.PlayOneShot(locked);
            else if (animator.GetBool("open")) audio.PlayOneShot(open);
            else audio.PlayOneShot(close);
        }
    }
}
