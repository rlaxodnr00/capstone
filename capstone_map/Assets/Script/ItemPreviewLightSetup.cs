using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ItemPreviewLightSetup : MonoBehaviour
{
    private void Awake()
    {
        SetupInventoryLight();
    }

    private void SetupInventoryLight()
    {
        // ������ ����Ʈ�� �̹� �پ��ִٸ� �ߺ� ���� ����
        if (transform.Find("InventoryLight") != null) return;

        // ����Ʈ ����
        GameObject lightObj = new GameObject("InventoryLight");
        lightObj.transform.SetParent(transform);
        lightObj.transform.localPosition = new Vector3(0, 2f, -2f);
        lightObj.transform.LookAt(transform.position);

        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.2f;
        light.shadowBias = 1f;
        light.cullingMask = LayerMask.GetMask("InventoryLayer"); // InventoryLayer������
        light.shadows = LightShadows.None; // �ʿ��ϸ� Soft �Ǵ� Hard�� ����
    }
}
