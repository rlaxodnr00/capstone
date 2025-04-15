using UnityEngine;

/*
 * �÷��̾��� �κ��丮 ���� ��� ����.
*/
public class PlayerInventory : MonoBehaviour
{
    [Header("�κ��丮�� �����Ǵ� ������ �迭")]
    public GameObject[] heldItems = new GameObject[2]; //(�⺻ 2��)

    [Header("������ ���� ��ġ")]
    public Transform handTransform;
    public Transform waistTransform;

    [Header("������ ��� ��ġ")]
    public Transform feetPoint;

    [Header("�κ��丮 UI ������Ʈ")]
    public InventoryUIController uiController; //�κ��丮 �Ѱ� ������Ʈ
    public RenderTexture[] inventoryRenderTextures; // ���Ժ� RenderTexture �迭(2��)

    private int currentSlot = 0; //���� ���� ���� �ε���

    void Update()
    {
        //���� Ű 1 2 ������ �κ��丮 ����
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetCurrentSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetCurrentSlot(1);

        //q�� ������ ���
        if (Input.GetKeyDown(KeyCode.Q)) DropCurrentItem();

        //f�� �κ��丮�� ������ ��ȣ�ۿ�
        if (Input.GetKeyDown(KeyCode.F)) InteractCurrentItem();
    }

    // ���� ���Կ� ���� ����
    void SetCurrentSlot(int slotIndex)
    {
        if (slotIndex == currentSlot) return; //�̹� ���õ� ���� �ε��� ������ ����

        //�ٸ� �ε��� ���� ���� �� ����
        currentSlot = slotIndex;
        uiController.SetSelectedSlot(slotIndex);
        UpdateHeldItemsDisplay();
    }

    //�κ��丮�� ���ο� ������ �߰�
    public bool TryPickup(GameObject newItem)
    {
        if (heldItems[currentSlot] == null)
            AssignItemToSlot(currentSlot, newItem); //���õ� �κ��丮 ���Կ� �������� ������ ������ �Ҵ�
        else
            SwapItems(currentSlot, newItem); //�������� �����ϴ� �����̸� �� ������ ����

        return true;
    }

    //���� ���õ� ���� ������ ���
    public void DropCurrentItem() 
    {
        DropItem(currentSlot);
    }

    void InteractCurrentItem()
    {
        // ���� ���Կ� �������� �ִ��� Ȯ��
        if (heldItems[currentSlot] != null)
        {
            // IInventoryInteractable �������̽��� ���� ������Ʈ���� Ȯ��
            var inventoryInteractable = heldItems[currentSlot].GetComponent<IInventoryInteractable>();
            if (inventoryInteractable != null)
            {
                // �ش� �������� InventoryInteract() �޼��� ȣ��
                inventoryInteractable.InventoryInteract();
            }
        }
    }


    //���õ� �κ��丮 ���Կ� �������� ������ ������ �Ҵ�
    void AssignItemToSlot(int slotIndex, GameObject item)
    {
        heldItems[slotIndex] = item; //���� ���Կ� ������ ����
        
        //���� �����̸� ��, �ƴϸ� �㸮�� ��ġ��Ŵ
        Transform targetTransform = (slotIndex == currentSlot) ? handTransform : waistTransform;
        item.transform.SetParent(targetTransform);
        item.transform.localPosition = Vector3.zero;//�÷��̾� �� ��ġ�� �̵�
        item.transform.localRotation = Quaternion.identity; //�������� �ٶ󺸴� ȸ���� ����

        // Rigidbody�� �ִٸ�, �κ��丮�� �ִ� ���� ���� �ùķ��̼��� ������Ŵ
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;   // ���� ������ ����
            rb.useGravity = false;    // �߷��� ��Ȱ��ȭ
        }


        //�κ��丮 ������ ī�޶� ��� Ȱ��ȭ�Ͽ� �κ��丮�� �������� ���̰� ��
        var previewController = item.GetComponent<ItemPreviewController>();
        previewController?.ActivatePreview(inventoryRenderTextures[slotIndex]);
        uiController.SetSlotPreviewTexture(slotIndex, inventoryRenderTextures[slotIndex]);

