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
        // ������ ȹ�� �Լ�
        ItemSpawnPoint spawnPoint = GetComponentInParent<ItemSpawnPoint>();
        if (spawnPoint != null)
        {
            spawnPoint.Item = false;
        }

        Destroy(gameObject);
    }
}
