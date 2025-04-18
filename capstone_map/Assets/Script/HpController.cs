using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HPController : MonoBehaviour
{

    [Header("Health Settings")]
    public float maxHealth = 100f;         // 최대 체력
    public float currentHealth = 100f;     // 현재 체력
    public float invincibleDuration = 1f;    // 무적 지속 시간 (초)
    public float exceptionDefaultDamage = 10f;      // 기본 데미지 (EnemyDamage 컴포넌트가 없을 경우)

    // 무적 상태 플래그
    private bool isInvincible = false;

    // 체력 변화 이벤트. 구독자(예, GameUIManager)에게 (현재 체력, 최대 체력) 정보를 전달.
    public event Action<float, float> OnHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
        // 체력 초기값 전달
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        // GameUIManager가 이미 존재한다면 구독 (HPController가 Start된 시점에 GameUIManager가 Awake에서 초기화되어 있다고 가정)
        if (GameUIManager.Instance != null)
        {
            OnHealthChanged += GameUIManager.Instance.UpdateHealthUI;
        }
    }

    //데미지를 받아 체력을 감소시키고, 체력 변경 이벤트를 발생
    public void TakeDamage(float damageAmount)
    {
        if (isInvincible) return;

        currentHealth = Mathf.Clamp(currentHealth - damageAmount, 0f, maxHealth);
        Debug.Log("HP 감소: " + damageAmount + " | 남은 체력: " + currentHealth);

        // 체력 변화 이벤트 발생
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // 플레이어 사망 처리
    private void Die()
    {
        Debug.Log("플레이어 사망!");
        GameUIManager.Instance.ShowDogImage();
    }

    // 무적 상태를 일정 시간 유지하는 코루틴
    public IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        Debug.Log("무적 상태 시작");
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
        Debug.Log("무적 상태 종료");
    }

    // 플레이어 충돌 판정 (CharacterController 사용 시)
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy") && !isInvincible)
        {
            // Enemy 오브젝트에서 데미지 정보를 가져오는 로직
            // 예를 들어, EnemyDamage 컴포넌트가 있으면 그 값을 사용하고, 없으면 defaultDamage 사용
            float damageAmount = exceptionDefaultDamage;
            var enemyDamage = hit.gameObject.GetComponent<EnemyDamage>();
            if (enemyDamage != null)
            {
                damageAmount = enemyDamage.damage;
            }
            Debug.Log("Enemy와 충돌, 데미지: " + damageAmount);
            TakeDamage(damageAmount);

            // 무적 상태 시작
            StartCoroutine(InvincibilityCoroutine());
        }
    }
}

