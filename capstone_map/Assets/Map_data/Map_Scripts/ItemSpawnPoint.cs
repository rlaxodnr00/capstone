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
        // �� ������ �������� �ִ��� �˻�
        if (!Item)
        {
            // ���ٸ� ������ ��� �� �������� ������ ����
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
