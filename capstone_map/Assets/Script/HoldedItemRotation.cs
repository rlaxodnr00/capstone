using UnityEngine;

public class HoldedItemRotation : MonoBehaviour
{
    [Header("���� �տ� ����� �� ����")]
    public Vector3 heldPosOffset;           // localPosition
    public Vector3 heldRotOffsetEuler;      // localEulerAngles

    [Header("���� �ʵ忡 �������� �� ����")]
    public Vector3 dropPosOffset;           // field �� ������ ��
    public Vector3 dropRotOffsetEuler;      // worldEulerAngles

    public void ApplyHeld(Transform Hand) //�÷��̾� �� ������Ʈ ��ġ
    {
        transform.SetParent(Hand);
        transform.localPosition = heldPosOffset;
        transform.localRotation = Quaternion.Euler(heldRotOffsetEuler);
    }

    public void DropHeld(Vector3 field) //�÷��̾� �� ������Ʈ������ �Ÿ�
    {
        transform.SetParent(null);
        transform.position = field + dropPosOffset;
        transform.rotation = Quaternion.Euler(dropRotOffsetEuler);
    }
}
