using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

public class MapGenerationManager : MonoBehaviour   // 맵에 navmesh를 자동으로 깔아주는 스크립트
{
    public NavMeshSurface navMeshSurface;

    IEnumerator Start()
    {
        // 맵 자동 생성 끝날 때까지 기다리기 (예시: 1초)
        yield return new WaitForSeconds(1.0f);

        // 지붕 자동 제외
        ExcludeCeilings();

        // NavMesh 다시 Bake
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
