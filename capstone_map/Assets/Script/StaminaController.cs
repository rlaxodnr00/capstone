using System;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 100f;       // 최대 스태미나
    public float currentStamina = 100f;   // 현재 스태미나
    public float staminaDrainRate = 20f;  // 초당 스태미나 소모량
    public float staminaRegenRate = 10f;  // 초당 스태미나 회복량
    public float regenDelay = 2f;         // 스태미나 회복 딜레이

    private float lastSprintTime;         // 마지막 달린 시간
    private bool isSprinting = false;

    // 스태미나가 변경될 때 발생하는 이벤트 (현재 스태미나, 최대 스태미나)
    public event Action<float, float> OnStaminaChanged;

    void Start()
    {
        currentStamina = maxStamina;
        // 초기 스태미나 상태를 이벤트로 알림
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);

        if (GameUIManager.Instance != null)
            OnStaminaChanged += GameUIManager.Instance.UpdateStaminaUI;
    }

    void Update()
    {
        HandleStamina();
    }

    void HandleStamina()
    {
        // 달리기를 감지 (LeftShift 입력) 및 스태미나 소모
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            isSprinting = true;
            currentStamina -= staminaDrainRate * Time.deltaTime;
            lastSprintTime = Time.time;
        }
        else
        {
            isSprinting = false;
        }

        // 달리기 중단 후 딜레이 시간 이후 스태미나 회복
        if (!isSprinting && Time.time > lastSprintTime + regenDelay)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
        }
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        // 스태미나 상태 변경을 이벤트로 발생시켜 UI 갱신을 요청
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
    }
}
