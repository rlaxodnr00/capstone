
/// IInteractable: ���� �󿡼��� ��ȣ�ۿ�(��: ������ ����) ó��.
/// IInventoryInteractable: �κ��丮 �������� ��ȣ�ۿ�(��: ���� ���) ó��.
/// IUIDeactivatable : UI Ȱ��, ��Ȱ��ȭ ó��

using UnityEngine;


public class Flashlight : MonoBehaviour, ICustomInteractable, IInventoryInteractable, ICustomDrop
{
    // ���͸� ���� ����
    [SerializeField] private float batteryLevel = 57f;         // �ʱ� ���͸� �ܷ� (0 ~ 100)
    [SerializeField] private float batteryDrainRate = 1.4f;      // �ʴ� ���͸� �Ҹ���
    [SerializeField] private float batteryChargeAmount = 30f;    // ���� ������ ��� �� ������
    [SerializeField] private float maxBatteryLevel = 100f;    // �ִ� ������

    // �������� ���� ǥ���ϴ� Light ������Ʈ
    [SerializeField] private Light flashlightLight;

    // ������ ���� ���� (����/����)
    private bool isFlashlightOn = false;

    // ���� �󿡼� ������ ���� ���� �Ǵ� (�̹� �κ��丮�� �Ѿ����)
    private bool hasBeenPickedUp = false;



    void Start()
    {
        // Inspector���� �Ҵ���� �ʾҴٸ� �������� Light ������Ʈ �˻�
        if (flashlightLight == null)
        {
            flashlightLight = GetComponentInChildren<Light>();
        }
        if (flashlightLight == null)
        {
            Debug.LogError("[Flashlight] ������ Light ������Ʈ�� �����ϴ�.");
        }
        else
        {
            flashlightLight.enabled = false;  // ���� �� �Һ� OFF
        }

        // �ʱ� ���͸� �ܷ� ������Ʈ: GameUIManager�� ���� ���͸� UI�� �ݿ�
        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.UpdateBatteryUI(batteryLevel);
            Debug.Log("���͸� UI ������Ʈ");
        }
        else
        {
            Debug.Log("���͸� UI ������Ʈ���� ����");
        }
    }

    void Update()
    {

        // �������� ���� �ִٸ� ���͸� �Ҹ� ����
        if (isFlashlightOn && batteryLevel > 0)
        {
            batteryLevel -= batteryDrainRate * Time.deltaTime;
            batteryLevel = Mathf.Max(batteryLevel, 0f);

            // ���͸� �ܷ��� �߾� UI �Ŵ����� ���� ������Ʈ
            if (GameUIManager.Instance != null)
            {
                GameUIManager.Instance.UpdateBatteryUI(batteryLevel);
            }

            // ���͸��� �����Ǹ� �ڵ����� ������ ���� ����
            if (batteryLevel <= 0f)
            {
                ToggleLight(false);
                Debug.Log("���͸� �������� �������� �������ϴ�.");
            }
        }
    }

    #region Interface Implementations


    //������ ȹ�� �� �߰� ��ȣ�ۿ� ����
    public void CustomInteractable()
    {
        if (!hasBeenPickedUp)
        {
            hasBeenPickedUp = true;
            Debug.Log("[Flashlight] �������� ����Ǿ����ϴ�.");
            // ������ ȹ�� �� ���͸� UI Ȱ��ȭ
            if (GameUIManager.Instance != null && GameUIManager.Instance.batteryUI != null)
            {
                GameUIManager.Instance.batteryUI.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.Log("[Flashlight] �̹� ����� �������Դϴ�.");
        }
    }


    //�κ��丮 ������ FŰ�� ��ȣ�ۿ��� �� ȣ��
    public void InventoryInteract(PlayerInventory inventory)
    {
        // ���͸��� ������ ���� ��� �õ����� ���� ����
        if (batteryLevel <= 0 && !isFlashlightOn)
        {
            Debug.Log("[Flashlight] ���͸��� ��� �������� �� �� �����ϴ�.");
            return;
        }
        ToggleLight(!isFlashlightOn);
    }

    public void CustomDrop()
    {
        hasBeenPickedUp = false; //������ ���� ����

        //GameUIManager�� ���� ���͸� UI ��Ȱ��ȭ
        if (GameUIManager.Instance != null && GameUIManager.Instance.batteryUI != null)
        {
            GameUIManager.Instance.batteryUI.gameObject.SetActive(false);
        }
        Debug.Log("[Flashlight] ������ ���");

    }



    #endregion


    // ������ ������ ����ϴ� �޼���
    // ���� �� ���� ���͸� ���µ� Ȯ��
    private void ToggleLight(bool state)
    {
        if (flashlightLight == null) return;

        // �ѷ��� ���, ���͸��� ������� Ȯ��
        if (state && batteryLevel <= 0f)
        {
            Debug.Log("[Flashlight] ���͸� �������� �������� �� �� �����ϴ�.");
            return;
        }
        isFlashlightOn = state;
        flashlightLight.enabled = isFlashlightOn;
        Debug.Log("[Flashlight] ������ ����: " + isFlashlightOn);
    }

    //���͸��� �����ϴ� �޼���
    //���� �� UI ������Ʈ ����
    public void ChargeBattery()
    {
        batteryLevel += batteryChargeAmount;
        batteryLevel = Mathf.Min(batteryLevel, maxBatteryLevel);
        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.UpdateBatteryUI(batteryLevel);
        }
        Debug.Log("[Flashlight] ���͸� ���� �� �ܷ�: " + batteryLevel);
    }
}

