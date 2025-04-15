using UnityEngine;

/*
 * 플레이어의 인벤토리 관련 기능 수행.
*/
public class PlayerInventory : MonoBehaviour
{
    [Header("인벤토리에 보관되는 아이템 배열")]
    public GameObject[] heldItems = new GameObject[2]; //(기본 2개)

    [Header("아이템 장착 위치")]
    public Transform handTransform;
    public Transform waistTransform;

    [Header("아이템 드롭 위치")]
    public Transform feetPoint;

    [Header("인벤토리 UI 오브젝트")]
    public InventoryUIController uiController; //인벤토리 총괄 오브젝트
    public RenderTexture[] inventoryRenderTextures; // 슬롯별 RenderTexture 배열(2개)

    private int currentSlot = 0; //현재 선택 슬롯 인덱스

    void Update()
    {
        //숫자 키 1 2 번으로 인벤토리 변경
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetCurrentSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetCurrentSlot(1);

        //q로 아이템 드롭
        if (Input.GetKeyDown(KeyCode.Q)) DropCurrentItem();

        //f로 인벤토리의 아이템 상호작용
        if (Input.GetKeyDown(KeyCode.F)) InteractCurrentItem();
    }

    // 선택 슬롯에 따른 동작
    void SetCurrentSlot(int slotIndex)
    {
        if (slotIndex == currentSlot) return; //이미 선택된 슬롯 인덱스 누르면 리턴

        //다른 인덱스 슬롯 선택 시 동작
        currentSlot = slotIndex;
        uiController.SetSelectedSlot(slotIndex);
        UpdateHeldItemsDisplay();
    }

    //인벤토리에 새로운 아이템 추가
    public bool TryPickup(GameObject newItem)
    {
        if (heldItems[currentSlot] == null)
            AssignItemToSlot(currentSlot, newItem); //선택된 인벤토리 슬롯에 아이템이 없으면 아이템 할당
        else
            SwapItems(currentSlot, newItem); //아이템이 존재하는 슬롯이면 각 아이템 스왑

        return true;
    }

    //현재 선택된 슬롯 아이템 드랍
    public void DropCurrentItem() 
    {
        DropItem(currentSlot);
    }

    void InteractCurrentItem()
    {
        // 현재 슬롯에 아이템이 있는지 확인
        if (heldItems[currentSlot] != null)
        {
            // IInventoryInteractable 인터페이스가 붙은 컴포넌트인지 확인
            var inventoryInteractable = heldItems[currentSlot].GetComponent<IInventoryInteractable>();
            if (inventoryInteractable != null)
            {
                // 해당 아이템의 InventoryInteract() 메서드 호출
                inventoryInteractable.InventoryInteract();
            }
        }
    }


    //선택된 인벤토리 슬롯에 아이템이 없으면 아이템 할당
    void AssignItemToSlot(int slotIndex, GameObject item)
    {
        heldItems[slotIndex] = item; //현재 슬롯에 아이템 배정
        
        //현재 슬롯이면 손, 아니면 허리에 위치시킴
        Transform targetTransform = (slotIndex == currentSlot) ? handTransform : waistTransform;
        item.transform.SetParent(targetTransform);
        item.transform.localPosition = Vector3.zero;//플레이어 손 위치로 이동
        item.transform.localRotation = Quaternion.identity; //아이템이 바라보는 회전각 변경

        // Rigidbody가 있다면, 인벤토리에 있는 동안 물리 시뮬레이션을 중지시킴
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;   // 물리 영향을 끄기
            rb.useGravity = false;    // 중력을 비활성화
        }


        //인벤토리 프리뷰 카메라 기능 활성화하여 인벤토리에 아이템이 보이게 함
        var previewController = item.GetComponent<ItemPreviewController>();
        previewController?.ActivatePreview(inventoryRenderTextures[slotIndex]);
        uiController.SetSlotPreviewTexture(slotIndex, inventoryRenderTextures[slotIndex]);

