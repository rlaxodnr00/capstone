using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BrightnessController : MonoBehaviour
{
    public Slider brightnessSlider;
    public TMP_InputField brightnessInput;

    private void Start()
    {
        // 초기값: GameSettingsManager에서 가져옴
        float normalized = GameSettingsManager.Instance != null
            ? GameSettingsManager.Instance.brightness
            : 0.6f;

        brightnessSlider.value = normalized;
        brightnessInput.text = Mathf.RoundToInt(normalized * 100f).ToString();

        // 리스너 등록
        brightnessSlider.onValueChanged.AddListener(OnSliderChanged);
        brightnessInput.onEndEdit.AddListener(OnInputChanged);
    }

    void OnSliderChanged(float value)
    {
        GameSettingsManager.Instance.SetBrightness(value);
        brightnessInput.text = Mathf.RoundToInt(value * 100f).ToString();
    }

    void OnInputChanged(string text)
    {
        if (float.TryParse(text, out float percent))
        {
            float normalized = Mathf.Clamp01(percent / 100f);
            brightnessSlider.value = normalized;
            GameSettingsManager.Instance.SetBrightness(normalized);
        }
    }
}
