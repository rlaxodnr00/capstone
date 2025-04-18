using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshUpdater : MonoBehaviour
{
    public NavMeshSurface surface;

    void Start()
    {
        // 1�� �Ŀ� NavMesh�� �����ϵ��� ������ �߰�
        Invoke("BuildNavMesh", 1f);
    }

    void BuildNavMesh()
    {
        surface.BuildNavMesh();
    }
}