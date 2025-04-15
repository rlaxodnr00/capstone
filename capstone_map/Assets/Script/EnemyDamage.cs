using UnityEngine;

//해당 적이 주는 데미지 값을 정의
// HPController에서 충돌 시 이 값을 읽어 플레이어 체력을 감소시키는 데 사용

public class EnemyDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    // 이 적이 플레이어에게 주는 데미지량.
    public float damage = 10f;
    
    // 필요에 따라, 데미지 계산에 영향을 줄 수 있는 추가 변수나 함수들을 여기에 추가할 수 있음.
}
