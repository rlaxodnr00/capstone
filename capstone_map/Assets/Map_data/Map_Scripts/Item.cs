using UnityEngine;

public class Item : Interactable
{
    public GameObject interactionUI;
    private void Start()
    {
        if (interactionUI == null)
        {
            interactionUI = GameObject.FindWithTag("InteractionUI");
        }
    }
    public override void OnLookAt()
    {
        
    }

    public override void OnLookAway()
    {
        
    }

    public override void OnInteract()
    {
        // 아이템 획득 함수
        ItemSpawnPoint spawnPoint = GetComponentInParent<ItemSpawnPoint>();
        if (spawnPoint != null)
        {
            spawnPoint.Item = false;
        }
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }

        Destroy(gameObject);
    }
}
