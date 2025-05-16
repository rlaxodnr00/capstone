using System.Collections;
using UnityEngine;

public class ApplyPhysicMaterialToFloors : MonoBehaviour
{
    [Tooltip("물리 머티리얼을 적용할 최상위 부모 오브젝트입니다.")]
    public GameObject mapRoot;

    [Tooltip("바닥 오브젝트에 적용할 물리 머티리얼입니다.")]
    public PhysicsMaterial floorPhysicMaterial;

    [Tooltip("맵 생성 대기 시간 (초)")]
    public float initializationDelay = 2.0f; // 기본 2초 딜레이


    IEnumerator Start()
    {
        yield return new WaitForSeconds(initializationDelay);
        ApplyMaterial();
    }


    public void ApplyMaterial()
    {
        if (mapRoot == null)
        {
            mapRoot = GameObject.Find("Generated Map");
            if (mapRoot == null)
            {
                Debug.LogError("Map Root ('Generated Map')를 찾을 수 없습니다. 맵이 생성되었는지 또는 이름이 올바른지 확인해주세요. 인스펙터에서 mapRoot 변수를 직접 설정할 수도 있습니다.");
                return;
            }
        }

        if (floorPhysicMaterial == null)
        {
            Debug.LogError("Floor Physic Material이 할당되지 않았습니다. 인스펙터에서 floorPhysicMaterial 변수를 설정해주세요.");
            return;
        }

        Debug.Log($"'{initializationDelay}'초 대기 후 물리 머티리얼 적용을 시작합니다. 대상 루트: {mapRoot.name}");

        // mapRoot 및 그 모든 하위 오브젝트에서 MeshCollider를 찾습니다. (비활성화된 오브젝트 포함)
        MeshCollider[] meshColliders = mapRoot.GetComponentsInChildren<MeshCollider>(true);

        int appliedCount = 0;
        foreach (MeshCollider meshCollider in meshColliders)
        {
            GameObject obj = meshCollider.gameObject;
            // 오브젝트 이름에 "floor" (대소문자 구분 없이)가 포함되어 있는지 확인합니다.
            if (obj.name.ToLower().Contains("floor"))
            {
                meshCollider.material = floorPhysicMaterial;
                meshCollider.convex = true; // Convex 옵션 활성화
                Debug.Log($"'{obj.name}' 오브젝트의 MeshCollider에 물리 머티리얼 '{floorPhysicMaterial.name}'을 적용하고 Convex를 활성화했습니다.");
                appliedCount++;
            }
        }
        Debug.Log($"물리 머티리얼 적용 작업 완료. 총 {appliedCount}개의 'floor' 오브젝트에 적용되었습니다.");
    }
}