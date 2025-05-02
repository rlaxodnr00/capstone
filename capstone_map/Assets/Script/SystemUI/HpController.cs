using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HPController : MonoBehaviour
{

    [Header("Health Settings")]
    public float maxHealth = 100f;         // �ִ� ü��
    public float currentHealth = 100f;     // ���� ü��
    public float invincibleDuration = 1f;    // ���� ���� �ð� (��)
    public float exceptionDefaultDamage = 10f;      // �⺻ ������ (EnemyDamage ������Ʈ�� ���� ���)

    // ���� ���� �÷���
    private bool isInvincible = false;

    // ü�� ��ȭ �̺�Ʈ. ������(��, GameUIManager)���� (���� ü��, �ִ� ü��) ������ ����.
    public event Action<float, float> OnHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
        // ü�� �ʱⰪ ����
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        // GameUIManager�� �̹� �����Ѵٸ� ���� (HPController�� Start�� ������ GameUIManager�� Awake���� �ʱ�ȭ�Ǿ� �ִٰ� ����)
        if (GameUIManager.Instance != null)
        {
            OnHealthChanged += GameUIManager.Instance.UpdateHealthUI;
        }
    }

    //�������� �޾� ü���� ���ҽ�Ű��, ü�� ���� �̺�Ʈ�� �߻�
    public void TakeDamage(float damageAmount)
    {
        if (isInvincible) return;

        currentHealth = Mathf.Clamp(currentHealth - damageAmount, 0f, maxHealth);
        Debug.Log("HP ����: " + damageAmount + " | ���� ü��: " + currentHealth);

        // ü�� ��ȭ �̺�Ʈ �߻�
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // �÷��̾� ��� ó��
    private void Die()
    {
        Debug.Log("�÷��̾� ���!");
        GameUIManager.Instance.ShowDogImage();
    }

    // ���� ���¸� ���� �ð� �����ϴ� �ڷ�ƾ
    public IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        Debug.Log("���� ���� ����");
        GameUIManager.Instance?.StartHitEffect();
        Camera.main.GetComponent<CameraShake>()?.TriggerShake();
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
        Debug.Log("���� ���� ����");
    }

    // �÷��̾� �浹 ���� (CharacterController ��� ��)
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy") && !isInvincible)
        {
            // Enemy ������Ʈ���� ������ ������ �������� ����
            // ���� ���, EnemyDamage ������Ʈ�� ������ �� ���� ����ϰ�, ������ defaultDamage ���
            float damageAmount = exceptionDefaultDamage;
            var enemyDamage = hit.gameObject.GetComponent<EnemyDamage>();
            if (enemyDamage != null)
            {
                damageAmount = enemyDamage.damage;
            }
            Debug.Log("Enemy�� �浹, ������: " + damageAmount);
            TakeDamage(damageAmount);

            // ���� ���� ����
            StartCoroutine(InvincibilityCoroutine());
        }
    }
}

