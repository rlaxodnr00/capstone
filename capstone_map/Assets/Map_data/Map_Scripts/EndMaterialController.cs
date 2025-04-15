using UnityEngine;

public class EndMaterialController : MonoBehaviour
{
    public Material normalMaterial;
    public Material basementMaterial;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        float y = transform.position.y;

        if (y < 0f)
        {
            renderer.material = basementMaterial;
        }
        else
        {
            renderer.material = normalMaterial;
        }
    }
}
