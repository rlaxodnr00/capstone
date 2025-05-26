using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GasVolumeController : MonoBehaviour
{
    private BoxCollider gasAreaCollider;
    private ParticleSystem gasParticleSystem;

    [Header("Map Alignment Settings")]
    [Tooltip("이 GasArea가 기준으로 삼을 맵 내의 GameObject (BoxCollider를 포함해야 함)")]
    public GameObject mapBoundaryLocator; // 맵의 기준이 될 GameObject (예: "Bounds" 오브젝트)

    [Tooltip("Awake 시 mapBoundaryLocator를 기준으로 GasArea의 Transform과 Collider를 자동 정렬할지 여부")]
    public bool alignToMapOnAwake = true;

    void Awake()
    {
        // GasArea에 있는 BoxCollider 컴포넌트를 가져옵니다.
        gasAreaCollider = GetComponent<BoxCollider>();

        // 자식 오브젝트에서 ParticleSystem 컴포넌트를 찾습니다.
        // 만약 다른 위치에 있거나 여러 개가 있다면, 직접 할당하거나 다른 방식으로 찾아야 합니다.
        gasParticleSystem = GetComponentInChildren<ParticleSystem>();

        if (gasParticleSystem == null)
        {
            Debug.LogError("GasParticleSystem을 찾을 수 없습니다. GasArea의 자식으로 ParticleSystem이 있는지 확인해주세요.", this);
            enabled = false; // 스크립트 비활성화
            return;
        }

        // RequireComponent로 인해 gasAreaCollider는 항상 존재하지만, 명시적으로 확인하는 것도 좋습니다.
        if (gasAreaCollider == null) // 이 부분은 RequireComponent로 인해 사실상 발생하지 않지만, 안전을 위해 유지 가능
        {
            Debug.LogError("BoxCollider를 찾을 수 없습니다. GasArea에 BoxCollider가 있는지 확인해주세요.", this);
            enabled = false; // 스크립트 비활성화
            return;
        }

        if (alignToMapOnAwake)
        {
            if (mapBoundaryLocator != null)
            {
                // mapBoundaryLocator 내에서 비활성화된 BoxCollider를 포함하여 찾습니다.
                // 자식 오브젝트들 중에서 첫 번째로 발견되는 BoxCollider를 사용합니다.
                BoxCollider targetMapCollider = mapBoundaryLocator.GetComponentInChildren<BoxCollider>(true); // true: 비활성 컴포넌트도 검색

                if (targetMapCollider != null)
                {
                    // GasArea의 Transform을 targetMapCollider의 GameObject의 Transform과 일치시킵니다.
                    transform.position = targetMapCollider.transform.position;
                    transform.rotation = targetMapCollider.transform.rotation;
                    transform.localScale = targetMapCollider.transform.lossyScale; // 월드 스케일을 복사

                    // GasArea의 BoxCollider의 중심과 크기를 targetMapCollider의 로컬 값과 일치시킵니다.
                    gasAreaCollider.center = targetMapCollider.center;
                    gasAreaCollider.size = targetMapCollider.size;

                    Debug.Log($"GasArea '{name}'가 '{mapBoundaryLocator.name}'의 BoxCollider ('{targetMapCollider.name}')에 맞춰 조정되었습니다. " +
                              $"새로운 Center: {gasAreaCollider.center}, 새로운 Size: {gasAreaCollider.size}", this);
                }
                else
                {
                    Debug.LogWarning($"'{mapBoundaryLocator.name}'에서 BoxCollider를 찾을 수 없습니다 (비활성 포함). GasArea가 맵 경계에 맞춰 조정되지 않았습니다.", this);
                }
            }
            else
            {
                Debug.LogWarning("'alignToMapOnAwake'가 활성화되었지만 'mapBoundaryLocator'가 할당되지 않았습니다. GasArea가 맵 경계에 맞춰 조정되지 않습니다.", this);
            }
        }

        // (맵 경계에 맞춰졌거나 기존 값 그대로인) gasAreaCollider 기준으로 파티클 시스템을 조정합니다.
        AdjustParticleSystemToCollider();
    }

    // BoxCollider의 크기와 위치에 맞춰 파티클 시스템의 Shape를 조절합니다.
    void AdjustParticleSystemToCollider()
    {
        if (gasParticleSystem == null || gasAreaCollider == null) return;

        var shapeModule = gasParticleSystem.shape;

        // 파티클 시스템의 Shape가 Box 타입인지 확인합니다.
        if (shapeModule.shapeType != ParticleSystemShapeType.Box && 
            shapeModule.shapeType != ParticleSystemShapeType.BoxShell && 
            shapeModule.shapeType != ParticleSystemShapeType.BoxEdge)
        {
            Debug.LogWarning("파티클 시스템의 Shape 타입이 Box가 아닙니다. Box 타입으로 변경해주세요.", gasParticleSystem);
            // 필요하다면 여기서 shapeModule.shapeType = ParticleSystemShapeType.Box; 로 강제 변경할 수 있습니다.
        }

        // BoxCollider의 크기를 파티클 시스템 Shape의 scale에 적용합니다.
        // Y 스케일은 BoxCollider의 Y 스케일의 25%로 설정합니다.
        Vector3 newParticleShapeScale = gasAreaCollider.size;
        newParticleShapeScale.y *= 0.25f; // Y 스케일을 25%로 줄임
        shapeModule.scale = newParticleShapeScale;

        // 파티클 시스템 Shape의 X, Z 중심은 BoxCollider의 중심을 따르고,
        // Y 중심은 파티클 Shape의 바닥이 BoxCollider의 바닥에 정렬되도록 조정합니다.
        // 파티클 시스템은 GasArea의 자식이므로, BoxCollider의 center는 로컬 좌표계 기준입니다.
        Vector3 newParticleShapePosition;
        newParticleShapePosition.x = gasAreaCollider.center.x;
        newParticleShapePosition.z = gasAreaCollider.center.z;

        // BoxCollider의 바닥 Y 위치 계산
        float colliderBottomY = gasAreaCollider.center.y - (gasAreaCollider.size.y / 2.0f);
        // 파티클 Shape의 높이의 절반 (새로운 Y 스케일 기준)
        float particleShapeHalfHeight = newParticleShapeScale.y / 2.0f;
        // 파티클 Shape의 중심 Y는 (콜라이더 바닥 Y) + (파티클 Shape 높이의 절반)
        newParticleShapePosition.y = colliderBottomY + particleShapeHalfHeight;
        
        shapeModule.position = newParticleShapePosition;
    }

#if UNITY_EDITOR
    // 에디터에서 BoxCollider의 값이 변경될 때마다 파티클 시스템을 업데이트합니다.
    void OnValidate()
    {
        // Awake가 호출되기 전이거나, 컴포넌트가 아직 할당되지 않았을 수 있으므로 다시 찾아봅니다.
        if (gasAreaCollider == null)
        {
            gasAreaCollider = GetComponent<BoxCollider>();
        }
        if (gasParticleSystem == null)
        {
            // GetComponentInChildren은 OnValidate에서 자주 호출되면 성능에 영향을 줄 수 있으므로 주의합니다.
            // 하지만 이 경우, Inspector에서 Collider 변경 시 피드백을 위한 것이므로 괜찮을 수 있습니다.
            gasParticleSystem = GetComponentInChildren<ParticleSystem>();
        }

        if (gasParticleSystem != null && gasAreaCollider != null)
        {
            // OnValidate는 에디터에서만 호출되므로, Application.isPlaying 체크는 필수는 아니지만,
            // 명확성을 위해 추가할 수 있습니다.
            if (!Application.isPlaying) 
            {
                // OnValidate에서 직접 파티클 시스템 프로퍼티를 변경하면 경고가 발생할 수 있으므로,
                // EditorApplication.delayCall을 사용하여 다음 업데이트 주기에 실행하도록 합니다.
                UnityEditor.EditorApplication.delayCall += AdjustParticleSystemToCollider;
            }
        }
    }
#endif
}
