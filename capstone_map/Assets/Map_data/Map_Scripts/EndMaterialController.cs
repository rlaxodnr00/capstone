using UnityEngine;

public class EndMaterialController : MonoBehaviour
{
    public Material normalMaterial;
    public Material basementMaterial;
    Transform tf;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        tf = GetComponent<Transform>();
        float y = transform.position.y;

        if (y < 0f)
        {
            renderer.material = basementMaterial;
            tf.localScale = new Vector3(1f, 1.17f, 1f);
        }
        else
        {
            renderer.material = normalMaterial;
        }
    }
}
