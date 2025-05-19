using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepHandler : MonoBehaviour
{
    private CharacterController controller; // ĳ���� ��Ʈ�ѷ�
    private UserMove userMove;             // UserMove ��ũ��Ʈ ����
    private CrouchHandler crouchHandler;   // �ɱ� ���� Ȯ���� ���� CrouchHandler ����

    [Header("Audio Sources")]
    public AudioSource[] footstepSources;  // �߼Ҹ� ����� AudioSource �迭 (�����Ϳ��� �Ҵ�)
    public AudioSource[] jumpSounds;       // ���� �Ҹ� AudioSource �迭

    [Header("Step Intervals")]
    public float normalStepInterval = 0.4f; // ����/�޸��� �̵� �߰��� ���� (��)
    public float crouchStepInterval = 0.6f; // �ɾƼ� �ȱ� �߰��� ���� (�߼Ҹ� X)

    private float footstepTimer = 0f;       // �߰��� Ÿ�̸�
    private bool isMoving = false;         // ���� �̵� ������ ����
    private bool hasJumped = false;        // ���� ���� �ߺ� ��� ����

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        userMove = GetComponent<UserMove>();
        crouchHandler = GetComponent<CrouchHandler>(); // CrouchHandler ������Ʈ ��������

        if (userMove == null)
        {
            Debug.LogError("UserMove ��ũ��Ʈ�� �ʿ��մϴ�.");
            enabled = false;
            return;
        }

        if (crouchHandler == null)
        {
            Debug.LogError("CrouchHandler ��ũ��Ʈ�� �ʿ��մϴ�.");
            enabled = false;
            return;
        }

        if (footstepSources == null || footstepSources.Length == 0)
        {
            Debug.LogError("Footstep Sources AudioSource �迭�� ��� �ֽ��ϴ�.");
            enabled = false;
        }
    }

    void Update()
    {
        CheckIsMoving();
        HandleFootsteps();
        HandleJumpSound();
    }

    void CheckIsMoving()
    {
        Vector3 horizontalMove = new Vector3(userMove.MoveDirection.x, 0, userMove.MoveDirection.z);
        isMoving = controller.isGrounded && horizontalMove.magnitude > 0.1f;
    }

    void HandleFootsteps()
    {
        if (!isMoving)
        {
            footstepTimer = 0f;
            return;
        }

        float currentStepInterval = normalStepInterval;

        if (crouchHandler.IsCrouching()) // �ɾ��ִ� ���¶��
        {
            currentStepInterval = crouchStepInterval; // �߼Ҹ� �� ���� �������� ����
        }
        else if (userMove.CurrentSpeed >= userMove.sprintSpeed - 0.05f) // �޸��� �ӵ��� ������
        {
            currentStepInterval = normalStepInterval * 0.6f; // �޸��� �߼Ҹ� ���� (�� ª��)
        }
        // �Ϲ� �ȱ� �ӵ��� ���� normalStepInterval ����

        footstepTimer += Time.deltaTime;

        if (footstepTimer >= currentStepInterval && footstepSources != null && footstepSources.Length > 0 && currentStepInterval > 0 && currentStepInterval != crouchStepInterval)
        {
            footstepTimer = 0f;
            int sourceIndex = Random.Range(0, footstepSources.Length);
            footstepSources[sourceIndex].Play();
        }
    }

    void HandleJumpSound()
    {
        if (controller.isGrounded && !userMove.isJumping)
        {
            hasJumped = false;
        }

        if (userMove.isJumping && !hasJumped && jumpSounds != null && jumpSounds.Length > 0)
        {
            int soundIndex = Random.Range(0, jumpSounds.Length);
            jumpSounds[soundIndex].Play();
            hasJumped = true;
        }
    }
}