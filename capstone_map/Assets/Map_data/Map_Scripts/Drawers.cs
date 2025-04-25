using System.Net.Sockets;
using UnityEngine;

public class Drawers : Interactable
{
    private Animator animator;
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

        if (audio != null && clip != null)
        {
            audio.PlayOneShot(clip, 0.3f);
        }
    }
}
