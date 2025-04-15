using UnityEngine;

public class ItemSpawnPoint : MonoBehaviour
{
    public GameObject[] itemList;
    public bool Item { get; set; }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void ItemCreate()
    {
        // 이 지점에 아이템이 있는지 검사
        if (!Item)
        {
            // 없다면 아이템 목록 중 랜덤으로 아이템 생성
            Item = true;
            int index = Random.Range(0, itemList.Length);
            if (itemList[index] != null)
            {
                GameObject item = Instantiate(itemList[index], transform);
                item.transform.position = transform.position;
            }
        }
    }
}
