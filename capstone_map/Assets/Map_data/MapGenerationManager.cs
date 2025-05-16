using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerationManager : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;

    [Header("���� ����")]
    public GameObject[] monsterPrefabs; // ���� ������ ���� ������
    public int monsterCount = 10;        // ������ ���� ��
    public float minDistanceFromPlayer = 10f; // �÷��̾���� �ּ� �Ÿ�
    public float minDistanceBetweenMonsters = 3f; // ���� �߰�!

    [Header("����")]
    public Transform playerTransform;     // �÷��̾� ��ġ

    private List<Vector3> spawnedPositions = new List<Vector3>(); // ������ ���͵��� ��ġ ���

    IEnumerator Start()
    {
        // �� �ڵ� ���� �ϷḦ ��ٸ��� ���� (����)
        // ���� �� ���� �Ϸ� ���ǿ� ���� �����ؾ� �մϴ�.
        yield return StartCoroutine(WaitForMapGeneration());

        // ���� ����
        ExcludeCeilings();

        // NavMesh ���� �� �Ϸ� ���
        BakeNavMesh();
        Debug.Log("NavMesh Bake �Ϸ�");

        // NavMesh ���� �Ϸ� �� ���� ��ġ
        SpawnMonstersOnNavMesh();
    }

    IEnumerator WaitForMapGeneration()
    {
        // ���⿡ �� ������ �Ϸ�Ǿ����� Ȯ���ϴ� ������ �����ϰų�,
        // �� ������ ����ϴ� ��ũ��Ʈ���� �̺�Ʈ�� �߻���Ű�� ��ٸ��� ������� ������ �� �ֽ��ϴ�.
        // �ӽ÷� 1�� ����մϴ�.
        yield return new WaitForSeconds(1.0f);
        Debug.Log("�� ���� �Ϸ� (����)");
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

        spawnedPositions.Clear(); // ����Ʈ �ʱ�ȭ

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

        if (spawned < monsterCount)
        {
            Debug.LogWarning($"�ִ� �õ� Ƚ��({maxAttempts})�� �ʰ��Ͽ� {spawned}/{monsterCount} ������ ���͸� �����߽��ϴ�.");
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

        return Vector3.zero; // NavMesh ���� ��ȿ�� ��ġ�� ã�� ������ ��� Vector3.zero ��ȯ
    }
}