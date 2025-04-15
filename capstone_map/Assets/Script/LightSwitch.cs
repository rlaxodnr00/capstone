using UnityEngine;

public class Light_Switch : MonoBehaviour, IInteractable, ILightSwitchable
{
    public GameObject onSwitch; //on ���� ����ġ
    public GameObject offSwitch; // off ���� ����ġ
    public Light lightSource; //�� ������Ʈ
    public AudioSource switchClick; //����ġ ����

    private bool isLightOn = false; //����ġ ����

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
