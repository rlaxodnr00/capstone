using UnityEngine;

public class ItemSpawnPoint : MonoBehaviour
{
    public GameObject[] itemList;
    Transform tf; // Transform 캐시
    public bool Item { get; set; } // 현재 이 스폰 포인트에 아이템이 있는지 여부

    void Awake() // Start 대신 Awake 사용 권장
    {
        tf = GetComponent<Transform>();
        // Item 프로퍼티는 기본적으로 false로 초기화됩니다.
    }

    // Update 메서드는 현재 사용하지 않으므로 비워둡니다.
    // void Update()
    // {
    // }

    public void ItemCreate()
    {
        if (Item) // 이미 이 스폰 포인트에 아이템이 있다면 생성하지 않음
        {
            return;
        }

        if (itemList == null || itemList.Length == 0)
        {
            // Debug.LogWarning($"ItemSpawnPoint ({gameObject.name}): itemList is empty or not configured.", this);
            return;
        }

        int index = Random.Range(0, itemList.Length);
        GameObject prefabToSpawn = itemList[index];

        if (prefabToSpawn == null)
        {
            // Debug.LogWarning($"ItemSpawnPoint ({gameObject.name}): Selected prefab at index {index} is null.", this);
            return;
        }

        bool canSpawnThisItem = true;
        if (ItemManager.Instance != null)
        {
            canSpawnThisItem = ItemManager.Instance.RequestSpawnForItem(prefabToSpawn);
        }
        else
        {
            // ItemManager가 없는 경우 (예: 테스트 씬), 제한 없이 생성하거나 경고를 표시할 수 있습니다.
            Debug.LogWarning("ItemManager.Instance is not available. Spawning item without checking limits.", this);
        }

        if (canSpawnThisItem)
        {
            // 아이템 생성 성공
            Item = true; // 스폰 포인트가 점유되었음을 표시

            // 아이템을 스폰 포인트의 위치와 회전값으로 생성하고, 스폰 포인트의 자식으로 설정
            //GameObject spawnedItem = Instantiate(prefabToSpawn, tf.position, tf.rotation, tf);
            Quaternion prefabRotation = prefabToSpawn.transform.rotation;
            GameObject spawnedItem = Instantiate(prefabToSpawn, tf.position, prefabRotation, tf);
            // 자식으로 설정했으므로, 로컬 위치/회전을 (0,0,0)으로 맞추고 싶다면 아래처럼 할 수 있습니다.
            // spawnedItem.transform.localPosition = Vector3.zero;
            // spawnedItem.transform.localRotation = Quaternion.identity;

            // Debug.Log($"ItemSpawnPoint ({gameObject.name}): Successfully spawned '{prefabToSpawn.name}'.");
        }
        else
        {
            // 생성 제한으로 인해 아이템 생성 실패.
            // Item 프로퍼티는 false로 유지되어, 이 스폰 포인트는 여전히 비어있는 것으로 간주됩니다.
            // Debug.Log($"ItemSpawnPoint ({gameObject.name}): Failed to spawn '{prefabToSpawn.name}' due to spawn limits. Spawn point remains available.");
        }
    }
}
