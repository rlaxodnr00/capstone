using System.Collections;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    GameObject map;
    ItemSpawnPoint[] itemSpawn = new ItemSpawnPoint[0]; // NullReferenceException 방지를 위해 빈 배열로 초기화

    public float itemDelay = 10f;
    float timer = 0f;
    [Tooltip("맵 및 스폰 포인트 초기화 대기 시간 (초)")]
    public float initializationDelay = 1.5f; // 맵 생성 및 스폰 포인트 준비 대기 시간

    IEnumerator Start()
    {
        // 맵과 스폰 포인트가 생성될 시간을 기다립니다.
        yield return new WaitForSeconds(initializationDelay);

        if (map == null)
        {
            map = GameObject.Find("Generated Map");
        }

        if (map != null)
        {
            // 비활성화된 오브젝트의 ItemSpawnPoint도 포함하여 검색합니다.
            itemSpawn = map.GetComponentsInChildren<ItemSpawnPoint>(true);
            Debug.Log($"ItemManager: '{map.name}'에서 {itemSpawn.Length}개의 아이템 스폰 포인트를 찾았습니다.");
            if (itemSpawn.Length == 0)
            {
                Debug.LogWarning("ItemManager: 'Generated Map' 하위에 ItemSpawnPoint 컴포넌트가 없습니다.");
            }
        }
        else
        {
            Debug.LogError("ItemManager: 'Generated Map'을(를) 찾을 수 없습니다. 아이템 스폰이 작동하지 않을 수 있습니다.");
        }
    }

    void Update()
    {
        // 아이템 생성 테스트 코드 (Q를 눌러 생성)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateItem();
        }

        // 일정 시간 마다 아이템을 생성함
        
        timer += Time.deltaTime;
        if (timer >= itemDelay)
        {
            CreateItem();
            timer = 0f;
        }
        
    }

    void CreateItem()
    {
        // 아이템 생성 위치가 없을 경우 생성하지 않음
        if (itemSpawn.Length == 0)
        {
            return;
        }
        // 맵에 있는 "모든" 아이템 생성 지점 중 하나를 지정함
        int index = Random.Range(0, itemSpawn.Length);
        
        // 아이템 생성 지점의 함수로 아이템 생성
        if (itemSpawn[index] != null)
        {
            itemSpawn[index].ItemCreate();
        }
        
    }
}
