
/// IInteractable: ���� �󿡼��� ��ȣ�ۿ�(��: ������ ����) ó��.
/// IInventoryInteractable: �κ��丮 �������� ��ȣ�ۿ�(��: ���� ���) ó��.
/// IUIDeactivatable : UI Ȱ��, ��Ȱ��ȭ ó��

using UnityEngine;


public class Flashlight : MonoBehaviour, ICustomInteractable, IInventoryInteractable, ICustomDrop
{
    // ���͸� ���� ����
    [SerializeField] private float batteryLevel = 86f;         // �ʱ� ���͸� �ܷ� (0 ~ 100)
    [SerializeField] private float batteryDrainRate = 0.4f;      // �ʴ� ���͸� �Ҹ���
    [SerializeField] private float batteryChargeAmount = 50f;    // ���� ������ ��� �� ������

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
    public void InventoryInteract()
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


    // ������ ������ ����ϴ� �޼���.
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

    /// <summary>
    /// ���͸��� �����ϴ� �޼���.
    /// ���� �� GameUIManager�� ���� UI ������Ʈ�� �����մϴ�.
    /// </summary>
    public void ChargeBattery()
    {
        batteryLevel += batteryChargeAmount;
        batteryLevel = Mathf.Min(batteryLevel, 100f);
        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.UpdateBatteryUI(batteryLevel);
        }
        Debug.Log("[Flashlight] ���͸� ���� �� �ܷ�: " + batteryLevel);
    }
}


/*
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour, IInteractable, ILightSwitchable
{
    private bool hasFlashlight = false;
    public bool HasFlashlight => hasFlashlight; // �б� ���� �Ӽ�

    private bool isFlashlightOn = false;
    private Light flashlightLight; //������ ��
    private float batteryLevel = 86f; // ������ ���͸� �ܷ�, �ʱ� ���͸� 86%
    private float batteryDrainRate = 0.4f; // 1�ʴ� 0.4% �Ҹ�
    private float batteryChargeAmount = 50f; // ���͸� 1���� 50% ����

    public BatteryUI batteryUI; // UI ������Ʈ��
    public GameObject batteryUIObject; // ������Ʈ Ȱ��ȭ / ��Ȱ��ȭ��
    public Transform playerHand; //�÷��̾� �� ��ġ
   



    void Start()
    {
        //flashlightLight = GameObject.FindGameObjectWithTag("FlashLight")?.GetComponent<Light>();
        flashlightLight = GetComponentInChildren<Light>(); //���糪 ���� ������Ʈ���� ù��°�� �߰ߵ� <>������Ʈ ������

        if (flashlightLight == null)
        {
            Debug.LogError("������ ������ Light ������Ʈ�� �����ϴ�");
        }
        else
        {
            flashlightLight.enabled = false; // ������ �� �Һ� OFF
        }

        // ���� ���� �� ���͸� UI �����
        if (batteryUIObject != null)
        {
            batteryUIObject.SetActive(false);
        }

        UpdateBatteryUI();
    }

    void Update()
    {
        
        if (hasFlashlight && Input.GetKeyDown(KeyCode.F)) // FŰ�� ������ ���
        {
            ToggleLight();
        }

        if (isFlashlightOn && batteryLevel > 0)
        {
            batteryLevel -= batteryDrainRate * Time.deltaTime;
            batteryLevel = Mathf.Max(0, batteryLevel);
            UpdateBatteryUI();

            if (batteryLevel <= 0)
            {
                ToggleLight(false); // ���͸� ���� �� ������ �ڵ����� ����
            }
        }
    }

    public void Interact() //����
    {
        if (!hasFlashlight) // ������ ����
        {
            Debug.Log("������ ����");
            hasFlashlight = true;

            // �÷��̾��� �� ������Ʈ�� ã��
            playerHand = GameObject.Find("playerHand").transform;

            //������ �÷��̾� ���� ��ġ�� �̵�
            transform.SetParent(playerHand); //�θ� playerHand�� ����
            transform.localPosition = Vector3.zero; //�����ǥ 0,0,0 >> �θ� �߽ɿ� ��ġ


            Vector3 flashlightRotation = new Vector3(90, 0, 0); // X, Y, Z ȸ����
            transform.localRotation = Quaternion.Euler(flashlightRotation);
            //Quaternion.identity; //Quaternion.identity >> ȸ�� �ʱ�ȭ(0,0,0)

            // �������� ȹ���ϸ� UI Ȱ��ȭ
            if (batteryUIObject != null)
            {
                batteryUIObject.SetActive(true);
            }
        }
    }

    public void ToggleLight(bool state) //������ ��ų� ���͸� ���� ��
    {
        if (!hasFlashlight || flashlightLight == null) return;

        isFlashlightOn = state;
        flashlightLight.enabled = isFlashlightOn;
    }

    public void ToggleLight() //������ ���� on / off
    {
        Debug.Log("������ ���� ToggleLight() Ȱ��ȭ");
        if (!hasFlashlight)
        {
            Debug.Log("�������� �����ϴ�");
            return;
        }

        if (flashlightLight == null)
        {
            Debug.LogError("flashlightLight�� null�Դϴ�! Light ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        if (batteryLevel > 0)
        {
            isFlashlightOn = !isFlashlightOn;
            flashlightLight.enabled = isFlashlightOn;
            Debug.Log("������ ���� ����: " + isFlashlightOn);
        }
        else
        {
            Debug.Log("������ ���͸��� �����Ͽ� ������ �� �� �����ϴ�");
        }
    }

    public void ChargeBattery()
    {
        batteryLevel += batteryChargeAmount;
        batteryLevel = Mathf.Min(batteryLevel, 100f); //�ִ�ġ 100f ���� �ʵ���
        UpdateBatteryUI();
    }

    private void UpdateBatteryUI()
    {
        if (batteryUI != null)
        {
            Debug.Log("���͸� �ܷ� : " + batteryLevel);
            batteryUI.UpdateBatteryUI(batteryLevel);
        }
    }
}








*/












/*
 * 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{

    [Header("Setting")]
    bool OnFlashLight; //������ ����ġ
    static bool userGetLight; //������ ȹ�� ���� Ȯ�� ����
    Light myLight; //Light ������Ʈ ��� ����

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        userGetLight = false;
        OnFlashLight = false; //������ off ����
        myLight = this.GetComponent<Light>(); //������Ʈ�� ���� Light�� ������
    }

    // Update is called once per frame
    void Update()
    {
        lightOnOff();
       

        //������ ��� ����
        if (OnFlashLight == true)
        {
            myLight.intensity = 10; //������ on
        }
        else
        {
            myLight.intensity = 0; //������ off
        }
    }


    static internal void GetLight()
    {
        if (Input.GetButtonDown("Interact"))
        {
            userGetLight = true;
            Destroy(GameObject.FindGameObjectWithTag("FlashLight"));
        }
    }

    void lightOnOff()
    {
        if(userGetLight == true)
        {
            //rŰ�� ������ on / off
            if (Input.GetKeyDown(KeyCode.R))
            {
                OnFlashLight = OnFlashLight ? false : true; //��� ���
            }
        } 
    }
}
*/