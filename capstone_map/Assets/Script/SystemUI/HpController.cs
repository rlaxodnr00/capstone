using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HPController : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;         // 최대 체력
    public float minHealth = 0f;                //최저 체력
    public float currentHealth = 100f;     // 현재 체력
    public float invincibleDuration = 1f;    // 무적 지속 시간 (초)
    public float exceptionDefaultDamage = 10f;      // 기본 데미지 (EnemyDamage 컴포넌트가 없을 경우)

    // 무적 상태 플래그
    private bool isInvincible = false;

    // 체력 변화 이벤트. 구독자(예, GameUIManager)에게 (현재 체력, 최대 체력) 정보를 전달.
    public event Action<float, float> OnHealthChanged;

    public void DIE() => Die(); //사망 외부 호출

    public void CLEAR() => Clear(); //클리어 외부 호출
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
    public void TakeHeal(float healAmount)
    {
        if (isInvincible) return;

        currentHealth = Mathf.Clamp(currentHealth + healAmount, minHealth, maxHealth);
        Debug.Log("HP 회복: " + healAmount + " | 남은 체력: " + currentHealth);

        // 체력 변화 이벤트 발생
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }



    //데미지를 받아 체력을 감소시키고, 체력 변경 이벤트를 발생
    public void TakeDamage(float damageAmount)
    {
        if (isInvincible) return;

        currentHealth = Mathf.Clamp(currentHealth - damageAmount, minHealth, maxHealth);
        Debug.Log("HP 감소: " + damageAmount + " | 남은 체력: " + currentHealth);

        // 체력 변화 이벤트 발생
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
        else
        {
            // 무적 상태 시작
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    // 플레이어 사망 처리
    private void Die()
    {
        if (isInvincible) return; // 이미 사망 중이면 리턴

        isInvincible = true; // 사망 상태에서는 무적 유지
        Debug.LogWarning("플레이어 사망");
        GameUIManager.Instance.ShowDieImage();

        StartCoroutine(HandleDeathSequence());
    }

    private void Clear()
    {
        if (isInvincible) return;

        isInvincible = true;
        StartCoroutine(HandleClearSequence());
    }

    // 피격 시 무적 상태를 일정 시간 유지하는 코루틴
    public IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        Debug.Log("무적 상태 시작");
        GameUIManager.Instance?.StartHitEffect();
        Camera.main.GetComponent<CameraShake>()?.TriggerShake(1f);
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
        Debug.Log("무적 상태 종료");
    }

    // 사망 연출 코루틴
    // 플레이어가 엎어지며 화면 서서히 암전 << 가능하면
    // 일정 시간 후 재시작
    private IEnumerator HandleDeathSequence()
    {
        var animator = GetComponent<Animator>();
        GetComponent<UserMove>().enabled = false;

        if (animator != null)
            animator.SetTrigger("Die"); //사망 애니메이션 만든다면 사용

        isInvincible = true; // 무적 처리

        // 1. 화면 붉어짐 
        GameUIManager.Instance.StartGameEndingEffect(2.3f, GameUIManager.RGB(80, 13, 13));

        // 2. 카메라 흔들림 시작
        Camera.main.GetComponent<CameraShake>()?.TriggerShake(4f);

        // 3. 2초 대기 후 암전 시작
        yield return new WaitForSeconds(2f);
        ScreenTransition.Instance.StartFadeOut("We_Make_This_Map");
    }



    // 게임 클리어 연출로 사용할 코드 일단 여기 작성
    private IEnumerator HandleClearSequence()
    {
        GetComponent<UserMove>().enabled = false;
        isInvincible = true;

        GameUIManager.Instance.StartGameEndingEffect(5f, Color.white);
        Camera.main.GetComponent<CameraShake>()?.TriggerShake(5f);
        yield return new WaitForSeconds(5f);
        Application.Quit();
    }








    // 플레이어 충돌 판정 (CharacterController 사용 시)
    //아마 이거 적용 안 되고 다른 곳에 기능 옮김
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy") && !isInvincible)
        {
            // Enemy 오브젝트에서 데미지 정보를 가져오는 로직
            // EnemyDamage 컴포넌트가 있으면 그 값을 사용하고, 없으면 defaultDamage 사용
            float damageAmount = exceptionDefaultDamage;
            var enemyDamage = hit.gameObject.GetComponent<EnemyDamage>();
            if (enemyDamage != null)
            {
                damageAmount = enemyDamage.damage;
            }
            Debug.Log("Enemy와 충돌, 데미지: " + damageAmount);
            TakeDamage(damageAmount);

            
        }
    }
}

