using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CreateSafe : MonoBehaviour
{
    [Header("Safe Settings")]
    [Tooltip("생성할 금고 프리팹")]
    public GameObject safePrefab;

    [Header("Spawn Area Settings")]
    [Tooltip("SafePosition 컴포넌트들을 포함하는 부모 GameObject 이름")]
    public string mapObjectName = "Generated Map";
    [Tooltip("SafePosition들을 찾기 전 대기 시간 (맵 생성 시간 고려)")]
    public float initializationDelay = 1.5f;

    [Header("Target Proximity Settings")]
    [Tooltip("거리를 측정할 대상 오브젝트의 태그 (예: Player)")]
    public string targetTag = "Player";
    [Tooltip("이 거리보다 가까운 위치는 생성 우선순위가 매우 낮아집니다.")]
    public float minDistanceThreshold = 10.0f;
    [Tooltip("거리에 따른 가중치 증가율 (높을수록 먼 곳 선호도 급증)")]
    public float distancePower = 2.0f;

    private GameObject targetObject;
    private SafePosition[] safeSpawnPoints;

    private struct WeightedSafePosition
    {
        public SafePosition position;
        public float weight;

        public WeightedSafePosition(SafePosition pos, float w)
        {
            position = pos;
            weight = w;
        }
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(initializationDelay);

        if (safePrefab == null)
        {
            Debug.LogError("[CreateSafe] Safe Prefab이 할당되지 않았습니다!");
            yield break;
        }

        targetObject = GameObject.FindWithTag(targetTag);
        if (targetObject == null)
        {
            Debug.LogError($"[CreateSafe] '{targetTag}' 태그를 가진 대상 오브젝트를 찾을 수 없습니다!");
            yield break;
        }

        // yield return new WaitForSeconds(initializationDelay);

        GameObject mapRoot = GameObject.Find(mapObjectName);
        if (mapRoot == null)
        {
            Debug.LogError($"[CreateSafe] '{mapObjectName}' 이름을 가진 맵 오브젝트를 찾을 수 없습니다!");
            yield break;
        }

        safeSpawnPoints = mapRoot.GetComponentsInChildren<SafePosition>(true); // 비활성화된 오브젝트도 포함
        if (safeSpawnPoints == null || safeSpawnPoints.Length == 0)
        {
            Debug.LogWarning("[CreateSafe] SafePosition 컴포넌트를 찾을 수 없습니다. 금고가 생성되지 않습니다.");
            yield break;
        }

        SpawnSingleSafe();
    }

    void SpawnSingleSafe()
    {
        if (safeSpawnPoints.Length == 0) return;

        List<WeightedSafePosition> weightedPositions = new List<WeightedSafePosition>();
        float totalWeight = 0f;

        foreach (SafePosition sp in safeSpawnPoints)
        {
            if (sp == null || !sp.gameObject.activeInHierarchy) continue;

            float distance = Vector3.Distance(sp.transform.position, targetObject.transform.position);
            float weight;

            if (distance < minDistanceThreshold)
            {
                weight = 0.01f; // 매우 가까운 위치에는 아주 작은 가중치 부여
            }
            else
            {
                // 기준 거리 이상일 경우, (거리 - 기준거리)^distancePower 에 비례하는 가중치 + 기본 가중치 1
                weight = 1.0f + Mathf.Pow(distance - minDistanceThreshold, distancePower);
            }
            weight = Mathf.Max(0.001f, weight); // 가중치가 음수가 되지 않도록 최소값 보정

            weightedPositions.Add(new WeightedSafePosition(sp, weight));
            totalWeight += weight;
        }

        if (weightedPositions.Count == 0 || totalWeight <= 0.001f) // 유효한 스폰 포인트가 없거나 총 가중치가 매우 낮은 경우
        {
            Debug.LogWarning("[CreateSafe] 유효한 금고 생성 위치를 찾지 못했거나 모든 위치의 가중치가 너무 낮습니다. 임의의 위치에 생성 시도합니다.");
            SafePosition fallbackSpawnPoint = safeSpawnPoints[Random.Range(0, safeSpawnPoints.Length)];
            Instantiate(safePrefab, fallbackSpawnPoint.transform.position, fallbackSpawnPoint.transform.rotation, fallbackSpawnPoint.transform);
            Debug.Log($"[CreateSafe] 금고가 비상 생성 위치에 생성되었습니다: {fallbackSpawnPoint.name}");
            return;
        }

        float randomRoll = Random.Range(0f, totalWeight);
        SafePosition selectedSpawnPoint = null;

        foreach (WeightedSafePosition wsp in weightedPositions.OrderBy(x => Random.value)) // 가중치 적용 전 한번 더 섞어 동일 가중치 처리 개선
        {
            if (randomRoll < wsp.weight)
            {
                selectedSpawnPoint = wsp.position;
                break;
            }
            randomRoll -= wsp.weight;
        }

        if (selectedSpawnPoint == null && weightedPositions.Count > 0) // 만약 위 로직에서 선택되지 않았다면 (부동소수점 오류 등 극히 드문 경우)
        {
            selectedSpawnPoint = weightedPositions.OrderByDescending(w => w.weight).First().position; // 가장 가중치가 높은 곳에 생성
            Debug.LogWarning("[CreateSafe] 가중치 기반 선택 실패, 가장 높은 가중치 위치에 생성합니다.");
        }

        if (selectedSpawnPoint != null)
        {
            Instantiate(safePrefab, selectedSpawnPoint.transform.position, selectedSpawnPoint.transform.rotation, selectedSpawnPoint.transform);
            float finalDistance = Vector3.Distance(selectedSpawnPoint.transform.position, targetObject.transform.position);
            Debug.Log($"[CreateSafe] 금고 생성 완료: {selectedSpawnPoint.name} (대상으로부터 거리: {finalDistance:F2})");
        }
        else
        {
            Debug.LogError("[CreateSafe] 모든 시도에도 불구하고 금고를 생성할 위치를 선택하지 못했습니다.");
        }
    }
}
