using UnityEngine;

public class ItemThrow : MonoBehaviour
{
    // �÷��̾��� �κ��丮 ���� (Inspector���� �Ҵ�)
    public PlayerInventory playerInventory;

    // ������ ���� ��ġ (�÷��̾��� �� �Ǵ� ī�޶� ��ġ ��)
    public Transform throwOrigin;


    // �⺻ ������ �� (�� ���� ������ ������ �����ϱ� ���� ���� ����)
    public float baseThrowForce = 12f;

    // ���� �� �߰��� ��°�(��: �ణ ���� ������ ����)
    public float upwardForceFactor = 0.2f;

    // ������ �Է� Ű (���⼭�� GŰ)
    public KeyCode throwKey = KeyCode.G;


    void Update()
    {
        // GŰ ������ �� ������ ����
        if (Input.GetKeyDown(throwKey))
        {
            ThrowCurrentItem();
        }
    }

    void ThrowCurrentItem()
    {
        // ���� �κ��丮�� ���õ� ������ ��������
        GameObject currentItem = playerInventory.GetCurrentItem();

        if (currentItem.GetComponent<ThrowableItem>() == null)
        {
            Debug.Log("�� �������� ���� �� ���� �������Դϴ�.");
            return;
        }

        if (currentItem == null)
        {
            Debug.Log("���� �������� �����ϴ�.");
            return;
        }

        // ������ ��� ó�� (�̹� �÷��̾� �κ��丮���� �ش� �������� �����ϵ��� �����Ǿ� �־�� ��)
        // �κ��丮 ���Ÿ� �����ϰ�, ��ġ/ȸ���� �״��
        playerInventory.ItemThrow(playerInventory.CurrentSlot); // ���� ���� ���� ����


        // ���� �����ۿ� Rigidbody�� ���ٸ� �߰�
        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = currentItem.AddComponent<Rigidbody>();
        }
        // ���� �� ���� ȿ�� Ȱ��ȭ
        rb.isKinematic = false;
        rb.useGravity = true;

        // �������� ���Ը� ��� (������ �⺻�� 1�� ����)
        float itemWeight = 1f;
        ThrowableItem throwable = currentItem.GetComponent<ThrowableItem>();
        if (throwable != null)
        {
            itemWeight = throwable.weight;
        }

        // �÷��̾��� ������ ����: �÷��̾� ���� + �ణ�� ��°�
        Vector3 throwDirection = transform.forward + transform.up * upwardForceFactor;
        throwDirection.Normalize();

        // ���� ������ �� ��� (��, ������ �������� �� �ָ� ����)
        float finalForce = baseThrowForce / itemWeight;

        // �����ۿ� ���޽� �� �߰� (��, ����)
        rb.AddForce(throwDirection * finalForce, ForceMode.Impulse);

        Debug.Log("������ ����, ��: " + finalForce);
    }
}
