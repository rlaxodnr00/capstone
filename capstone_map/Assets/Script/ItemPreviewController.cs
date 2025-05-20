using UnityEngine;

// �������� ������ ī�޶� ���������� �����ϴ� Ŭ����
public class ItemPreviewController : MonoBehaviour
{
    [Header("������ ������ ���� ī�޶�")]
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
            Debug.Log("ItemPreviewController�� previewCamera ������ �Ҵ���� �ʾҽ��ϴ�.");
        }
    }


    // ������ ī�޶� Ȱ��ȭ �� ���� �ؽ�ó �Ҵ�
    public void ActivatePreview(RenderTexture targetRenderTexture)
    {
        if (previewCamera == null) return;

        previewCamera.targetTexture = targetRenderTexture;
        previewCamera.enabled = true;

        if (previewLight != null)
            previewLight.enabled = true;

    }

    // ������ ī�޶� ��Ȱ��ȭ
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

    void Awake() //���� �� ī�޶� ����
    {
        if (previewCamera != null)
        {
            previewCamera.enabled = false;
            previewCamera.targetTexture = null;
        }
    }

    // ���Կ� ���� RenderTexture �Ҵ�
    public void SetPreviewRenderTexture(int slotIndex)
    {
        if (previewCamera == null) return;

        previewCamera.targetTexture = slotIndex == 0 ? renderTexture1 : renderTexture2;
        Debug.Log($"{gameObject.name} �� PreviewTexture ����: {previewCamera.targetTexture}");
    }

    // ������ ī�޶� On/Off
    public void SetPreviewCameraActive(bool active)
    {
        if (previewCamera != null)
        {
            previewCamera.enabled = active;
        }
    }
}
*/