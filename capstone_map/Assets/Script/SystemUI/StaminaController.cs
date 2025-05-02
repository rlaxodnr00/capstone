using System;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 100f;       // �ִ� ���¹̳�
    public float currentStamina = 100f;   // ���� ���¹̳�
    public float staminaDrainRate = 20f;  // �ʴ� ���¹̳� �Ҹ�
    public float staminaRegenRate = 10f;  // �ʴ� ���¹̳� ȸ����
    public float regenDelay = 2f;         // ���¹̳� ȸ�� ������

    private float lastSprintTime;         // ������ �޸� �ð�
    private bool isSprinting = false;

    // ���¹̳��� ����� �� �߻��ϴ� �̺�Ʈ (���� ���¹̳�, �ִ� ���¹̳�)
    public event Action<float, float> OnStaminaChanged;

    void Start()
    {
        currentStamina = maxStamina;
        // �ʱ� ���¹̳� ���¸� �̺�Ʈ�� �˸�
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
        // �޸��⸦ ���� (LeftShift �Է�) �� ���¹̳� �Ҹ�
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

        // �޸��� �ߴ� �� ������ �ð� ���� ���¹̳� ȸ��
        if (!isSprinting && Time.time > lastSprintTime + regenDelay)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
        }
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        // ���¹̳� ���� ������ �̺�Ʈ�� �߻����� UI ������ ��û
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
    }
}
