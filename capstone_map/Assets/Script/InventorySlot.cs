using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("아이템 프리뷰 표시용")]
    public RenderTexture previewRenderTexture;
    public RawImage previewImage;

    private GameObject storedItem;
    private Camera previewCamera;

    public void SetItem(GameObject item)
    {
        ClearSlot();
        storedItem = item;

        // 아이템 비활성화 후 인벤토리 내부로 이동
        storedItem.transform.SetParent(transform);
        storedItem.SetActive(false);

        CreatePreviewCamera(item);
    }

    public GameObject RemoveItem()
    {
        if (storedItem != null)
        {
            if (previewCamera != null) Destroy(previewCamera.gameObject);
            GameObject result = storedItem;
            storedItem = null;
            previewCamera = null;
            return result;
        }
        return null;
    }

    public bool HasItem()
    {
        return storedItem != null;
    }

    public GameObject GetItem()
    {
        return storedItem;
    }

    private void CreatePreviewCamera(GameObject target)
    {
        GameObject camObj = new GameObject("PreviewCamera");
        camObj.transform.SetParent(target.transform);
        camObj.transform.localPosition = new Vector3(0, 0, -0.5f);
        camObj.transform.localRotation = Quaternion.identity;

        previewCamera = camObj.AddComponent<Camera>();
        previewCamera.clearFlags = CameraClearFlags.SolidColor;
        previewCamera.backgroundColor = new Color(0, 0, 0, 0);
        previewCamera.orthographic = true;
        previewCamera.orthographicSize = 0.3f;
        previewCamera.cullingMask = LayerMask.GetMask("InventoryLayer");
        previewCamera.targetTexture = previewRenderTexture;

        previewImage.texture = previewRenderTexture;

        SetLayerRecursively(target.transform, LayerMask.NameToLayer("InventoryLayer"));
    }

    private void SetLayerRecursively(Transform target, int layer)
    {
        target.gameObject.layer = layer;
        foreach (Transform child in target)
        {
            SetLayerRecursively(child, layer);
        }
    }

    public void ClearSlot()
    {
        if (previewCamera != null) Destroy(previewCamera.gameObject);
        if (storedItem != null) Destroy(storedItem);
        previewCamera = null;
        storedItem = null;
    }
}
