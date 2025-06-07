using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    [Header("Hit Effect")]
    public RawImage hitImage;

    [Header("GasMask UI")]
    public Image gasMaskImage;

    [Header("Die Image")] //안쓰고있음
    public GameObject dieImage;

    [Header("Timer UI")] //안쓰고있음
    public GameOverTimer gameOverTimer;

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
        Physics.gravity = new Vector3(0, -0.981f, 0); // 스케일 0.1 기준 중력 보정
        Debug.Log("중력 설정됨: " + Physics.gravity);

        if (ScreenTransition.Instance != null)
        {
            ScreenTransition.Instance.StartFadeIn();
        }

    }

    public static Color RGB(byte r, byte g, byte b) //색 간결하게 쓰는 유틸 메서드
    {
        return new Color(r / 255f, g / 255f, b / 255f);
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

    public void ShowDieImage() //안쓰고있음
    {
        if(dieImage != null)
        {
            dieImage.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Die Image가 할당되지 않았습니다.");
        }
    }

    public void StartHitEffect()
    {
        StartCoroutine(HitEffectCoroutine());
    }


    private IEnumerator HitEffectCoroutine()
    {
        if (hitImage == null) yield break;

        float upDuration = 0.2f; //알파값 올라가는 시간
        float holdDuration = 0.4f; //알파값 유지 시간
        float downDuration = 0.4f; //알파값 내려가는 시간
        float time = 0f;

        // 상승 (alpha 0 → 0.666)
        while (time < upDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 0.666f, time / upDuration);
            SetOverlayAlpha(alpha);
            yield return null;
        }

        // 유지 (alpha 0.666 고정)
        SetOverlayAlpha(0.666f);
        yield return new WaitForSeconds(holdDuration);

        // 하강 (alpha 0.666 → 0)
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


    public void EnableGasMaskOverlay()
    {
        ShowGasMaskOverlay(true);
    }

    public void DisableGasMaskOverlay()
    {
        ShowGasMaskOverlay(false);
    }

    public void ShowGasMaskOverlay(bool show)
    {
        if (gasMaskImage != null)
        {
            gasMaskImage.gameObject.SetActive(show);
        }
        else
        {
            Debug.LogWarning("GasMask UI 이미지가 할당되지 않았습니다.");
        }
    }


    public void StartGameEndingEffect(float duration, Color overlayColor) //사망 효과 참조용
    {
        StartCoroutine(GameEndingEffectCoroutine(duration, overlayColor));
    }

    private IEnumerator GameEndingEffectCoroutine(float duration, Color targetColor) //사망 효과
    {
        if (hitImage == null) yield break;

        float time = 0f;
        float maxAlpha = 1f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, maxAlpha, time / duration); // 0 → maxAlpha까지 서서히 상승

            Color c = targetColor;
            c.a = alpha;

            hitImage.color = c;

            yield return null;
        }

        SetOverlayAlpha(maxAlpha); // 알파값 최대치 상태 유지 (씬 전환 중)
    }

   
}
