
/// IInteractable: 월드 상에서의 상호작용(예: 손전등 습득) 처리.
/// IInventoryInteractable: 인벤토리 내에서의 상호작용(예: 전원 토글) 처리.
/// IUIDeactivatable : UI 활성, 비활성화 처리

using UnityEngine;


public class Flashlight : MonoBehaviour, ICustomInteractable, IInventoryInteractable, ICustomDrop
{
    // 배터리 관련 변수
    [SerializeField] private float batteryLevel = 57f;         // 초기 배터리 잔량 (0 ~ 100)
    [SerializeField] private float batteryDrainRate = 1.4f;      // 초당 배터리 소모율
    [SerializeField] private float batteryChargeAmount = 30f;    // 충전 아이템 사용 시 증가량
    [SerializeField] private float maxBatteryLevel = 100f;    // 최대 충전량

    // 손전등의 빛을 표현하는 Light 컴포넌트
    [SerializeField] private Light flashlightLight;

    // 손전등 전원 상태 (켜짐/꺼짐)
    private bool isFlashlightOn = false;

    // 월드 상에서 손전등 습득 여부 판단 (이미 인벤토리로 넘어갔는지)
    private bool hasBeenPickedUp = false;



    void Start()
    {
        // Inspector에서 할당되지 않았다면 하위에서 Light 컴포넌트 검색
        if (flashlightLight == null)
        {
            flashlightLight = GetComponentInChildren<Light>();
        }
        if (flashlightLight == null)
        {
            Debug.LogError("[Flashlight] 하위에 Light 컴포넌트가 없습니다.");
        }
        else
        {
            flashlightLight.enabled = false;  // 시작 시 불빛 OFF
        }

        // 초기 배터리 잔량 업데이트: GameUIManager를 통해 배터리 UI에 반영
        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.UpdateBatteryUI(batteryLevel);
            Debug.Log("배터리 UI 업데이트");
        }
        else
        {
            Debug.Log("배터리 UI 업데이트하지 못함");
        }
    }

    void Update()
    {

        // 손전등이 켜져 있다면 배터리 소모 진행
        if (isFlashlightOn && batteryLevel > 0)
        {
            batteryLevel -= batteryDrainRate * Time.deltaTime;
            batteryLevel = Mathf.Max(batteryLevel, 0f);

            // 배터리 잔량을 중앙 UI 매니저를 통해 업데이트
            if (GameUIManager.Instance != null)
            {
                GameUIManager.Instance.UpdateBatteryUI(batteryLevel);
            }

            // 배터리가 소진되면 자동으로 손전등 전원 끄기
            if (batteryLevel <= 0f)
            {
                ToggleLight(false);
                Debug.Log("배터리 소진으로 손전등이 꺼졌습니다.");
            }
        }
    }

    #region Interface Implementations


    //손전등 획득 시 추가 상호작용 동작
    public void CustomInteractable()
    {
        if (!hasBeenPickedUp)
        {
            hasBeenPickedUp = true;
            Debug.Log("[Flashlight] 손전등이 습득되었습니다.");
            // 손전등 획득 시 배터리 UI 활성화
            if (GameUIManager.Instance != null && GameUIManager.Instance.batteryUI != null)
            {
                GameUIManager.Instance.batteryUI.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.Log("[Flashlight] 이미 습득된 손전등입니다.");
        }
    }


    //인벤토리 내에서 F키로 상호작용할 때 호출
    public void InventoryInteract(PlayerInventory inventory)
    {
        // 배터리가 없으면 전원 토글 시도조차 하지 않음
        if (batteryLevel <= 0 && !isFlashlightOn)
        {
            Debug.Log("[Flashlight] 배터리가 없어서 손전등을 켤 수 없습니다.");
            return;
        }
        ToggleLight(!isFlashlightOn);
    }

    public void CustomDrop()
    {
        hasBeenPickedUp = false; //가지고 있지 않음

        //GameUIManager를 통한 배터리 UI 비활성화
        if (GameUIManager.Instance != null && GameUIManager.Instance.batteryUI != null)
        {
            GameUIManager.Instance.batteryUI.gameObject.SetActive(false);
        }
        Debug.Log("[Flashlight] 손전등 드롭");

    }



    #endregion


    // 손전등 전원을 토글하는 메서드
    // 불을 켤 때는 배터리 상태도 확인
    private void ToggleLight(bool state)
    {
        if (flashlightLight == null) return;

        // 켜려는 경우, 배터리가 충분한지 확인
        if (state && batteryLevel <= 0f)
        {
            Debug.Log("[Flashlight] 배터리 부족으로 손전등을 켤 수 없습니다.");
            return;
        }
        isFlashlightOn = state;
        flashlightLight.enabled = isFlashlightOn;
        Debug.Log("[Flashlight] 손전등 상태: " + isFlashlightOn);
    }

    //배터리를 충전하는 메서드
    //충전 후 UI 업데이트 진행
    public void ChargeBattery()
    {
        batteryLevel += batteryChargeAmount;
        batteryLevel = Mathf.Min(batteryLevel, maxBatteryLevel);
        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.UpdateBatteryUI(batteryLevel);
        }
        Debug.Log("[Flashlight] 배터리 충전 후 잔량: " + batteryLevel);
    }
}

