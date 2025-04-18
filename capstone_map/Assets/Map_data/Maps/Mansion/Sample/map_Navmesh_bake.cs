using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshUpdater : MonoBehaviour
{
    public NavMeshSurface surface;

    void Start()
    {
        // 1초 후에 NavMesh를 빌드하도록 딜레이 추가
        Invoke("BuildNavMesh", 1f);
    }

    void BuildNavMesh()
    {
        surface.BuildNavMesh();
    }
}