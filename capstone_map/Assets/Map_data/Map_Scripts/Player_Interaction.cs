using UnityEngine;

public class Player_Interaction : MonoBehaviour
{
    public float interactionDistance = 3f; // 상호작용 가능한 거리
    public LayerMask interactableLayer; // 상호작용 가능한 오브젝트의 레이어

    private Camera playerCamera; // 플레이어 카메라
    private Interactable currentInteractable; // 현재 감지된 상호작용 가능한 오브젝트
    public GameObject interactionUI;
    public float interactionMaxCooldown = 0.5f;
    private float interactionCooldown = 0f;

    void Start()
    {
        playerCamera = Camera.main; // 메인 카메라를 가져옴
        interactionUI.SetActive(false);
    }

    void Update()
    {
        if (currentInteractable != null && false)
        {
            Debug.Log(currentInteractable.name);
        }

        CheckForInteractable(); // 상호작용 가능한 오브젝트 감지
        HandleInteraction(); // 플레이어 입력 처리

        if (interactionCooldown > 0f)
        {
            interactionCooldown -= Time.deltaTime;
        }
    }

    void CheckForInteractable()
    {
        // 플레이어 카메라에서 정면 방향으로 레이 생성
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // 모든 레이어를 대상으로 레이캐스트 실행
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            GameObject hitObject = hit.collider.gameObject;
            int hitLayer = hitObject.layer;

            // 벽 같은 장애물에 막혔는지 확인 (예: "Wall" 레이어)
            if (hitLayer == LayerMask.NameToLayer("Wall"))
            {
                // 벽을 맞았으므로 상호작용 불가 처리
                if (currentInteractable != null)
                {
                    currentInteractable.OnLookAway();
                    currentInteractable = null;
                    interactionUI.SetActive(false);
                }
                return;
            }

            // 상호작용 가능한 오브젝트인지 확인
            Interactable interactable = hitObject.GetComponent<Interactable>();
            if (interactable != null)
            {
                if (interactionCooldown <= 0f)
                {
                    interactionUI.SetActive(true);
                }
                else
                {
                    interactionUI.SetActive(false);
                }

                // 새롭게 감지된 오브젝트라면 변경 처리
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

        // 감지된 오브젝트가 없으면 기존 오브젝트에서 시선을 돌렸다고 처리
        if (currentInteractable != null)
        {
            currentInteractable.OnLookAway();
            currentInteractable = null;
            interactionUI.SetActive(false);
        }
    }


    void HandleInteraction()
    {
        // 현재 감지된 오브젝트가 있고, 상호작용 키를 눌렀을 때 상호작용 실행
        if (currentInteractable != null && Input.GetButtonDown("Interact") && interactionCooldown <= 0f)
        {
            currentInteractable.OnInteract();
            interactionCooldown = interactionMaxCooldown;
        }
    }
}
