using UnityEngine;

public class Memo :Interactable
{
    // �޸��� ��ȣ
    public int memoNumber;
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
        Debug.Log(memoNumber + "�� �޸��� ��ȣ�ۿ�");
    }
}
