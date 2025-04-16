using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine.AI;
using UnityEngine;

// �� �������� �����ϴ� ��ũ��Ʈ (�� ������Ʈ�� �����ϸ��)
public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] monsterPrefabs; // ���� ������
    [SerializeField] private int monsterCount = 10; // ���� ��
    [SerializeField] private float monsterSpacing = 5f; // ���ͳ��� ����
    [SerializeField] private float playerSafeRadius = 10f; // �÷��̾� ���� �Ÿ�
    [SerializeField] private Transform playerTransform; // �÷��̾� ��ġ ����
    [SerializeField] private Vector2 mapSize = new Vector2(50, 50); // �� ����

    [SerializeField] private NavMeshSurface navMeshSurface;

    private List<Vector3> placedPositions = new List<Vector3>();

    void Awake()
    {
        // ���� ������ navMesh�� bake�ϰ� ����
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

            // NavMesh ������ Ȯ��
            if (!NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 2f, NavMesh.AllAreas)) continue;

            Vector3 finalPos = hit.position;

            // �÷��̾�� �Ÿ� üũ
            if (Vector3.Distance(playerTransform.position, finalPos) < playerSafeRadius) continue;

            // �ٸ� ���͵�� ���� üũ
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

            // ���� ������ ���� ����
            GameObject prefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];

            Instantiate(prefab, finalPos, Quaternion.identity);
            placedPositions.Add(finalPos);
            spawned++;
        }
    }
}
