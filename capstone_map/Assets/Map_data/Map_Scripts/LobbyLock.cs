using UnityEngine;

public class LobbyLock : MonoBehaviour
{
    public Door[] lobbyDoors;

    float timestamp;
    void Start()
    {
        timestamp = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timestamp + 1)
        {
            foreach (Door o in lobbyDoors)
            {
                o.Doorlock(true);
            }
        }
    }
}
