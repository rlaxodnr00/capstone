using UnityEngine;

public class Safe : Interactable
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
        // ���� �����ְų� ����� �ʾ��� ��
        if (animator.GetBool("open") || !animator.GetBool("locked"))
        {
            animator.SetBool("open", !animator.GetBool("open"));
        }

        // ȿ������ ������ ���
        if (audioS != null && clip != null)
        {
            audioS.PlayOneShot(clip);
        }

    }
}
