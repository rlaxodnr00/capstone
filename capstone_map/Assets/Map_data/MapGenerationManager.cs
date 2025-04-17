using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

public class MapGenerationManager : MonoBehaviour   // �ʿ� navmesh�� �ڵ����� ����ִ� ��ũ��Ʈ
{
    public NavMeshSurface navMeshSurface;

    IEnumerator Start()
    {
        // �� �ڵ� ���� ���� ������ ��ٸ��� (����: 1��)
        yield return new WaitForSeconds(1.0f);

        // ���� �ڵ� ����
        ExcludeCeilings();

        // NavMesh �ٽ� Bake
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
}
