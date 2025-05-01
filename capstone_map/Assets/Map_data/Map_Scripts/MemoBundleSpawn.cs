using UnityEngine;
using System.Collections;
using System.Linq;

public class MemoBundleSpawn : MonoBehaviour
{
    public GameObject bundle;
    MemoBundlePoint[] memoSpawn;
    public GameObject prefabToSpawn; // ������ �޸� ������

    public int minMemoCount = 3;
    public int maxMemoCount = 7;
    int memoCount;

    IEnumerator Start()
    {
        if (bundle == null)
        {
            Debug.Log("���� �����̳� ����");
            yield break;
        }

        yield return new WaitForSeconds(1.0f);

        memoSpawn = bundle.GetComponentsInChildren<MemoBundlePoint>();
        Debug.Log("�޸� ���� ���� : " + memoSpawn.Length);

        memoCount = Random.Range(minMemoCount, maxMemoCount);

        if (memoSpawn.Length >= memoCount)
        {
            
        }
        else
        {
            memoCount = memoSpawn.Length;
        }

        int memoNum = 1;

        // memoCount ��ŭ �������� �̾Ƽ� �޸� ����
        var selectedTargets = memoSpawn
            .OrderBy(x => Random.value) // �������� ����
            .Take(memoCount)           // memoCount ��ŭ ����
            .ToArray();

        if (prefabToSpawn != null)
        {
            foreach (var target in selectedTargets)
            {
                // ���õ� ��ġ�� �޸� ����
                GameObject newMemo = Instantiate(prefabToSpawn, target.transform);

                Debug.Log("�޸� ����: " + newMemo.name + " at position " + target.transform.position);
                newMemo.GetComponentInChildren<Memo>().memoNumber = memoNum++;
            }
        }
    }
}
