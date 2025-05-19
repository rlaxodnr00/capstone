using UnityEngine;

public class Item : Interactable, IInteractable
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
        // 아이템 생성 위치에 있는 아이템일 때,
        ItemSpawnPoint spawnPoint = GetComponentInParent<ItemSpawnPoint>();
        if (spawnPoint != null)
        {
            // 아이템 생성 위치에 아이템이 다시 생길 수 있도록 설정
            spawnPoint.Item = false;
        }
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }

        Debug.Log("아이템 획득");
        // 아이템 삭제
        // Destroy(gameObject);
    }

    public void Interact()
    {
        OnInteract();
    }
}
