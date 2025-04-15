using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 3f; // 플레이어가 상호작용 가능한 최대 거리
    public Crosshair crosshair; // UI 조준선

    void Update()
    {
        HandleInteraction();
    }

    void HandleInteraction()
    {
        // 화면 중앙에서 레이케스트 발사
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // 광선이 특정 거리 이내의 오브젝트와 충돌했는지 검사
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            GameObject hitObject = hit.collider.gameObject;
            IInteractable interactable = hitObject.GetComponentInParent<IInteractable>(); // 부모 포함하여 인터페이스 존재 확인

            if (interactable != null) // 상호작용 가능한 오브젝트일 경우
            {
                crosshair.SetColor(Color.red); //조준선 붉은 색으로
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();

                    // 추가 상호작용이 있는 경우, 추가 인터페이스 탐색
                    ICustomInteractable additional = hitObject.GetComponentInParent<ICustomInteractable>();
                    if (additional != null)
                    {
                        additional.CustomInteractable();
                    }
                }
            }
            else
            {
                crosshair.SetColor(Color.green); //상호작용 안되는 오브젝트는 녹색
            }
        }
        else
        {
            crosshair.SetColor(Color.white); //아무것도 없을 땐 흰색
        }
    }
}
