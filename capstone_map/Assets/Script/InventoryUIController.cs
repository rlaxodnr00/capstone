using UnityEngine;
using UnityEngine.UI;

// 인벤토리 슬롯의 UI를 관리하는 클래스
public class InventoryUIController : MonoBehaviour
{
    [Header("슬롯 강조 효과 (부모 오브젝트 배열)")]
    public GameObject[] slotPreviewParents; // 슬롯 강조 효과 관리용 (ex: scale 조정)

    [Header("슬롯 미리보기 이미지 (RawImage 배열)")]
    public RawImage[] slotPreviewRawImages; // 아이템 미리보기 RenderTexture 연결

    private int currentSelectedSlot = -1;

    // 슬롯 선택 및 강조 효과 설정
    public void SetSelectedSlot(int index)
    {
        if (index == currentSelectedSlot) return;

        for (int i = 0; i < slotPreviewParents.Length; i++)
        {
            slotPreviewParents[i].transform.localScale = (i == index) ? Vector3.one * 1.2f : Vector3.one;
        }

        currentSelectedSlot = index;
    }

    // RenderTexture 연결 및 미리보기 이미지 설정
    public void SetSlotPreviewTexture(int index, RenderTexture texture)
    {
        if (slotPreviewRawImages.Length > index)
            slotPreviewRawImages[index].texture = texture;
    }

    // RenderTexture 연결 해제
    public void ClearSlotPreviewTexture(int index)
    {
        if (slotPreviewRawImages.Length > index)
        {
            slotPreviewRawImages[index].texture = null;
        }
    }
}

/*
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [Header("인벤토리 슬롯 UI 오브젝트")]
    public GameObject[] slotPreviewImages; // ex: ItemPreview_LeftEye, ItemPreview_RightEye 의 부모 (RawImage 포함)

    private int currentSelected = -1;

    // 슬롯 강조 처리만 수행 (비활성화 X)
    public void SetSelectedSlot(int index)
    {
        if (index == currentSelected) return;

        for (int i = 0; i < slotPreviewImages.Length; i++)
        {
            var image = slotPreviewImages[i].transform;
            image.localScale = (i == index) ? Vector3.one * 1.2f : Vector3.one;
        }

        currentSelected = index;
    }
}
*/