using UnityEngine;

public class Spawnpoint : MonoBehaviour
{

    private void Awake()
    {
        // Player 태그를 가진 오브젝트를 찾기
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // 플레이어를 찾으면 플레이어를 스폰포인트 위치로 옮기기
        if (player != null) {
            player.transform.position = transform.position;
        }  
    }
}
