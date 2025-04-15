using Unity.VisualScripting;
using UnityEngine;

public class ObjectSpawnPoint : MonoBehaviour
{
    public GameObject FlashlightColor;

    private void Awake() //Start보다 먼저 실행, 스크립트 비활성화에도 실행됨
    {
        FlashMake();
    }

    // Update is called once per frame
    void FlashMake()
    {
        Instantiate(FlashlightColor, transform.position, transform.rotation);
    }
        }
