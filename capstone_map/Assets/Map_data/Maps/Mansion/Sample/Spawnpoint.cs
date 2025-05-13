using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    GameObject player;
    CharacterController cc;

    private void Start()
    {
        // Player 태그를 가진 오브젝트를 찾기
        player = GameObject.Find("WomanWarrior");

        // 플레이어를 찾으면 플레이어를 스폰포인트 위치로 옮기기
        if (player != null) {
            // CharacterController를 비활성화 한 후 스폰으로 이동시킨 뒤 재활성화
            cc = player.GetComponent<CharacterController>();
            cc.enabled = false;
            player.transform.position = transform.position;
            cc.enabled = true;
        }  
    }
}
