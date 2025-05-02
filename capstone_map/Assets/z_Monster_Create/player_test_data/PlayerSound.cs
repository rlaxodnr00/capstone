using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource audioSource;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click - ��� �õ�");
            audioSource.PlayOneShot(audioSource.clip); // �ߺ� ��� ����
        }
    }
}