using Unity.VisualScripting;
using UnityEngine;

public class ObjectSpawnPoint : MonoBehaviour
{
    public GameObject FlashlightColor;

    private void Awake() //Start���� ���� ����, ��ũ��Ʈ ��Ȱ��ȭ���� �����
    {
        FlashMake();
    }

    // Update is called once per frame
    void FlashMake()
    {
        Instantiate(FlashlightColor, transform.position, transform.rotation);
    }
        }
