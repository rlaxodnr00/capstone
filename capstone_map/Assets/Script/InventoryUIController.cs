using UnityEngine;
using UnityEngine.UI;

// �κ��丮 ������ UI�� �����ϴ� Ŭ����
public class InventoryUIController : MonoBehaviour
{
    [Header("���� ���� ȿ�� (�θ� ������Ʈ �迭)")]
    public GameObject[] slotPreviewParents; // ���� ���� ȿ�� ������ (ex: scale ����)

    [Header("���� �̸����� �̹��� (RawImage �迭)")]
    public RawImage[] slotPreviewRawImages; // ������ �̸����� RenderTexture ����

    private int currentSelectedSlot = -1;

    // ���� ���� �� ���� ȿ�� ����
    public void SetSelectedSlot(int index)
    {
        if (index == currentSelectedSlot) return;

        for (int i = 0; i < slotPreviewParents.Length; i++)
        {
            slotPreviewParents[i].transform.localScale = (i == index) ? Vector3.one * 1.2f : Vector3.one;
        }

        currentSelectedSlot = index;
    }

    // RenderTexture ���� �� �̸����� �̹��� ����
    public void SetSlotPreviewTexture(int index, RenderTexture texture)
    {
        if (slotPreviewRawImages.Length > index)
            slotPreviewRawImages[index].texture = texture;
    }

    // RenderTexture ���� ����
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
    [Header("�κ��丮 ���� UI ������Ʈ")]
    public GameObject[] slotPreviewImages; // ex: ItemPreview_LeftEye, ItemPreview_RightEye �� �θ� (RawImage ����)

    private int currentSelected = -1;

    // ���� ���� ó���� ���� (��Ȱ��ȭ X)
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