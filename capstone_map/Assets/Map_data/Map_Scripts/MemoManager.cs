using UnityEngine;
using System.Collections;
using System.Linq;

public class MemoManager : MonoBehaviour
{
    GameObject map;
    MemoSpawnPoint[] memoSpawn;
    public GameObject prefabToSpawn; // ������ �޸� ������

    int memoCount = 3;

    IEnumerator Start()
    {
        if (map == null)
        {
            map = GameObject.Find("Generated Map");
        }
        yield return new WaitForSeconds(1.0f);

        memoSpawn = map.GetComponentsInChildren<MemoSpawnPoint>();
        Debug.Log("�޸� ���� ���� : " + memoSpawn.Length);

        if (memoSpawn.Length >= memoCount)
        {
            Debug.Log("�޸� ���� ���");

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
        else
        {
            Debug.Log("�޸� ���� ����");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
