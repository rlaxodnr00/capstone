using System.Collections;
using System.Collections.Generic; // Dictionary 사용을 위해 추가
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static ItemManager Instance { get; private set; }

    [Header("Map Settings")]
    [Tooltip("아이템 스폰 포인트들을 포함하는 부모 GameObject (예: Generated Map). 비워두면 이름으로 찾습니다.")]
    public GameObject mapRootObject; // 인스펙터에서 할당 가능
    private const string DEFAULT_MAP_NAME = "Generated Map";
    ItemSpawnPoint[] itemSpawn = new ItemSpawnPoint[0]; // NullReferenceException 방지를 위해 빈 배열로 초기화

    [Header("Spawning Settings")]
    public float itemDelay = 10f;
    private float timer = 0f;
    [Tooltip("맵 및 스폰 포인트 초기화 대기 시간 (초)")]
    public float initializationDelay = 1.5f; // 맵 생성 및 스폰 포인트 준비 대기 시간

    [Header("Limited Item Spawn Settings")]
    [Tooltip("생성 횟수 제한을 둘 아이템 목록과 최대 생성 횟수")]
    public List<LimitedItemConfig> limitedItemConfigs = new List<LimitedItemConfig>();
    private Dictionary<GameObject, LimitedItemConfig> limitedItemLookup = new Dictionary<GameObject, LimitedItemConfig>();

    private bool isInitialized = false;

    [System.Serializable]
    public class LimitedItemConfig
    {
        public GameObject itemPrefab;
        public int maxTotalSpawns;
        [HideInInspector] public int currentSpawnCount = 0; // 현재까지 생성된 횟수
    }

    void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            // 씬 전환 시 파괴되지 않게 하려면 아래 주석 해제
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeLimitedItems();
    }

    void InitializeLimitedItems()
    {
        limitedItemLookup.Clear();
        foreach (var config in limitedItemConfigs)
        {
            if (config.itemPrefab != null && !limitedItemLookup.ContainsKey(config.itemPrefab))
            {
                config.currentSpawnCount = 0; // 매번 초기화 시 카운트 리셋
                limitedItemLookup.Add(config.itemPrefab, config);
            }
            else if (config.itemPrefab != null)
            {
                Debug.LogWarning($"ItemManager: Duplicate limited item prefab '{config.itemPrefab.name}' in configuration. Only the first one will be used.", this);
            }
            else
            {
                Debug.LogWarning("ItemManager: A null itemPrefab found in limitedItemConfigs.", this);
            }
        }
    }

    IEnumerator Start()
    {
        // 맵과 스폰 포인트가 생성될 시간을 기다립니다.
        yield return new WaitForSeconds(initializationDelay);

        if (mapRootObject == null)
        {
            mapRootObject = GameObject.Find(DEFAULT_MAP_NAME);
        }

        if (mapRootObject != null)
        {
            // 비활성화된 오브젝트의 ItemSpawnPoint도 포함하여 검색합니다.
            itemSpawn = mapRootObject.GetComponentsInChildren<ItemSpawnPoint>(true);
            Debug.Log($"ItemManager: Found {itemSpawn.Length} item spawn points in '{mapRootObject.name}'.");
            if (itemSpawn.Length == 0)
            {
                Debug.LogWarning($"ItemManager: No ItemSpawnPoint components found under '{mapRootObject.name}'.", this);
            }
        }
        else
        {
            Debug.LogError("ItemManager: Could not find the map root object. Item spawning might not work.", this);
        }
        isInitialized = true;
    }

    void Update()
    {
        if (!isInitialized || itemSpawn.Length == 0)
        {
            return; // 초기화 전이거나 스폰 포인트가 없으면 실행 안 함
        }

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
        if (!isInitialized || itemSpawn.Length == 0)
        {
            // 이중 체크, Update 시작부에서 이미 확인하지만 안전을 위해
            return;
        }

        // 활성화된 스폰 포인트 중에서 무작위로 하나 선택 시도
        List<ItemSpawnPoint> availableSpawnPoints = new List<ItemSpawnPoint>();
        foreach(var sp in itemSpawn)
        {
            if(sp != null && sp.gameObject.activeInHierarchy && !sp.Item) // 아이템이 없는 스폰 포인트만
            {
                availableSpawnPoints.Add(sp);
            }
        }

        if (availableSpawnPoints.Count == 0)
        {
            // Debug.Log("ItemManager: No available spawn points to create an item at this moment.");
            return; // 아이템을 생성할 수 있는 스폰 포인트가 없음
        }
        
        int index = Random.Range(0, availableSpawnPoints.Count);
        ItemSpawnPoint selectedSpawnPoint = availableSpawnPoints[index];
        
        selectedSpawnPoint.ItemCreate();
    }

    /// <summary>
    /// 특정 아이템 프리팹의 생성을 요청하고, 가능하면 카운트를 증가시킵니다.
    /// </summary>
    /// <param name="itemPrefab">생성하려는 아이템 프리팹</param>
    /// <returns>생성 가능하면 true, 아니면 false</returns>
    public bool RequestSpawnForItem(GameObject itemPrefab)
    {
        if (itemPrefab == null) return false;

        if (limitedItemLookup.TryGetValue(itemPrefab, out LimitedItemConfig config))
        {
            if (config.currentSpawnCount < config.maxTotalSpawns)
            {
                config.currentSpawnCount++;
                // Debug.Log($"ItemManager: Spawning limited item '{itemPrefab.name}'. Count: {config.currentSpawnCount}/{config.maxTotalSpawns}");
                return true; // 생성 허용 및 카운트 증가
            }
            else
            {
                // Debug.Log($"ItemManager: Max spawn count reached for limited item '{itemPrefab.name}'. Cannot spawn.");
                return false; // 생성 불가 (최대치 도달)
            }
        }
        return true; // 제한 목록에 없는 아이템은 항상 생성 가능
    }
}
