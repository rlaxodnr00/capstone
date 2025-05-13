using UnityEngine;
using AiSoundDetect;

public class PlayerFootsteps : MonoBehaviour
{
    [Tooltip("�߼Ҹ� Sound_Emitter ������Ʈ")]
    public Sound_Emitter soundEmitter;

    [Tooltip("�߼Ҹ� ����� Ŭ�� ���")]
    public AudioClip[] footstepSounds;

    [Tooltip("�߼Ҹ� ��� ���� (��)")]
    public float stepInterval = 0.5f;

    private float timer = 0f;
    private CharacterController characterController; // �ʿ信 ���� �ٸ� �̵� ������Ʈ ��� ����
    private bool isMoving = false;

    void Start()
    {
        // Sound_Emitter ������Ʈ�� �Ҵ���� �ʾ��� ��� ���� ������Ʈ���� ã��
        if (soundEmitter == null)
        {
            soundEmitter = GetComponent<Sound_Emitter>();
            if (soundEmitter == null)
            {
                Debug.LogError("Sound_Emitter ������Ʈ�� �ʿ��մϴ�.");
                enabled = false; // ������Ʈ ��Ȱ��ȭ
            }
        }

        // CharacterController ������Ʈ ã�� (�̵� ������ ����)
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogWarning("CharacterController ������Ʈ�� ���� �̵� ������ �� �� �����ϴ�. �׻� �߼Ҹ��� ����� �� �ֽ��ϴ�.");
        }
    }

    void Update()
    {
        // �̵� ������ Ȯ�� (CharacterController�� �ִ� ���)
        if (characterController != null && characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        // �����̴� ���̰�, ��� ������ �Ǹ� �߼Ҹ� ���
        if (isMoving)
        {
            timer += Time.deltaTime;
            if (timer >= stepInterval)
            {
                PlayFootstepSound();
                timer = 0f;
            }
        }
    }

    void PlayFootstepSound()
    {
        if (footstepSounds.Length > 0 && soundEmitter != null)
        {
            // �������� �߼Ҹ� ����
            int randomIndex = Random.Range(0, footstepSounds.Length);
            soundEmitter.m_AudioClip = footstepSounds[randomIndex];
            soundEmitter.ClipPlay(); // Sound_Emitter�� ClipPlay �Լ� ȣ���Ͽ� ���
        }
    }
}