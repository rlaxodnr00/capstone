using UnityEngine;

public class Memo :Interactable
{
    // �޸��� UI
    // �޸��� �ؽ�Ʈ ��

    public override void OnLookAt()
    {
        
    }

    public override void OnLookAway()
    {
        // �޸����� ���� �ΰ� �ü��� ������ �޸����� ����
    }

    public override void OnInteract()
    {
        // �޸��� ����
        Debug.Log("�޸��� ��ȣ�ۿ�");
    }
}
