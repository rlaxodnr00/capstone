/*
 * �⺻ ��ȣ�ۿ� << Interact
 * ��ȣ�ۿ� �� �κ��丮�� ���� ������ ��,
 *  �κ��丮 ������ ��ȣ�ۿ� << IInventoryUsable
*/

public interface IInteractable //�Ϲ� ��ȣ�ۿ�
{
    void Interact(); //�⺻ EŰ ��ȣ�ۿ�
}

public interface ICustomInteractable  //�Ϲ� ��ȣ�ۿ뿡 ������ �� �߰� ��ȣ�ۿ�
{
    void CustomInteractable(); //Interact ��ȣ�ۿ뿡 �߰� ���
}

public interface ICustomDrop // ������ ��� �� ������ �� �߰� ��ȣ�ۿ�
{
    void CustomDrop();
}

public interface ILightSwitchable //���� ���� ��� ��� << ���� ����
{
    void ToggleLight(); // ������ �Ѱ� ���� ���
}

public interface IInventoryInteractable //�κ��丮 ���� ��ȣ�ۿ�
{
    void InventoryInteract(PlayerInventory inventory);       // �⺻ FŰ ��ȣ�ۿ�
}



