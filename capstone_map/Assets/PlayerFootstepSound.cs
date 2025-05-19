using UnityEngine;
using AiSoundDetect;

[RequireComponent(typeof(UserMove))]
public class PlayerFootstepSound : MonoBehaviour
{
    [Header("발소리 간격")]
    public float walkStepInterval = 0.5f;
    public float runStepInterval = 0.3f;

    [Header("사운드 구성")]
    public AudioSource footstepSource;           // 발소리를 재생할 AudioSource
    public Sound_Emitter soundEmitter;           // Sound_Emitter

    private UserMove userMove;                   // UserMove 스크립트 참조
    private float stepTimer;

    void Start()
    {
        userMove = GetComponent<UserMove>();

        if (footstepSource == null)
            Debug.LogError("Footstep AudioSource가 지정되지 않았습니다.");

        if (soundEmitter == null)
            Debug.LogError("Sound_Emitter가 지정되지 않았습니다.");

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

        // 사운드 감지 트리거
        if (!soundEmitter.startMethod)
        {
            soundEmitter.startMethod = true;
        }
    }

    bool IsGrounded()
    {
        // CharacterController.isGrounded 참조
        return GetComponent<CharacterController>().isGrounded;
    }
}
