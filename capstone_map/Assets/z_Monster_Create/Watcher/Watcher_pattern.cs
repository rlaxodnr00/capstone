using UnityEngine;
using UnityEngine.AI;  // NavMesh ����� ���� ���ӽ����̽�

public class Watcher_pattern : MonoBehaviour
{
   
    public float detectionRange = 4f;   // �÷��̾� ���� ����
    public float attackRange = 2f;  // ���� ���� ����
    public float moveSpeed = 1f;    // ������ �̵� �ӵ�
    public float fieldOfView = 60f; // �þ߰� (60��)
    public float maxHealth = 100f; // ������ �ִ� ü��
    public float attackDamage = 10f;   // ������ ���ݷ�


    private float currentHealth;  // ���� ü��
    private Animator animator;  // �ִϸ����� ������Ʈ
    private Transform player;   // �÷��̾��� ��ġ
    private NavMeshAgent agent; // ������̼� AI�� ���� NavMeshAgent

    private bool isDead = false;      // ���Ͱ� �׾����� ����
    private bool isAttacking = false; // ���Ͱ� ���� ���� ������ ����

    void Start()
    {
        // ������ �ִϸ����Ϳ� NavMeshAgent ������Ʈ�� ������
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // �±װ� "Player"�� ������Ʈ�� ã�Ƽ� �÷��̾�� ����
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // ���� ü���� �ִ� ü������ �ʱ�ȭ
        currentHealth = maxHealth;
    }

    void Update()
    {
        // ���Ͱ� �׾��ٸ� �ƹ� �ൿ�� ���� ����
        if (isDead) return;

        // �÷��̾�� ���� ������ �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // ���� ���� �ȿ� ���Դٸ� ���� ����
        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        // �÷��̾ ���� ���� ���� ������ �߰�
        else if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        // �÷��̾ �������� ������ ���� (Idle �� Walk)
        else
        {
            Patrol();
        }
    }

    // ���Ͱ� �÷��̾ ã�� ������ �� �����ϴ� �Լ�
    void Patrol()
    {
        agent.isStopped = false;  // �̵� Ȱ��ȭ
        agent.speed = moveSpeed;  // �⺻ �̵� �ӵ��� ����

        // ���Ͱ� �̵� ������ Ȯ���Ͽ� �ִϸ��̼� ���� ����
        if (agent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("isMoving", true); // Walk ���� Ȱ��ȭ
        }
        else
        {
            animator.SetBool("isMoving", false); // Idle ���� Ȱ��ȭ
        }

        // �÷��̾� ���� �� ���� ���� ����
        animator.SetBool("playerDetected", false);
        animator.SetBool("inAttackRange", false);
    }

    // �÷��̾ �����ϸ� Run ���·� ����
    void ChasePlayer()
    {
        agent.isStopped = false;             // �̵� Ȱ��ȭ
        agent.speed = moveSpeed * 1.5f;      // �߰� �� �̵� �ӵ� ����
        agent.SetDestination(player.position); // �÷��̾ ��ǥ �������� ����

        animator.SetBool("isMoving", true); // �̵� ���̹Ƿ� Walk �ִϸ��̼� Ȱ��ȭ
        animator.SetBool("playerDetected", true); // �÷��̾� ���� ���� Ȱ��ȭ
        animator.SetBool("inAttackRange", false); // ���� ���� ���� �ƴ�
    }

    // ���� ���� (Attack)
    void AttackPlayer()
    {
        if (isAttacking) return; // ���� ���� ���̶�� �������� ����

        isAttacking = true;   // ���� �� ���·� ����
        agent.isStopped = true; // ���� �߿��� �̵� ����

        animator.SetBool("inAttackRange", true); // ���� �ִϸ��̼� ����
        animator.SetTrigger("attack"); // ���� �ִϸ��̼� Ʈ���� ����

        // ���� �ð��� ������ �ٽ� ���� ����
        Invoke("ResetAttack", 1.5f);
    }

    // ���� ���� �ʱ�ȭ (���� �� ������ ����)
    void ResetAttack()
    {
        isAttacking = false; // �ٽ� ���� �����ϵ��� ����
    }

    // ���Ͱ� �ǰݴ����� �� ����Ǵ� �Լ�
    public void TakeDamage(int damage)
    {
        if (isDead) return; // ���Ͱ� �̹� �׾��ٸ� �������� ����

        currentHealth -= damage; // ü�� ����
        animator.SetTrigger("isHit"); // �ǰ� �ִϸ��̼� ����

        // ü���� 0 �����̸� ��� ó��
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ���� ��� ó��
    void Die()
    {
        isDead = true; // ��� ���·� ����
        agent.isStopped = true; // �̵� ����
        animator.SetBool("isDead", true); // ��� �ִϸ��̼� ����

        // ���� �ð� �� ������Ʈ ���� (���� ������ ��� �߰� ����)
        Destroy(gameObject, 3f);
    }
}
