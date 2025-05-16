using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    [Header("Hit Effect")]
    public RawImage hitImage;

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

    private void Start()
    {
        if (ScreenTransition.Instance != null)
        {
            ScreenTransition.Instance.StartFadeIn();
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

    public void StartHitEffect()
    {
        StartCoroutine(HitEffectCoroutine());
    }


    private IEnumerator HitEffectCoroutine()
    {
        if (hitImage == null) yield break;

        float upDuration = 0.2f; //���İ� �ö󰡴� �ð�
        float holdDuration = 0.4f; //���İ� ���� �ð�
        float downDuration = 0.4f; //���İ� �������� �ð�
        float time = 0f;

        // ��� (alpha 0 �� 0.666)
        while (time < upDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 0.666f, time / upDuration);
            SetOverlayAlpha(alpha);
            yield return null;
        }

        // ���� (alpha 0.666 ����)
        SetOverlayAlpha(0.666f);
        yield return new WaitForSeconds(holdDuration);

        // �ϰ� (alpha 0.666 �� 0)
        time = 0f;
        while (time < downDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0.666f, 0f, time / downDuration);
            SetOverlayAlpha(alpha);
            yield return null;
        }

        SetOverlayAlpha(0f);
    }

    private void SetOverlayAlpha(float alpha)
    {
        Color c = hitImage.color;
        c.a = alpha;
        hitImage.color = c;
    }

}
