using UnityEngine;

public class LobbyLock : MonoBehaviour
{
    public Door[] lobbyDoors;

    void Start()
    {
        foreach (Door o in lobbyDoors)
        {
            o.Doorlock(true);
        }
    }
}
