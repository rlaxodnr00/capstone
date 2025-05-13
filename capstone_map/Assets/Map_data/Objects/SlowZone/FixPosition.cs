using UnityEngine;

public class FixPosition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position += Vector3.up * 0.005f;
    }
}
