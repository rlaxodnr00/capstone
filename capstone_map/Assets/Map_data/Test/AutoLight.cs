using UnityEngine;

public class AutoLight : MonoBehaviour
{
    public GameObject map_root;
    private Transform root_tf;
    Light light_setting;
    float intensity;
    float range;
    float root_scale;
    void Start()
    {
        light_setting = GetComponent<Light>();
        intensity = light_setting.intensity;
        range = light_setting.range;

        if (map_root == null)
        {
            map_root = GameObject.Find("Generated Map");
            root_tf = map_root.GetComponent<Transform>();
        }

        if (map_root != null)
        {
            root_scale = (root_tf.localScale.x + root_tf.localScale.y + root_tf.localScale.z) / 3;

            light_setting.range *= root_scale;
        }
    }
}
