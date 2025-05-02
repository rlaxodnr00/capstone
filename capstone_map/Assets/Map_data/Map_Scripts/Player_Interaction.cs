using UnityEngine;

public class Player_Interaction : MonoBehaviour
{
    public float interactionDistance = 3f; // ��ȣ�ۿ� ������ �Ÿ�
    // public LayerMask interactableLayer; // ��ȣ�ۿ� ������ ������Ʈ�� ���̾�

    private Camera playerCamera; // �÷��̾� ī�޶�
    private Interactable currentInteractable; // ���� ������ ��ȣ�ۿ� ������ ������Ʈ
    public GameObject interactionUI;
    public float interactionMaxCooldown = 0.5f;
    private float interactionCooldown = 0f;

    void Start()
    {
        playerCamera = Camera.main; // ���� ī�޶� ������
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }

    void Update()
    {
        if (currentInteractable != null && false)
        {
            Debug.Log(currentInteractable.name);
        }

        CheckForInteractable(); // ��ȣ�ۿ� ������ ������Ʈ ����
        HandleInteraction(); // �÷��̾� �Է� ó��

        if (interactionCooldown > 0f)
        {
            interactionCooldown -= Time.deltaTime;
        }
    }

    void CheckForInteractable()
    {
        // �÷��̾� ī�޶󿡼� ���� �������� ���� ����
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // ��� ���̾ ������� ����ĳ��Ʈ ����
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            GameObject hitObject = hit.collider.gameObject;
            int hitLayer = hitObject.layer;

            // ��ȣ�ۿ� ���̾ �ƴ� ���̾����� Ȯ��
            if (hitLayer != LayerMask.NameToLayer("Interactable"))
            {
                // ��ȣ�ۿ� �Ұ� ó��
                if (currentInteractable != null)
                {
                    currentInteractable.OnLookAway();
                    currentInteractable = null;
                    if (interactionUI != null)
                    {
                        interactionUI.SetActive(false);
                    }
                }
                return;
            }

            // ��ȣ�ۿ� ������Ʈ Ȯ��
            Interactable interactable = hitObject.GetComponent<Interactable>();
            if (interactable != null)
            {
                if (interactionCooldown <= 0f)
                {
                    if (interactionUI != null)
                    {
                        interactionUI.SetActive(true);
                    }
                }
                else
                {
                    if (interactionUI != null)
                    {
                        interactionUI.SetActive(false);
                    }
                }

                // ���Ӱ� ������ ������Ʈ��� ���� ó��
                if (currentInteractable != interactable)
                {
                    if (currentInteractable != null)
                        currentInteractable.OnLookAway();

                    currentInteractable = interactable;
                    currentInteractable.OnLookAt();
                }
                return;
            }
        }

        // ������ ������Ʈ�� ������ ���� ������Ʈ���� �ü��� ���ȴٰ� ó��
        if (currentInteractable != null)
        {
            currentInteractable.OnLookAway();
            currentInteractable = null;
            if (interactionUI != null)
            {
                interactionUI.SetActive(false);
            }
        }
    }


    void HandleInteraction()
    {
        // ���� ������ ������Ʈ�� �ְ�, ��ȣ�ۿ� Ű (Input Manager���� ����)�� ������ �� ��ȣ�ۿ� ����
        if (currentInteractable != null && Input.GetButtonDown("Interact") && interactionCooldown <= 0f)
        {
            currentInteractable.OnInteract();
            interactionCooldown = interactionMaxCooldown;
        }
    }
}
