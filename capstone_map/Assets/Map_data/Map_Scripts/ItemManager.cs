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
        // ������ ���� �׽�Ʈ �ڵ� (Q�� ���� ����)
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

        // ���� �ð� ���� �������� ������
        
        timer += Time.deltaTime;
        if (timer >= itemDelay)
        {
            CreateItem();
            timer = 0f;
        }
        
    }

    void CreateItem()
    {
        // ������ ���� ��ġ�� ���� ��� �������� ����
        if (itemSpawn.Length == 0)
        {
            return;
        }
        // �ʿ� �ִ� "���" ������ ���� ���� �� �ϳ��� ������
        int index = Random.Range(0, itemSpawn.Length);
        
        // ������ ���� ������ �Լ��� ������ ����
        if (itemSpawn[index] != null)
        {
            itemSpawn[index].ItemCreate();
        }
        
    }
}
