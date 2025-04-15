using UnityEngine;

public class Interactable : MonoBehaviour
{
    // �÷��̾ ������Ʈ�� �ٶ� �� ȣ��Ǵ� �Լ�
    public virtual void OnLookAt()
    {
        // Debug.Log(gameObject.name + "�� �ٶ�.");
    }

    // �÷��̾ ������Ʈ���� �ü��� ���� �� ȣ��Ǵ� �Լ�
    public virtual void OnLookAway()
    {
        // Debug.Log(gameObject.name + "���� �ü��� ����.");
    }

    // �÷��̾ ��ȣ�ۿ��� �� ȣ��Ǵ� �Լ�
    public virtual void OnInteract()
    {
        // Debug.Log(gameObject.name + "�� ��ȣ�ۿ���.");
    }
}