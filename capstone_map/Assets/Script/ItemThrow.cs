using UnityEngine;

public class ItemThrow : MonoBehaviour
{
    // 플레이어의 인벤토리 참조 (Inspector에서 할당)
    public PlayerInventory playerInventory;

    // 던지기 시작 위치 (플레이어의 손 또는 카메라 위치 등)
    public Transform throwOrigin;


    // 기본 던지기 힘 (이 값은 던지는 느낌을 조절하기 위해 조정 가능)
    public float baseThrowForce = 12f;

    // 던질 때 추가할 상승각(예: 약간 위로 던지기 위함)
    public float upwardForceFactor = 0.2f;

    // 던지기 입력 키 (여기서는 G키)
    public KeyCode throwKey = KeyCode.G;


    void Update()
    {
        // G키 눌렀을 때 던지기 실행
        if (Input.GetKeyDown(throwKey))
        {
            ThrowCurrentItem();
        }
    }

    void ThrowCurrentItem()
    {
        // 현재 인벤토리에 선택된 아이템 가져오기
        GameObject currentItem = playerInventory.GetCurrentItem();

        if (currentItem.GetComponent<ThrowableItem>() == null)
        {
            Debug.Log("이 아이템은 던질 수 없는 아이템입니다.");
            return;
        }

        if (currentItem == null)
        {
            Debug.Log("던질 아이템이 없습니다.");
            return;
        }

        // 아이템 드롭 처리 (이미 플레이어 인벤토리에서 해당 아이템을 제거하도록 구현되어 있어야 함)
        // 인벤토리 제거만 수행하고, 위치/회전은 그대로
        playerInventory.ItemThrow(playerInventory.CurrentSlot); // 현재 선택 슬롯 기준


        // 던질 아이템에 Rigidbody가 없다면 추가
        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = currentItem.AddComponent<Rigidbody>();
        }
        // 던질 때 물리 효과 활성화
        rb.isKinematic = false;
        rb.useGravity = true;

        // 아이템의 무게를 고려 (없으면 기본값 1로 설정)
        float itemWeight = 1f;
        ThrowableItem throwable = currentItem.GetComponent<ThrowableItem>();
        if (throwable != null)
        {
            itemWeight = throwable.weight;
        }

        // 플레이어의 던지는 방향: 플레이어 전방 + 약간의 상승각
        Vector3 throwDirection = transform.forward + transform.up * upwardForceFactor;
        throwDirection.Normalize();

        // 최종 던지기 힘 계산 (예, 가벼운 아이템은 더 멀리 날림)
        float finalForce = baseThrowForce / itemWeight;

        // 아이템에 임펄스 힘 추가 (즉, 던짐)
        rb.AddForce(throwDirection * finalForce, ForceMode.Impulse);

        Debug.Log("아이템 던짐, 힘: " + finalForce);
    }
}
