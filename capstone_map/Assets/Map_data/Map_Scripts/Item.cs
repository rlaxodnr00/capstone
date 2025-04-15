using UnityEngine;

public class Item : Interactable
{
    public override void OnLookAt()
    {
        
    }

    public override void OnLookAway()
    {
        
    }

    public override void OnInteract()
    {
        // æ∆¿Ã≈€ »πµÊ «‘ºˆ
        ItemSpawnPoint spawnPoint = GetComponentInParent<ItemSpawnPoint>();
        if (spawnPoint != null)
        {
            spawnPoint.Item = false;
        }

        Destroy(gameObject);
    }
}
