using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // for .OrderBy

public class MemoManager : MonoBehaviour
{
    public GameObject prefabToSpawn; // �߰��� ������
    public int requiredCount = 3;    // ��ٸ� �ּ� ������Ʈ ��
    public int selectCount = 3;      // �������� ���� ��
    public float checkInterval = 0.5f; // üũ �ֱ� (��)
    GameObject map;

    void Start()
    {
        if (map == null)
        {
            map = GameObject.Find("Generated Map");
        }
        StartCoroutine(WaitAndSpawn());
    }

    IEnumerator WaitAndSpawn()
    {
        while (true)
        {
            var targets = map.GetComponentsInChildren<MemoSpawnPoint>();
            if (targets.Length >= requiredCount)
                break;

            yield return new WaitForSeconds(checkInterval);
        }

        var allTargets = map.GetComponentsInChildren<MemoSpawnPoint>().ToList();

        var selectedTargets = allTargets
            .OrderBy(x => Random.value)
            .Take(selectCount)
            .ToArray();

        for (int i = 0; i < selectedTargets.Length; i++)
        {
            var target = selectedTargets[i];

            GameObject newObj = Instantiate(prefabToSpawn, target.transform);
            newObj.transform.localPosition = Vector3.zero;

            var memoCom = newObj.GetComponentInChildren<Memo>();
            if (memoCom != null)
            {
                memoCom.memoNumber = i;
            }
            else
            {
                Debug.LogWarning("newObj(Ȥ�� �� �ڽĵ�)�� Memo ������Ʈ�� �����ϴ�!");
            }

        }
    }

}
