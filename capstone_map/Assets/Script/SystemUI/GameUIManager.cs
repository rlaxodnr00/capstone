using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
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
            Debug.LogWarning("[GameUIManager] batteryUI�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    // ü�� UI ������Ʈ �Լ�
    public void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        if (healthGauge != null)
        {
            healthGauge.fillAmount = currentHealth / maxHealth;
        }
        else
        {
            Debug.LogWarning("[GameUIManager] healthGauge�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }


    // ���¹̳� UI ������Ʈ �Լ�.
    public void UpdateStaminaUI(float currentStamina, float maxStamina)
    {
        if (staminaGauge != null)
        {
            staminaGauge.fillAmount = currentStamina / maxStamina;
        }
        else
        {
            Debug.LogWarning("[GameUIManager] staminaGauge�� �Ҵ���� �ʾҽ��ϴ�.");
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
            Debug.LogWarning("Dog Image�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}
