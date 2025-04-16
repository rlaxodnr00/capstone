using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine.AI;
using UnityEngine;

// 몹 랜덤으로 생성하는 스크립트 (빈 오브젝트에 적용하면됨)
public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] monsterPrefabs; // 몬스터 종류들
    [SerializeField] private int monsterCount = 10; // 몬스터 수
    [SerializeField] private float monsterSpacing = 5f; // 몬스터끼리 간격
    [SerializeField] private float playerSafeRadius = 10f; // 플레이어 안전 거리
    [SerializeField] private Transform playerTransform; // 플레이어 위치 참조
    [SerializeField] private Vector2 mapSize = new Vector2(50, 50); // 맵 범위

    [SerializeField] private NavMeshSurface navMeshSurface;

    private List<Vector3> placedPositions = new List<Vector3>();

    void Awake()
    {
        // 게임 시작후 navMesh를 bake하게 설정
        navMeshSurface.BuildNavMesh(); 
    }

    void Start()
    {
        SpawnMonsters();
    }

    void SpawnMonsters()
    {
        int spawned = 0;
        int tryCount = 0;

        while (spawned < monsterCount && tryCount < monsterCount * 20)
        {
            tryCount++;

            Vector3 randomPos = new Vector3(
                Random.Range(-mapSize.x / 2f, mapSize.x / 2f),
                0,
                Random.Range(-mapSize.y / 2f, mapSize.y / 2f)
            );

            // NavMesh 위인지 확인
            if (!NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 2f, NavMesh.AllAreas)) continue;

            Vector3 finalPos = hit.position;

            // 플레이어와 거리 체크
            if (Vector3.Distance(playerTransform.position, finalPos) < playerSafeRadius) continue;

            // 다른 몬스터들과 간격 체크
            bool tooClose = false;
            foreach (var pos in placedPositions)
            {
                if (Vector3.Distance(pos, finalPos) < monsterSpacing)
                {
                    tooClose = true;
                    break;
                }
            }
            if (tooClose) continue;

            // 몬스터 프리팹 랜덤 선택
            GameObject prefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];

            Instantiate(prefab, finalPos, Quaternion.identity);
            placedPositions.Add(finalPos);
            spawned++;
        }
    }
}
