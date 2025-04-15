using UnityEngine;

public class Light_Switch : MonoBehaviour, IInteractable, ILightSwitchable
{
    public GameObject onSwitch; //on 상태 스위치
    public GameObject offSwitch; // off 상태 스위치
    public Light lightSource; //빛 오브젝트
    public AudioSource switchClick; //스위치 사운드

    private bool isLightOn = false; //스위치 상태

    void Start()
    {
        UpdateLightState();
    }

    public void Interact()
    {
        if (switchClick != null) switchClick.Play();
        ToggleLight();
    }

    public void ToggleLight()
    {
        isLightOn = !isLightOn;
        UpdateLightState();
    }

    void UpdateLightState()
    {
        if (lightSource != null)
        {
            lightSource.enabled = isLightOn;
        }

        if (onSwitch != null) onSwitch.SetActive(isLightOn);
        if (offSwitch != null) offSwitch.SetActive(!isLightOn);
    }
}
