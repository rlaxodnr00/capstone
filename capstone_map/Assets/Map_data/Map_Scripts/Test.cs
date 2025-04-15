using UnityEngine;

public class Test : MonoBehaviour
{
    GameObject spawn;
    bool check;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!check)
        {
            if (spawn == null)
            {
                spawn = GameObject.Find("Spawn");
            }
            transform.position = spawn.transform.position;
            check = true;
        }
    }
}
