using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerationManager : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;

    [Header("몬스터 설정")]
    public GameObject[] monsterPrefabs; // 여러 종류의 몬스터 프리팹
    public int monsterCount = 10;        // 생성할 몬스터 수
    public float minDistanceFromPlayer = 10f; // 플레이어와의 최소 거리
    public float minDistanceBetweenMonsters = 3f; // 새로 추가!

    [Header("참조")]
    public Transform playerTransform;     // 플레이어 위치

    private List<Vector3> spawnedPositions = new List<Vector3>(); // 생성된 몬스터들의 위치 기록

    IEnumerator Start()
    {
        // 맵 자동 생성 완료를 기다리는 로직 (가정)
        // 실제 맵 생성 완료 조건에 따라 수정해야 합니다.
        yield return StartCoroutine(WaitForMapGeneration());

        // 지붕 제외
        ExcludeCeilings();

        // NavMesh 생성 및 완료 대기
        BakeNavMesh();
        Debug.Log("NavMesh Bake 완료");

        // NavMesh 생성 완료 후 몬스터 배치
        SpawnMonstersOnNavMesh();
    }

    IEnumerator WaitForMapGeneration()
    {
        // 여기에 맵 생성이 완료되었는지 확인하는 로직을 구현하거나,
        // 맵 생성을 담당하는 스크립트에서 이벤트를 발생시키고 기다리는 방식으로 변경할 수 있습니다.
        // 임시로 1초 대기합니다.
        yield return new WaitForSeconds(1.0f);
        Debug.Log("맵 생성 완료 (가정)");
    }

    void BakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }

    void ExcludeCeilings()
    {
        GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.ToLower().Contains("ceiling"))
            {
                var modifier = obj.GetComponent<NavMeshModifier>();
                if (modifier == null)
                    modifier = obj.AddComponent<NavMeshModifier>();

                modifier.overrideArea = true;
                modifier.area = 1;
                modifier.ignoreFromBuild = true;
            }
        }
    }

    void SpawnMonstersOnNavMesh()
    {
        int spawned = 0;
        int attempts = 0;
        int maxAttempts = monsterCount * 10;

        spawnedPositions.Clear(); // 리스트 초기화

        while (spawned < monsterCount && attempts < maxAttempts)
        {
            attempts++;

            Vector3 spawnPos = GetRandomPointOnNavMesh();
            if (spawnPos == Vector3.zero) continue;

            // 1. 플레이어와의 거리 확인
            if (Vector3.Distance(spawnPos, playerTransform.position) < minDistanceFromPlayer)
                continue;

            // 2. 기존 몬스터들과의 거리 확인
            bool tooCloseToOthers = false;
            foreach (Vector3 pos in spawnedPositions)
            {
                if (Vector3.Distance(spawnPos, pos) < minDistanceBetweenMonsters)
                {
                    tooCloseToOthers = true;
                    break;
                }
            }

            if (tooCloseToOthers) continue;

            // 3. 몬스터 프리팹 랜덤 선택 및 생성
            GameObject prefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
            Instantiate(prefab, spawnPos, Quaternion.identity);

            spawnedPositions.Add(spawnPos);
            spawned++;
        }

        if (spawned < monsterCount)
        {
            Debug.LogWarning($"최대 시도 횟수({maxAttempts})를 초과하여 {spawned}/{monsterCount} 마리의 몬스터만 생성했습니다.");
        }
    }

    Vector3 GetRandomPointOnNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 50f;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 20f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return Vector3.zero; // NavMesh 위에 유효한 위치를 찾지 못했을 경우 Vector3.zero 반환
    }
}