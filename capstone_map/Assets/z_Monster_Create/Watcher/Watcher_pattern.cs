using UnityEngine;
using UnityEngine.AI;  // NavMesh 사용을 위한 네임스페이스

public class Watcher_pattern : MonoBehaviour
{
   
    public float detectionRange = 4f;   // 플레이어 감지 범위
    public float attackRange = 2f;  // 공격 가능 범위
    public float moveSpeed = 1f;    // 몬스터의 이동 속도
    public float fieldOfView = 60f; // 시야각 (60도)
    public float maxHealth = 100f; // 몬스터의 최대 체력
    public float attackDamage = 10f;   // 몬스터의 공격력


    private float currentHealth;  // 현재 체력
    private Animator animator;  // 애니메이터 컴포넌트
    private Transform player;   // 플레이어의 위치
    private NavMeshAgent agent; // 내비게이션 AI를 위한 NavMeshAgent

    private bool isDead = false;      // 몬스터가 죽었는지 여부
    private bool isAttacking = false; // 몬스터가 현재 공격 중인지 여부

    void Start()
    {
        // 몬스터의 애니메이터와 NavMeshAgent 컴포넌트를 가져옴
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // 태그가 "Player"인 오브젝트를 찾아서 플레이어로 설정
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // 현재 체력을 최대 체력으로 초기화
        currentHealth = maxHealth;
    }

    void Update()
    {
        // 몬스터가 죽었다면 아무 행동도 하지 않음
        if (isDead) return;

        // 플레이어와 몬스터 사이의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 공격 범위 안에 들어왔다면 공격 실행
        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        // 플레이어가 감지 범위 내에 있으면 추격
        else if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        // 플레이어가 감지되지 않으면 순찰 (Idle ↔ Walk)
        else
        {
            Patrol();
        }
    }

    // 몬스터가 플레이어를 찾지 못했을 때 순찰하는 함수
    void Patrol()
    {
        agent.isStopped = false;  // 이동 활성화
        agent.speed = moveSpeed;  // 기본 이동 속도로 설정

        // 몬스터가 이동 중인지 확인하여 애니메이션 상태 변경
        if (agent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("isMoving", true); // Walk 상태 활성화
        }
        else
        {
            animator.SetBool("isMoving", false); // Idle 상태 활성화
        }

        // 플레이어 감지 및 공격 상태 해제
        animator.SetBool("playerDetected", false);
        animator.SetBool("inAttackRange", false);
    }

    // 플레이어를 감지하면 Run 상태로 변경
    void ChasePlayer()
    {
        agent.isStopped = false;             // 이동 활성화
        agent.speed = moveSpeed * 1.5f;      // 추격 시 이동 속도 증가
        agent.SetDestination(player.position); // 플레이어를 목표 지점으로 설정

        animator.SetBool("isMoving", true); // 이동 중이므로 Walk 애니메이션 활성화
        animator.SetBool("playerDetected", true); // 플레이어 감지 상태 활성화
        animator.SetBool("inAttackRange", false); // 아직 공격 범위 아님
    }

    // 공격 상태 (Attack)
    void AttackPlayer()
    {
        if (isAttacking) return; // 현재 공격 중이라면 실행하지 않음

        isAttacking = true;   // 공격 중 상태로 설정
        agent.isStopped = true; // 공격 중에는 이동 정지

        animator.SetBool("inAttackRange", true); // 공격 애니메이션 실행
        animator.SetTrigger("attack"); // 공격 애니메이션 트리거 실행

        // 일정 시간이 지나야 다시 공격 가능
        Invoke("ResetAttack", 1.5f);
    }

    // 공격 상태 초기화 (공격 후 딜레이 적용)
    void ResetAttack()
    {
        isAttacking = false; // 다시 공격 가능하도록 설정
    }

    // 몬스터가 피격당했을 때 실행되는 함수
    public void TakeDamage(int damage)
    {
        if (isDead) return; // 몬스터가 이미 죽었다면 실행하지 않음

        currentHealth -= damage; // 체력 감소
        animator.SetTrigger("isHit"); // 피격 애니메이션 실행

        // 체력이 0 이하이면 사망 처리
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 몬스터 사망 처리
    void Die()
    {
        isDead = true; // 사망 상태로 설정
        agent.isStopped = true; // 이동 중지
        animator.SetBool("isDead", true); // 사망 애니메이션 실행

        // 일정 시간 후 오브젝트 제거 (추후 아이템 드롭 추가 가능)
        Destroy(gameObject, 3f);
    }
}
