using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 3f; // �÷��̾ ��ȣ�ۿ� ������ �ִ� �Ÿ�
    public Crosshair crosshair; // UI ���ؼ�

    void Update()
    {
        HandleInteraction();
    }

    void HandleInteraction()
    {
        // ȭ�� �߾ӿ��� �����ɽ�Ʈ �߻�
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // ������ Ư�� �Ÿ� �̳��� ������Ʈ�� �浹�ߴ��� �˻�
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            GameObject hitObject = hit.collider.gameObject;
            IInteractable interactable = hitObject.GetComponentInParent<IInteractable>(); // �θ� �����Ͽ� �������̽� ���� Ȯ��

            if (interactable != null) // ��ȣ�ۿ� ������ ������Ʈ�� ���
            {
                crosshair.SetColor(Color.red); //���ؼ� ���� ������
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();

                    // �߰� ��ȣ�ۿ��� �ִ� ���, �߰� �������̽� Ž��
                    ICustomInteractable additional = hitObject.GetComponentInParent<ICustomInteractable>();
                    if (additional != null)
                    {
                        additional.CustomInteractable();
                    }
                }
            }
            else
            {
                crosshair.SetColor(Color.green); //��ȣ�ۿ� �ȵǴ� ������Ʈ�� ���
            }
        }
        else
        {
            crosshair.SetColor(Color.white); //�ƹ��͵� ���� �� ���
        }
    }
}
