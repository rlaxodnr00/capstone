using UnityEngine;

public class Interactable : MonoBehaviour
{
    // 플레이어가 오브젝트를 바라볼 때 호출되는 함수
    public virtual void OnLookAt()
    {
        // Debug.Log(gameObject.name + "를 바라봄.");
    }

    // 플레이어가 오브젝트에서 시선을 돌릴 때 호출되는 함수
    public virtual void OnLookAway()
    {
        // Debug.Log(gameObject.name + "에서 시선을 돌림.");
    }

    // 플레이어가 상호작용할 때 호출되는 함수
    public virtual void OnInteract()
    {
        // Debug.Log(gameObject.name + "와 상호작용함.");
    }
}