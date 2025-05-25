using UnityEngine;
using UnityEngine.UI;

public class GameSettingsManager : MonoBehaviour
{
    public static GameSettingsManager Instance { get; private set; }

    [Range(0f, 1f)] public float brightness = 0.6f; // 기본값은 60%

    public RawImage brightnessOverlay; // 전역에서 적용할 오버레이

    private const float minAlpha = 0f;
    private const float maxAlpha = 230f / 255f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 초기값 로딩
            float saved = PlayerPrefs.GetFloat("Brightness", 60f);
            SetBrightness(saved / 100f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetBrightness(float value)
    {
        brightness = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat("Brightness", brightness * 100f);
        ApplyBrightness();
    }

    public void ApplyBrightness()
    {
        if (brightnessOverlay != null)
        {
            float alpha = Mathf.Lerp(maxAlpha, minAlpha, brightness);
            Color c = brightnessOverlay.color;
            brightnessOverlay.color = new Color(c.r, c.g, c.b, alpha);
        }
    }
}
