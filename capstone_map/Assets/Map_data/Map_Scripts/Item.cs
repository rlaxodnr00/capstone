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
        // æ∆¿Ã≈€ »πµÊ «‘ºˆ
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
