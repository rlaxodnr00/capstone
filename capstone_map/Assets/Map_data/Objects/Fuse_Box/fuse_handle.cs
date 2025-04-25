using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class fuse_handle : Interactable
{
    private Animator animator;
    private Breaker breaker;

    public AudioSource audio;

    private void Start()
    {
        animator = GetComponent<Animator>();
        breaker = GetComponent<Breaker>();
    }

    public override void OnLookAt()
    {
        
    }

    public override void OnLookAway()
    {
        
    }

    public override void OnInteract()
    {
        animator.SetBool("shutdown", !animator.GetBool("shutdown"));
        if (animator.GetBool("shutdown"))
        {
            breaker.TurnOffBreaker();
        } else
        {
            breaker.TurnOnBreaker();
        }

        if (audio != null)
        {
            audio.Play();
            StartCoroutine(StopAudio(2f));
        }
    }

    IEnumerator StopAudio(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        audio.Stop();
    }
}
