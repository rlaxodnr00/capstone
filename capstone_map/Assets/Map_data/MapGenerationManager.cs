using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerationManager : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;

    [Header("���� ����")]
    public GameObject[] monsterPrefabs;  // ���� ������ ���� ������
    public int monsterCount = 10;        // ������ ���� ��
    public float minDistanceFromPlayer = 10f; // �÷��̾���� �ּ� �Ÿ�

    public float minDistanceBetweenMonsters = 3f; // ���� �߰�!
    List<Vector3> spawnedPositions = new List<Vector3>(); // ������ ���͵��� ��ġ ���

    [Header("����")]
    public Transform playerTransform;    // �÷��̾� ��ġ

    IEnumerator Start()
    {
        // �� �ڵ� ���� ��ٸ���
        yield return new WaitForSeconds(1.0f);

        // ���� ����
        ExcludeCeilings();

        // NavMesh ����
        navMeshSurface.BuildNavMesh();

        // NavMesh ���� �Ϸ� �� ���� ��ġ
        yield return new WaitForEndOfFrame(); // navmesh bake ���
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

            // 1. �÷��̾���� �Ÿ� Ȯ��
            if (Vector3.Distance(spawnPos, playerTransform.position) < minDistanceFromPlayer)
                continue;

            // 2. ���� ���͵���� �Ÿ� Ȯ��
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

            // 3. ���� ������ ���� ���� �� ����
            GameObject prefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
            Instantiate(prefab, spawnPos, Quaternion.identity);

            spawnedPositions.Add(spawnPos);
            spawned++;
        }
    }


    Vector3 GetRandomPointOnNavMesh()
    {
        // ���� ���� ���� (���� �ʹ� ���� ��� ���� ����)
        Vector3 randomDirection = Random.insideUnitSphere * 50f;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 20f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position; // fallback
    }
}