        item.SetActive(true);
    }

    //아이템이 존재하는 슬롯이면 각 아이템 스왑
    void SwapItems(int slotIndex, GameObject newItem)
    {
        DropItem(slotIndex);
        AssignItemToSlot(slotIndex, newItem);
    }

    //선택된 슬롯 아이템 드롭과 후처리
    void DropItem(int slotIndex)
    {
        GameObject currentItem = heldItems[slotIndex];
        if (currentItem == null) return; //선택된 슬롯이 비어있으면 리턴

        currentItem.transform.SetParent(null); //인벤토리와 부모관계 해제

        // feetPoint보다 y축 약간 위에 드롭되도록 함
        Vector3 dropPosition = feetPoint.position + Vector3.up * 0.2f; 

        //드롭 위치 정확도 개선 코드인데 GPT가 캐리해서 잘 모름
        if (Physics.Raycast(dropPosition, Vector3.down, out RaycastHit hit, 5f))
        {
            Collider col = currentItem.GetComponentInChildren<Collider>();
            dropPosition = hit.point + Vector3.up * (col?.bounds.extents.y ?? 0.2f);
        }

        // 아이템 위치 및 회전 초기화하여 필드에 곱게 배치함
        currentItem.transform.position = dropPosition;
        currentItem.transform.rotation = Quaternion.identity;
        currentItem.SetActive(true);


        // 인벤토리 프리뷰 카메라 및 UI 정리 << 이 내용 나중에 점검
        var previewController = currentItem.GetComponent<ItemPreviewController>();
        previewController?.DeactivatePreview(); // 프리뷰 비활성화
        uiController.ClearSlotPreviewTexture(slotIndex); // UI 텍스쳐 클리어

        /*
         if (previewController != null)
    {
        previewController.DeactivatePreview();
    }
    GameUIManager.Instance.ClearInventorySlotPreviewTexture(slotIndex);
        이걸로도 테스트
        */

        // 아이템 드랍 시 개별 동작
        ICustomDrop dropHandler = currentItem.GetComponent<ICustomDrop>();
        if (dropHandler != null)
        {
            dropHandler.CustomDrop();
        }

        heldItems[slotIndex] = null; //손 비우기
    }

    // 인벤토리에 보관된 모든 아이템의 위치와 부모 할당을 슬롯 상태에 맞게 업데이트
    void UpdateHeldItemsDisplay()
    {
        for (int i = 0; i < heldItems.Length; i++)
        {
            if (heldItems[i] != null) //손에 아이템 있으면
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


    //현재 손에 쥔 아이템 외부 참조용
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
    [Header("인벤토리 슬롯 (슬롯 2개 고정)")]
    public GameObject[] heldItems = new GameObject[2];
    public Transform handTransform; //손 위치
    public Transform waistTransform; // 허리 위치

    [Header("슬롯 관련")]
    public InventoryUIController uiController;
    public GameObject[] itemPreviewObjects; // 실제 아이템 프리뷰 (RenderTexture를 가진 RawImage 오브젝트) << 이거  없어도 되나?
    private int currentSlot = 0;

    [Header("드롭 설정")]
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
            itemPreviewObjects[slotIndex].SetActive(true); // 프리뷰 이미지 보여주기
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
                itemPreviewObjects[slotIndex].SetActive(false); // 이미지 끄기
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

                //  아이템의 위치 이동 (손 or 허리)
                Transform targetTransform = isHeld ? handTransform : waistTransform;
                heldItems[i].transform.SetParent(targetTransform);
                heldItems[i].transform.localPosition = Vector3.zero;
                heldItems[i].transform.localRotation = Quaternion.identity;

                //  필요 시 아이템별 커스텀 위치 적용
                Item itemComp = heldItems[i].GetComponent<Item>();
                if (itemComp != null)
                {
                    itemComp.SetPreviewCameraActive(true);
                   // itemComp.ApplyTransform(targetTransform, isHeld); 
                }

                //  UI는 항상 보이게
                if (itemPreviewObjects.Length > i)
                {
                    itemPreviewObjects[i].SetActive(true);
                }

                //  실제 Scene 상에서는 손에 든 아이템만 Active (선택)
                heldItems[i].SetActive(true); // 혹시 이미지 사라지면 false → true 바꿔줘
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
            itemPreviewObjects[currentSlot].SetActive(false); // 드롭한 슬롯의 이미지 끄기
        }

        UpdateHeldItemDisplay();
    }
}

*/