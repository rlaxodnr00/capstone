using UnityEngine;
using System.Collections;
using System.Linq;

public class MemoBundleSpawn : MonoBehaviour
{
    public GameObject bundle;
    MemoBundlePoint[] memoSpawn;
    public GameObject prefabToSpawn; // 생성할 메모 프리팹

    public int minMemoCount = 3;
    public int maxMemoCount = 7;
    int memoCount;

    IEnumerator Start()
    {
        if (bundle == null)
        {
            Debug.Log("번들 컨테이너 없음");
            yield break;
        }

        yield return new WaitForSeconds(1.0f);

        memoSpawn = bundle.GetComponentsInChildren<MemoBundlePoint>();
        Debug.Log("메모 스폰 갯수 : " + memoSpawn.Length);

        memoCount = Random.Range(minMemoCount, maxMemoCount);

        if (memoSpawn.Length >= memoCount)
        {
            
        }
        else
        {
            memoCount = memoSpawn.Length;
        }

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
}
