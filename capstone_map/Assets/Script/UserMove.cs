using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserMove : MonoBehaviour
{
    private CharacterController controller; // 캐릭터 컨트롤러
    private CrouchHandler crouchHandler;      // 캐릭터 앉기 스크립트

    private Vector3 dir; // 캐릭터 이동 벡터

    [Header("Speed Settings")]
    public float currentSpeed = 2f;  // 현재 이동 속도
    public float walkSpeed = 2f;     // 걷기 이동 속도
    public float crouchSpeed = 1f;   // 앉기 이동 속도
    public float sprintSpeed = 4f;   // 달리기 속도

    [Header("Other Settings")]
    private float jumpForce = 2f;    // 점프력
    public float gravity;          // 캐릭터 적용 중력
    private bool jumpFlag = false; // 점프 상태 판별

    private float moveX = 0f;
    private float moveZ = 0f;

    [Header("Mouse Settings")]
    public Transform viewPivot;    // 머리 회전 기준점
    public Transform headBone;     // 실제 머리 본 (ex: Bip001 Head)
    [SerializeField] private float mouseSpeed = 8f; // 마우스 이동 속도
    private float mouseX = 0f;     // 좌우 회전값 변수
    private float mouseY = 0f;     // 상하 회전값 변수

    public float WalkSpeed => walkSpeed; // 읽기 전용 Getter

    public float CurrentSpeed
    {
        get => currentSpeed;
        set => currentSpeed = Mathf.Clamp(value, 0.5f, 5f); // 이동 속도 값 제한
    }

    // -----------------------------------------------------------------
    // [Header] Footstep Settings
    [Header("Footstep Settings")]
    public AudioSource footstepSource;      // 발소리 재생용 AudioSource
    public AudioClip[] footstepClips;         // 발소리 AudioClip 배열
    public float baseStepInterval = 0.5f;     // 걷기 속도 기준 기본 발걸음 간격 (초)
    private float footstepTimer = 0f;         // 발걸음 타이머
    // -----------------------------------------------------------------

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        crouchHandler = GetComponent<CrouchHandler>();

        dir = Vector3.zero; // 벡터 초기화
        currentSpeed = walkSpeed; // 기본 속도 설정
        gravity = 10f;    // 중력값 설정
    }

    void Update()
    {
        PlayerMove();     // 플레이어 이동
        PlayerCrouch();   // 구르기(앉기)
        ScreenMove();     // 화면(카메라) 이동
        PlayerJump();     // 플레이어 점프
        HandleFootsteps(); // 발소리 처리 추가
    }

    void PlayerMove()
    {
        if (Time.timeScale == 0) return; // 일시정지 중이면 이동 X

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(moveX, 0, moveZ);
        moveDir = controller.transform.TransformDirection(moveDir);

        // 수평 이동 벡터 업데이트 (Y 값은 별도로 점프 처리)
        dir.x = moveDir.x;
        dir.z = moveDir.z;

        // 이동 속도 조정
        if (crouchHandler.IsCrouching())
        {
            CurrentSpeed = crouchSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && GetComponent<PlayerStamina>().currentStamina > 0)
        {
            CurrentSpeed = sprintSpeed;
        }
        else
        {
            CurrentSpeed = walkSpeed;
        }

        controller.Move(dir * currentSpeed * Time.deltaTime);
    }

    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            Debug.Log("점프");
            jumpFlag = true;
            dir.y = jumpForce;
        }

        // 바닥에 있지 않으면 중력 적용
        if (!controller.isGrounded)
        {
            if (jumpFlag)
            {
                dir.y += jumpForce;
                jumpFlag = false;
            }
            else
            {
                dir.y -= gravity * Time.deltaTime;
            }
        }
    }

    void PlayerCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouchHandler.CrouchTransition();
            Debug.Log("Left Control 입력 감지됨. 상태 변경 시도");
        }
    }

    void ScreenMove()
    {
        if (Time.timeScale == 0f) return; // 일시정지 시 마우스 입력 차단

        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY += Input.GetAxis("Mouse Y") * mouseSpeed;
        mouseY = Mathf.Clamp(mouseY, -60f, 60f);

        // 좌우 회전: 플레이어 본체 회전
        transform.rotation = Quaternion.Euler(0, mouseX, 0);

        // 상하 회전: ViewPivot 회전
        viewPivot.localRotation = Quaternion.Euler(-mouseY, 0, 0);
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (headBone == null || viewPivot == null) return;

        Vector3 lookTarget = viewPivot.position + viewPivot.forward * 10f;
        headBone.LookAt(lookTarget);

        Vector3 angles = headBone.localEulerAngles;
        headBone.localEulerAngles = new Vector3(angles.x, angles.y, 0f); // Z축 고정
    }

    /// <summary>
    /// 발소리 로직 처리.
    /// 플레이어가 지면에 있고, 일정 속도로 움직일 때, 일정 간격마다 발소리를 재생합니다.
    /// </summary>
    void HandleFootsteps()
    {
        // 플레이어가 지면에 있고, 상당한 수평 이동이 있을 때 발소리 발동
        Vector3 horizontalMove = new Vector3(dir.x, 0, dir.z);
        if (controller.isGrounded && horizontalMove.magnitude > 0.1f)
        {
            // 발소리 타이머 누적
            footstepTimer += Time.deltaTime;

            // 현재 속도에 따라 발걸음 간격 조절 (속도가 빠르면 간격 짧음)
            // 기본 간격(baseStepInterval)이 걷기 속도(walkSpeed) 기준일 때,
            // 현재 속도가 빨라지면 비례해서 간격을 줄인다.
            float stepInterval = baseStepInterval * (walkSpeed / currentSpeed);

            if (footstepTimer >= stepInterval)
            {
                footstepTimer = 0f;
                // 발소리 AudioSource와 클립들이 올바르게 할당되어 있는지 확인
                if (footstepSource != null && footstepClips != null && footstepClips.Length > 0)
                {
                    // 랜덤으로 발소리 클립 선택
                    int clipIndex = Random.Range(0, footstepClips.Length);
                    // 볼륨: 현재 속도에 비례 (예: 달릴 때 강하게)
                    // 여기는 스프린트 속도 기준으로 정규화하되, 최소 0.3 정도는 유지.
                    float volume = Mathf.Clamp(currentSpeed / sprintSpeed, 0.3f, 1f);
                    footstepSource.PlayOneShot(footstepClips[clipIndex], volume);
                }
            }
        }
        else
        {
            // 이동이 없거나 공중이면 타이머 리셋
            footstepTimer = 0f;
        }
    }
}
