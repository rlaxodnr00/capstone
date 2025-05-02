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
        

        var rot = item.GetComponent<HoldedItemRotation>();
        if(rot != null)
        {
            rot.ApplyHeld(targetTransform);
        }
        else
        {
            item.transform.SetParent(targetTransform);
            item.transform.localPosition = Vector3.zero;//�÷��̾� �� ��ġ�� �̵�
            item.transform.localRotation = Quaternion.identity; //�������� �ٶ󺸴� ȸ���� ����
        }

        

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

        // feetPoint���� y�� �ణ ���� ��ӵǵ��� ��
        Vector3 dropPosition = feetPoint.position + Vector3.up * 0.2f; 

        //��� ��ġ ��Ȯ�� ���� �ڵ��ε� GPT�� ĳ���ؼ� �� ��
        if (Physics.Raycast(dropPosition, Vector3.down, out RaycastHit hit, 5f))
        {
            Collider col = currentItem.GetComponentInChildren<Collider>();
            dropPosition = hit.point + Vector3.up * (col?.bounds.extents.y ?? 0.2f);
        }

        var rot = currentItem.GetComponent<HoldedItemRotation>();
        if (rot != null)
        {
            rot.DropHeld(dropPosition);
        }
        else
        {
            // ������ ��ġ �� ȸ�� �ʱ�ȭ�Ͽ� �ʵ忡 ���� ��ġ��
            currentItem.transform.SetParent(null); //�κ��丮�� �θ���� ����
            currentItem.transform.position = dropPosition;
            currentItem.transform.rotation = Quaternion.identity;
            
        }
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