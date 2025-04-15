using UnityEngine;

public class Field_Light_Rotate : MonoBehaviour
{
    public float rotateSpeed = 1f;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}
