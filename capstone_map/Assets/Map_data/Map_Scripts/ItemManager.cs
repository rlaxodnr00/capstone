using UnityEngine;

public class ItemManager : MonoBehaviour
{
    GameObject map;
    ItemSpawnPoint[] itemSpawn;

    bool test;
    public float itemDelay = 10f;
    float timer = 0f;

    void Start()
    {
        if (map == null)
        {
            map = GameObject.Find("Generated Map");
        }
        itemSpawn = map.GetComponentsInChildren<ItemSpawnPoint>();
    }

    void Update()
    {
        // 아이템 생성 테스트 코드
        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!test)
            {
                test = true;
                CreateItem();
            }
        } else
        {
            test = false;
        }
        */

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
        // Debug.Log("called CreateItem()");

        // 맵에 있는 "모든" 아이템 생성 지점 중 하나를 지정함
        int index = Random.Range(0, itemSpawn.Length);
        
        // 아이템 생성 지점의 함수로 아이템 생성
        itemSpawn[index].ItemCreate();
    }
}