        item.SetActive(true);
    }

    //�������� �����ϴ� �����̸� �� ������ ����
    void SwapItems(int slotIndex, GameObject newItem)
    {
        DropItem(slotIndex);
        AssignItemToSlot(slotIndex, newItem);
    }

    //���õ� ���� ������ ��Ӱ� ��ó��
    void DropItem(int slotIndex)
    {
        GameObject currentItem = heldItems[slotIndex];
        if (currentItem == null) return; //���õ� ������ ��������� ����

        currentItem.transform.SetParent(null); //�κ��丮�� �θ���� ����

        // feetPoint���� y�� �ణ ���� ��ӵǵ��� ��
        Vector3 dropPosition = feetPoint.position + Vector3.up * 0.2f; 

        //��� ��ġ ��Ȯ�� ���� �ڵ��ε� GPT�� ĳ���ؼ� �� ��
        if (Physics.Raycast(dropPosition, Vector3.down, out RaycastHit hit, 5f))
        {
            Collider col = currentItem.GetComponentInChildren<Collider>();
            dropPosition = hit.point + Vector3.up * (col?.bounds.extents.y ?? 0.2f);
        }

        // ������ ��ġ �� ȸ�� �ʱ�ȭ�Ͽ� �ʵ忡 ���� ��ġ��
        currentItem.transform.position = dropPosition;
        currentItem.transform.rotation = Quaternion.identity;
        currentItem.SetActive(true);


        // �κ��丮 ������ ī�޶� �� UI ���� << �� ���� ���߿� ����
        var previewController = currentItem.GetComponent<ItemPreviewController>();
        previewController?.DeactivatePreview(); // ������ ��Ȱ��ȭ
        uiController.ClearSlotPreviewTexture(slotIndex); // UI �ؽ��� Ŭ����

        /*
         if (previewController != null)
    {
        previewController.DeactivatePreview();
    }
    GameUIManager.Instance.ClearInventorySlotPreviewTexture(slotIndex);
        �̰ɷε� �׽�Ʈ
        */

        // ������ ��� �� ���� ����
        ICustomDrop dropHandler = currentItem.GetComponent<ICustomDrop>();
        if (dropHandler != null)
        {
            dropHandler.CustomDrop();
        }

        heldItems[slotIndex] = null; //�� ����
    }

    // �κ��丮�� ������ ��� �������� ��ġ�� �θ� �Ҵ��� ���� ���¿� �°� ������Ʈ
    void UpdateHeldItemsDisplay()
    {
        for (int i = 0; i < heldItems.Length; i++)
        {
            if (heldItems[i] != null) //�տ� ������ ������
            {
                bool isHeld = (i == currentSlot);
                Transform targetTransform = isHeld ? handTransform : waistTransform;
                heldItems[i].transform.SetParent(targetTransform);
                heldItems[i].transform.localPosition = Vector3.zero;
                heldItems[i].transform.localRotation = Quaternion.identity;
                heldItems[i].SetActive(true);
            }
        }
    }


    //���� �տ� �� ������ �ܺ� ������
    public GameObject GetCurrentItem()
    {
        return heldItems[currentSlot];
    }
}



