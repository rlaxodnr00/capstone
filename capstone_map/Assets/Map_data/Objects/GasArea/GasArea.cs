using UnityEngine;

public class GasArea : MonoBehaviour
{
    

    void OnTriggerEnter(Collider other)
    {
        // 플레이어 태그를 가진 오브젝트인지 확인
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<GasDamage>())
            {
                other.GetComponent<GasDamage>().addGas();
            }
            Debug.Log("플레이어가 가스 영역에 들어왔습니다.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // 플레이어 태그를 가진 오브젝트인지 확인
        if (other.CompareTag("Player"))
        {
            other.GetComponent<GasDamage>().removeGas();
            Debug.Log("플레이어가 가스 영역에서 나갔습니다.");
        }
    }
}
