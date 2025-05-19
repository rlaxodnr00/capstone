using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepHandler : MonoBehaviour
{
    private CharacterController controller; // 캐릭터 컨트롤러
    private UserMove userMove;             // UserMove 스크립트 참조
    private CrouchHandler crouchHandler;   // 앉기 상태 확인을 위한 CrouchHandler 참조

    [Header("Audio Sources")]
    public AudioSource[] footstepSources;  // 발소리 재생용 AudioSource 배열 (에디터에서 할당)
    public AudioSource[] jumpSounds;       // 점프 소리 AudioSource 배열

    [Header("Step Intervals")]
    public float normalStepInterval = 0.4f; // 평상시/달리기 이동 발걸음 간격 (초)
    public float crouchStepInterval = 0.6f; // 앉아서 걷기 발걸음 간격 (발소리 X)

    private float footstepTimer = 0f;       // 발걸음 타이머
    private bool isMoving = false;         // 현재 이동 중인지 여부
    private bool hasJumped = false;        // 점프 사운드 중복 재생 방지

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        userMove = GetComponent<UserMove>();
        crouchHandler = GetComponent<CrouchHandler>(); // CrouchHandler 컴포넌트 가져오기

        if (userMove == null)
        {
            Debug.LogError("UserMove 스크립트가 필요합니다.");
            enabled = false;
            return;
        }

        if (crouchHandler == null)
        {
            Debug.LogError("CrouchHandler 스크립트가 필요합니다.");
            enabled = false;
            return;
        }

        if (footstepSources == null || footstepSources.Length == 0)
        {
            Debug.LogError("Footstep Sources AudioSource 배열이 비어 있습니다.");
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

        if (crouchHandler.IsCrouching()) // 앉아있는 상태라면
        {
            currentStepInterval = crouchStepInterval; // 발소리 안 나는 간격으로 설정
        }
        else if (userMove.CurrentSpeed >= userMove.sprintSpeed - 0.05f) // 달리기 속도에 가까우면
        {
            currentStepInterval = normalStepInterval * 0.6f; // 달리기 발소리 간격 (더 짧게)
        }
        // 일반 걷기 속도일 때는 normalStepInterval 유지

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
<<<<<<< HEAD
        if (controller.isGrounded && !userMove.isJumping)
=======
        if (controller.isGrounded && !userMove.IsJumping)
>>>>>>> b1a15ef7e75e224d0b87ccbfe86cfe42190c7f51
        {
            hasJumped = false;
        }

<<<<<<< HEAD
        if (userMove.isJumping && !hasJumped && jumpSounds != null && jumpSounds.Length > 0)
=======
        if (userMove.IsJumping && !hasJumped && jumpSounds != null && jumpSounds.Length > 0)
>>>>>>> b1a15ef7e75e224d0b87ccbfe86cfe42190c7f51
        {
            int soundIndex = Random.Range(0, jumpSounds.Length);
            jumpSounds[soundIndex].Play();
            hasJumped = true;
        }
    }
}