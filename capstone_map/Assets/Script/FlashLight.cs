
/// IInteractable: 월드 상에서의 상호작용(예: 손전등 습득) 처리.
/// IInventoryInteractable: 인벤토리 내에서의 상호작용(예: 전원 토글) 처리.
/// IUIDeactivatable : UI 활성, 비활성화 처리

using UnityEngine;


public class Flashlight : MonoBehaviour, ICustomInteractable, IInventoryInteractable, ICustomDrop
{
    // 배터리 관련 변수
    [SerializeField] private float batteryLevel = 86f;         // 초기 배터리 잔량 (0 ~ 100)
    [SerializeField] private float batteryDrainRate = 0.4f;      // 초당 배터리 소모율
    [SerializeField] private float batteryChargeAmount = 50f;    // 충전 아이템 사용 시 증가량

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
    public void InventoryInteract()
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


    // 손전등 전원을 토글하는 메서드.
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

    /// <summary>
    /// 배터리를 충전하는 메서드.
    /// 충전 후 GameUIManager를 통해 UI 업데이트도 진행합니다.
    /// </summary>
    public void ChargeBattery()
    {
        batteryLevel += batteryChargeAmount;
        batteryLevel = Mathf.Min(batteryLevel, 100f);
        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.UpdateBatteryUI(batteryLevel);
        }
        Debug.Log("[Flashlight] 배터리 충전 후 잔량: " + batteryLevel);
    }
}


/*
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour, IInteractable, ILightSwitchable
{
    private bool hasFlashlight = false;
    public bool HasFlashlight => hasFlashlight; // 읽기 전용 속성

    private bool isFlashlightOn = false;
    private Light flashlightLight; //손전등 빛
    private float batteryLevel = 86f; // 손전등 배터리 잔량, 초기 배터리 86%
    private float batteryDrainRate = 0.4f; // 1초당 0.4% 소모
    private float batteryChargeAmount = 50f; // 배터리 1개당 50% 충전

    public BatteryUI batteryUI; // UI 업데이트용
    public GameObject batteryUIObject; // 오브젝트 활성화 / 비활성화용
    public Transform playerHand; //플레이어 손 위치
   



    void Start()
    {
        //flashlightLight = GameObject.FindGameObjectWithTag("FlashLight")?.GetComponent<Light>();
        flashlightLight = GetComponentInChildren<Light>(); //현재나 하위 오브젝트에서 첫번째로 발견된 <>오브젝트 가져옴

        if (flashlightLight == null)
        {
            Debug.LogError("손전등 하위에 Light 컴포넌트가 없습니다");
        }
        else
        {
            flashlightLight.enabled = false; // 시작할 때 불빛 OFF
        }

        // 게임 시작 시 배터리 UI 숨기기
        if (batteryUIObject != null)
        {
            batteryUIObject.SetActive(false);
        }

        UpdateBatteryUI();
    }

    void Update()
    {
        
        if (hasFlashlight && Input.GetKeyDown(KeyCode.F)) // F키로 손전등 토글
        {
            ToggleLight();
        }

        if (isFlashlightOn && batteryLevel > 0)
        {
            batteryLevel -= batteryDrainRate * Time.deltaTime;
            batteryLevel = Mathf.Max(0, batteryLevel);
            UpdateBatteryUI();

            if (batteryLevel <= 0)
            {
                ToggleLight(false); // 배터리 소진 시 손전등 자동으로 꺼짐
            }
        }
    }

    public void Interact() //습득
    {
        if (!hasFlashlight) // 손전등 습득
        {
            Debug.Log("손전등 습득");
            hasFlashlight = true;

            // 플레이어의 손 오브젝트를 찾음
            playerHand = GameObject.Find("playerHand").transform;

            //손전등 플레이어 지정 위치로 이동
            transform.SetParent(playerHand); //부모를 playerHand로 변경
            transform.localPosition = Vector3.zero; //상대좌표 0,0,0 >> 부모 중심에 위치


            Vector3 flashlightRotation = new Vector3(90, 0, 0); // X, Y, Z 회전값
            transform.localRotation = Quaternion.Euler(flashlightRotation);
            //Quaternion.identity; //Quaternion.identity >> 회전 초기화(0,0,0)

            // 손전등을 획득하면 UI 활성화
            if (batteryUIObject != null)
            {
                batteryUIObject.SetActive(true);
            }
        }
    }

    public void ToggleLight(bool state) //손전등 얻거나 배터리 소진 시
    {
        if (!hasFlashlight || flashlightLight == null) return;

        isFlashlightOn = state;
        flashlightLight.enabled = isFlashlightOn;
    }

    public void ToggleLight() //손전등 전원 on / off
    {
        Debug.Log("손전등 습득 ToggleLight() 활성화");
        if (!hasFlashlight)
        {
            Debug.Log("손전등이 없습니다");
            return;
        }

        if (flashlightLight == null)
        {
            Debug.LogError("flashlightLight가 null입니다! Light 컴포넌트가 할당되지 않았습니다.");
            return;
        }

        if (batteryLevel > 0)
        {
            isFlashlightOn = !isFlashlightOn;
            flashlightLight.enabled = isFlashlightOn;
            Debug.Log("손전등 상태 변경: " + isFlashlightOn);
        }
        else
        {
            Debug.Log("손전등 배터리가 부족하여 전원을 켤 수 없습니다");
        }
    }

    public void ChargeBattery()
    {
        batteryLevel += batteryChargeAmount;
        batteryLevel = Mathf.Min(batteryLevel, 100f); //최대치 100f 넘지 않도록
        UpdateBatteryUI();
    }

    private void UpdateBatteryUI()
    {
        if (batteryUI != null)
        {
            Debug.Log("배터리 잔량 : " + batteryLevel);
            batteryUI.UpdateBatteryUI(batteryLevel);
        }
    }
}








*/












/*
 * 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{

    [Header("Setting")]
    bool OnFlashLight; //손전등 스위치
    static bool userGetLight; //손전등 획득 여부 확인 변수
    Light myLight; //Light 컴포넌트 담는 변수

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        userGetLight = false;
        OnFlashLight = false; //손전등 off 상태
        myLight = this.GetComponent<Light>(); //오브젝트가 가진 Light값 가져옴
    }

    // Update is called once per frame
    void Update()
    {
        lightOnOff();
       

        //손전등 기능 구현
        if (OnFlashLight == true)
        {
            myLight.intensity = 10; //손전등 on
        }
        else
        {
            myLight.intensity = 0; //손전등 off
        }
    }


    static internal void GetLight()
    {
        if (Input.GetButtonDown("Interact"))
        {
            userGetLight = true;
            Destroy(GameObject.FindGameObjectWithTag("FlashLight"));
        }
    }

    void lightOnOff()
    {
        if(userGetLight == true)
        {
            //r키로 손전등 on / off
            if (Input.GetKeyDown(KeyCode.R))
            {
                OnFlashLight = OnFlashLight ? false : true; //토글 기능
            }
        } 
    }
}
*/