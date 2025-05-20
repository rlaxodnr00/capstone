using UnityEngine;

// 아이템의 프리뷰 카메라를 독립적으로 관리하는 클래스
public class ItemPreviewController : MonoBehaviour
{
    [Header("아이템 프리뷰 전용 카메라")]
    public Camera previewCamera;
    public Light previewLight;

    void Awake()
    {
        if (previewCamera != null)
        {
            previewCamera.enabled = false;
            previewCamera.targetTexture = null;
        }
        else
        {
            Debug.Log("ItemPreviewController의 previewCamera 변수가 할당되지 않았습니다.");
        }
    }


    // 프리뷰 카메라 활성화 및 렌더 텍스처 할당
    public void ActivatePreview(RenderTexture targetRenderTexture)
    {
        if (previewCamera == null) return;

        previewCamera.targetTexture = targetRenderTexture;
        previewCamera.enabled = true;

        if (previewLight != null)
            previewLight.enabled = true;

    }

    // 프리뷰 카메라 비활성화
    public void DeactivatePreview()
    {
        if (previewCamera == null) return;

        if (previewCamera.targetTexture != null)
        {
            RenderTexture.active = previewCamera.targetTexture;
            GL.Clear(true, true, Color.clear);
            RenderTexture.active = null;
        }

        previewCamera.enabled = false;
        previewCamera.targetTexture = null;

        if (previewLight != null)
            previewLight.enabled = false;
    }
}


/*
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Item : MonoBehaviour
{
    public Camera previewCamera;

    public RenderTexture renderTexture1;
    public RenderTexture renderTexture2;

    void Awake() //시작 시 카메라 끄기
    {
        if (previewCamera != null)
        {
            previewCamera.enabled = false;
            previewCamera.targetTexture = null;
        }
    }

    // 슬롯에 따라 RenderTexture 할당
    public void SetPreviewRenderTexture(int slotIndex)
    {
        if (previewCamera == null) return;

        previewCamera.targetTexture = slotIndex == 0 ? renderTexture1 : renderTexture2;
        Debug.Log($"{gameObject.name} → PreviewTexture 연결: {previewCamera.targetTexture}");
    }

    // 프리뷰 카메라 On/Off
    public void SetPreviewCameraActive(bool active)
    {
        if (previewCamera != null)
        {
            previewCamera.enabled = active;
        }
    }
}
*/