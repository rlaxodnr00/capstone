using UnityEngine;

public class ThunderController : MonoBehaviour
{
    Light[] tdLight;
    public float duration = 0.5f;
    float timestamp;
    GameObject map;
    fuse_handle fuse;
    void Start()
    {
        tdLight = GetComponentsInChildren<Light>();
        // Debug.Log(tdLight.Length);
        map = GameObject.Find("Generated Map");
        fuse = map.GetComponentInChildren<fuse_handle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (fuse == null)
            {
                fuse = map.GetComponentInChildren<fuse_handle>();
            }
            timestamp = Time.time;
            // Debug.Log("Get Key");
            Thunderstrike();
        }
        if (Time.time > timestamp + duration)
        {
            DisableThunder();
        }
    }

    public void Thunderstrike()
    {
        // Debug.Log("In Func");
        foreach (Light light in tdLight)
        {
            light.enabled = true;
        }
        
        // tdLight.enabled = false;

        if (fuse != null)
        {
            if (!fuse.GetShutdown()) fuse.OnInteract();
        } else
        {
            Debug.Log("Fuse Box ¾øÀ½");
        }
    }

    public void DisableThunder()
    {
        foreach (Light light in tdLight)
        {
            light.enabled = false;
        }
    }
}