/*
 * 
 * using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("�κ��丮 ���� (���� 2�� ����)")]
    public GameObject[] heldItems = new GameObject[2];
    public Transform handTransform; //�� ��ġ
    public Transform waistTransform; // �㸮 ��ġ

    [Header("���� ����")]
    public InventoryUIController uiController;
    public GameObject[] itemPreviewObjects; // ���� ������ ������ (RenderTexture�� ���� RawImage ������Ʈ) << �̰�  ��� �ǳ�?
    private int currentSlot = 0;

    [Header("��� ����")]
    public float dropForwardDistance = 2f;
    public Transform feetPoint;

    void Update()
    {
        HandleSlotInput();
        HandleDropInput();
    }

    void HandleSlotInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetCurrentSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetCurrentSlot(1);
    }

    void HandleDropInput()
    {
        if (Input.GetKeyDown(KeyCode.Q)) DropItem();
    }

    void SetCurrentSlot(int slotIndex)
    {
        if (slotIndex == currentSlot) return;
        currentSlot = slotIndex;
        uiController?.SetSelectedSlot(slotIndex);
        UpdateHeldItemDisplay();
    }

    public bool TryPickup(GameObject newItem)
    {
        if (heldItems[currentSlot] == null)
        {
            AssignItemToSlot(currentSlot, newItem);
        }
        else
        {
            SwapItems(currentSlot, newItem);
        }

        return true;
    }

    void AssignItemToSlot(int slotIndex, GameObject item)
    {
        heldItems[slotIndex] = item;

        item.transform.SetParent(handTransform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        Item itemComp = item.GetComponent<Item>();
        if (itemComp != null)
        {
            itemComp.SetPreviewRenderTexture(slotIndex);
            itemComp.SetPreviewCameraActive(true);
        }

        if (itemPreviewObjects.Length > slotIndex)
        {
            itemPreviewObjects[slotIndex].SetActive(true); // ������ �̹��� �����ֱ�
        }

        item.SetActive(slotIndex == currentSlot);
        UpdateHeldItemDisplay();
    }

    void SwapItems(int slotIndex, GameObject newItem)
    {
        GameObject currentItem = heldItems[slotIndex];

        if (currentItem != null)
        {
            currentItem.transform.SetParent(null);
            Vector3 dropPos = feetPoint.position + Vector3.up * 0.2f;

            if (Physics.Raycast(dropPos, Vector3.down, out RaycastHit hit, 5f))
            {
                dropPos = hit.point;
                Collider col = currentItem.GetComponentInChildren<Collider>();
                dropPos += Vector3.up * (col?.bounds.extents.y ?? 0.2f);
            }

            currentItem.transform.position = dropPos;
            currentItem.transform.rotation = Quaternion.identity;
            currentItem.SetActive(true);

            Item itemComp = currentItem.GetComponent<Item>();
            if (itemComp != null)
            {
                itemComp.SetPreviewCameraActive(false);
            }

            if (itemPreviewObjects.Length > slotIndex)
            {
                itemPreviewObjects[slotIndex].SetActive(false); // �̹��� ����
            }
        }

        AssignItemToSlot(slotIndex, newItem);
    }

    void UpdateHeldItemDisplay()
    {
        for (int i = 0; i < heldItems.Length; i++)
        {
            if (heldItems[i] != null)
            {
                bool isHeld = (i == currentSlot);

                //  �������� ��ġ �̵� (�� or �㸮)
                Transform targetTransform = isHeld ? handTransform : waistTransform;
                heldItems[i].transform.SetParent(targetTransform);
                heldItems[i].transform.localPosition = Vector3.zero;
                heldItems[i].transform.localRotation = Quaternion.identity;

                //  �ʿ� �� �����ۺ� Ŀ���� ��ġ ����
                Item itemComp = heldItems[i].GetComponent<Item>();
                if (itemComp != null)
                {
                    itemComp.SetPreviewCameraActive(true);
                   // itemComp.ApplyTransform(targetTransform, isHeld); 
                }

                //  UI�� �׻� ���̰�
                if (itemPreviewObjects.Length > i)
                {
                    itemPreviewObjects[i].SetActive(true);
                }

                //  ���� Scene �󿡼��� �տ� �� �����۸� Active (����)
                heldItems[i].SetActive(true); // Ȥ�� �̹��� ������� false �� true �ٲ���
            }
        }
    }

    void DropItem()
    {
        GameObject currentItem = heldItems[currentSlot];
        if (currentItem == null) return;

        heldItems[currentSlot] = null;
        currentItem.transform.SetParent(null);

        Vector3 rayOrigin = feetPoint.position + Vector3.up * 0.2f;
        Vector3 dropPosition = rayOrigin + transform.forward * 1f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 5f))
        {
            dropPosition = hit.point;
            Collider col = currentItem.GetComponentInChildren<Collider>();
            dropPosition += Vector3.up * (col?.bounds.extents.y ?? 0.2f);
        }

        currentItem.transform.position = dropPosition;
        currentItem.transform.rotation = Quaternion.identity;
        currentItem.SetActive(true);

        Item itemComp = currentItem.GetComponent<Item>();
        if (itemComp != null)
        {
            itemComp.SetPreviewCameraActive(false);
        }

        if (itemPreviewObjects.Length > currentSlot)
        {
            itemPreviewObjects[currentSlot].SetActive(false); // ����� ������ �̹��� ����
        }

        UpdateHeldItemDisplay();
    }
}

*/