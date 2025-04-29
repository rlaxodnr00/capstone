using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerationManager : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;

    [Header("몬스터 설정")]
    public GameObject[] monsterPrefabs;  // 여러 종류의 몬스터 프리팹
    public int monsterCount = 10;        // 생성할 몬스터 수
    public float minDistanceFromPlayer = 10f; // 플레이어와의 최소 거리

    public float minDistanceBetweenMonsters = 3f; // 새로 추가!
    List<Vector3> spawnedPositions = new List<Vector3>(); // 생성된 몬스터들의 위치 기록

    [Header("참조")]
    public Transform playerTransform;    // 플레이어 위치

    IEnumerator Start()
    {
        // 맵 자동 생성 기다리기
        yield return new WaitForSeconds(1.0f);

        // 지붕 제외
        ExcludeCeilings();

        // NavMesh 생성
        navMeshSurface.BuildNavMesh();

        // NavMesh 생성 완료 후 몬스터 배치
        yield return new WaitForEndOfFrame(); // navmesh bake 대기
        SpawnMonstersOnNavMesh();
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

        List<Vector3> spawnedPositions = new List<Vector3>();

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
    }


    Vector3 GetRandomPointOnNavMesh()
    {
        // 랜덤 범위 조절 (맵이 너무 넓을 경우 수정 가능)
        Vector3 randomDirection = Random.insideUnitSphere * 50f;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 20f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position; // fallback
    }
}
