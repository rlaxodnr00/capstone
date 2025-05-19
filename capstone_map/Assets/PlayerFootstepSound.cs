using UnityEngine;
using AiSoundDetect;

[RequireComponent(typeof(UserMove))]
public class PlayerFootstepSound : MonoBehaviour
{
    [Header("�߼Ҹ� ����")]
    public float walkStepInterval = 0.5f;
    public float runStepInterval = 0.3f;

    [Header("���� ����")]
    public AudioSource footstepSource;           // �߼Ҹ��� ����� AudioSource
    public Sound_Emitter soundEmitter;           // Sound_Emitter

    private UserMove userMove;                   // UserMove ��ũ��Ʈ ����
    private float stepTimer;

    void Start()
    {
        userMove = GetComponent<UserMove>();

        if (footstepSource == null)
            Debug.LogError("Footstep AudioSource�� �������� �ʾҽ��ϴ�.");

        if (soundEmitter == null)
            Debug.LogError("Sound_Emitter�� �������� �ʾҽ��ϴ�.");

        soundEmitter.AudioMethod = Sound_Emitter.audioChoice.AudioSource;
        soundEmitter.objectEmitterSource = footstepSource;
    }

    void Update()
    {
        if (!IsGrounded()) return;

        Vector3 move = userMove.MoveDirection;
        float moveAmount = new Vector3(move.x, 0, move.z).magnitude;

        bool isMoving = moveAmount > 0.1f;
        bool isCrouching = userMove.CurrentSpeed == userMove.crouchSpeed;
        bool isRunning = userMove.CurrentSpeed == userMove.sprintSpeed;

        if (isMoving && !isCrouching)
        {
            float interval = isRunning ? runStepInterval : walkStepInterval;
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = interval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void PlayFootstep()
    {
        if (!footstepSource.isPlaying)
        {
            footstepSource.Play();
        }

        // ���� ���� Ʈ����
        if (!soundEmitter.startMethod)
        {
            soundEmitter.startMethod = true;
        }
    }

    bool IsGrounded()
    {
        // CharacterController.isGrounded ����
        return GetComponent<CharacterController>().isGrounded;
    }
}
