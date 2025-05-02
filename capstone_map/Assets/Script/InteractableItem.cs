using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableItem : MonoBehaviour, IInteractable
{
    [Header("�� �������� �κ��丮�� ������ ���")]
    public PlayerInventory playerInventory;

    private void Start()
    {
        // �κ��丮 ���� (Player �±׸� ���� ������Ʈ���� PlayerInventory ������Ʈ�� ã��)
        if (playerInventory == null)
        {
            playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        }
    }

    public void Interact()
    {
        // �κ��丮�� ������� �ʾҴٸ� ��� ���
        if (playerInventory == null)
        {
            Debug.LogWarning($"[InteractableItem] PlayerInventory�� ������� �ʾҽ��ϴ�: {gameObject.name}");
            return;
        }

        // �κ��丮�� ������ ����
        playerInventory.TryPickup(gameObject);
    }
}
