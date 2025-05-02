using UnityEngine;

public class Door : Interactable
{
    private Animator animator;

    public AudioSource audioS;
    public AudioClip open;
    public AudioClip close;
    public AudioClip locked;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Doorlock(bool locked)
    {
        animator.SetBool("locked", locked);
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

        // �����ִ� ���� �ݰų�, ���� ����� �ʾ��� ��
        if (animator.GetBool("open") || !animator.GetBool("locked"))
        {
            animator.SetBool("open", !animator.GetBool("open"));
        }
        
        // �Ҹ� ���
        if (audioS != null && open != null && close != null && locked != null)
        {
            if (animator.GetBool("locked")) audioS.PlayOneShot(locked);
            else if (animator.GetBool("open")) audioS.PlayOneShot(open);
            else audioS.PlayOneShot(close);
        }
    }
}
