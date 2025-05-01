using UnityEngine;
using System.Collections;
using System.Linq;

public class MemoManager : MonoBehaviour
{
    GameObject map;
    MemoSpawnPoint[] memoSpawn;
    public GameObject prefabToSpawn; // 생성할 메모 프리팹

    int memoCount = 3;

    IEnumerator Start()
    {
        if (map == null)
        {
            map = GameObject.Find("Generated Map");
        }
        yield return new WaitForSeconds(1.0f);

        memoSpawn = map.GetComponentsInChildren<MemoSpawnPoint>();
        Debug.Log("메모 스폰 갯수 : " + memoSpawn.Length);

        if (memoSpawn.Length >= memoCount)
        {
            Debug.Log("메모 스폰 충분");

            int memoNum = 1;

            // memoCount 만큼 랜덤으로 뽑아서 메모 생성
            var selectedTargets = memoSpawn
                .OrderBy(x => Random.value) // 랜덤으로 섞기
                .Take(memoCount)           // memoCount 만큼 선택
                .ToArray();

            if (prefabToSpawn != null)
            {
                foreach (var target in selectedTargets)
                {
                    // 선택된 위치에 메모 생성
                    GameObject newMemo = Instantiate(prefabToSpawn, target.transform);
                    
                    Debug.Log("메모 생성: " + newMemo.name + " at position " + target.transform.position);
                    newMemo.GetComponentInChildren<Memo>().memoNumber = memoNum++;
                }
            }
        }
        else
        {
            Debug.Log("메모 스폰 부족");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
