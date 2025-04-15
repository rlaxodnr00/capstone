using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    // 싱글턴 인스턴스
    public static GameUIManager Instance;

    [Header("Battery UI")]
    public BatteryUI batteryUI;

    [Header("Health UI")]
    public Image healthGauge;
    public float uiMaxHealth = 100f;

    [Header("Stamina UI")]
    public Image staminaGauge;
    public float uiMaxStamina = 100f;

    [Header("Dog Image")]
    public GameObject dogImage; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (batteryUI != null)
            {
                batteryUI.gameObject.SetActive(false);
            }
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateBatteryUI(float batteryLevel)
    {
        if (batteryUI != null)
        {
            batteryUI.UpdateBatteryUI(batteryLevel);
        }
        else
        {
            Debug.LogWarning("[GameUIManager] batteryUI가 할당되지 않았습니다.");
        }
    }

    // 체력 UI 업데이트 함수
    public void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        if (healthGauge != null)
        {
            healthGauge.fillAmount = currentHealth / maxHealth;
        }
        else
        {
            Debug.LogWarning("[GameUIManager] healthGauge가 할당되지 않았습니다.");
        }
    }


    // 스태미나 UI 업데이트 함수.
    public void UpdateStaminaUI(float currentStamina, float maxStamina)
    {
        if (staminaGauge != null)
        {
            staminaGauge.fillAmount = currentStamina / maxStamina;
        }
        else
        {
            Debug.LogWarning("[GameUIManager] staminaGauge가 할당되지 않았습니다.");
        }
    }

    public void ShowDogImage()
    {
        if(dogImage != null)
        {
            dogImage.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Dog Image가 할당되지 않았습니다.");
        }
    }
}
