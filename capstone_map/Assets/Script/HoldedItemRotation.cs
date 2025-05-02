using UnityEngine;

public class HoldedItemRotation : MonoBehaviour
{
    [Header("── 손에 들렸을 때 ──")]
    public Vector3 heldPosOffset;           // localPosition
    public Vector3 heldRotOffsetEuler;      // localEulerAngles

    [Header("── 필드에 떨어졌을 때 ──")]
    public Vector3 dropPosOffset;           // field 에 더해줄 값
    public Vector3 dropRotOffsetEuler;      // worldEulerAngles

    public void ApplyHeld(Transform Hand) //플레이어 손 오브젝트 위치
    {
        transform.SetParent(Hand);
        transform.localPosition = heldPosOffset;
        transform.localRotation = Quaternion.Euler(heldRotOffsetEuler);
    }

    public void DropHeld(Vector3 field) //플레이어 발 오브젝트에서의 거리
    {
        transform.SetParent(null);
        transform.position = field + dropPosOffset;
        transform.rotation = Quaternion.Euler(dropRotOffsetEuler);
    }
}
