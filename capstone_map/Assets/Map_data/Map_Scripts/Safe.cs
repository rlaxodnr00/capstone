using UnityEngine;

public class Safe : Interactable, IInteractable
{
    Animator animator;
    AudioSource audioS;
    public AudioClip clip;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        audioS = GetComponentInParent<AudioSource>();
    }

    public override void OnLookAt() { }

    public override void OnLookAway() { }

    public override void OnInteract()
    {
        // 문이 열려있거나 잠기지 않았을 때
        if (animator.GetBool("open") || !animator.GetBool("locked"))
        {
            animator.SetBool("open", !animator.GetBool("open"));
        }

        // 효과음이 있으면 재생
        if (audioS != null && clip != null)
        {
            audioS.PlayOneShot(clip);
        }

    }

    public void Interact()
    {
        OnInteract();
    }
}
