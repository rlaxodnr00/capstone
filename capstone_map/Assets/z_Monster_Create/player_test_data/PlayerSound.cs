using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource audioSource;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click - 재생 시도");
            audioSource.PlayOneShot(audioSource.clip); // 중복 재생 가능
        }
    }
}