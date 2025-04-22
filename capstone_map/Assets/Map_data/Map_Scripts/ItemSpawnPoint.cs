using UnityEngine;

public class ItemSpawnPoint : MonoBehaviour
{
    public GameObject[] itemList;
    Transform tf;
    public bool Item { get; set; }

    void Start()
    {
        tf = GetComponent<Transform>();
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
                item.transform.position = tf.position;
            }
        }
    }
}
