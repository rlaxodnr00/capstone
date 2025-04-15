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
        // 기존에 라이트가 이미 붙어있다면 중복 생성 방지
        if (transform.Find("InventoryLight") != null) return;

        // 라이트 생성
        GameObject lightObj = new GameObject("InventoryLight");
        lightObj.transform.SetParent(transform);
        lightObj.transform.localPosition = new Vector3(0, 2f, -2f);
        lightObj.transform.LookAt(transform.position);

        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.2f;
        light.shadowBias = 1f;
        light.cullingMask = LayerMask.GetMask("InventoryLayer"); // InventoryLayer만照射
        light.shadows = LightShadows.None; // 필요하면 Soft 또는 Hard로 변경
    }
}
