/*using AiSoundDetect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserMove : MonoBehaviour
{
    private CharacterController controller; // 캐릭터 컨트롤러
    private CrouchHandler crouchHandler;    // 캐릭터 앉기 스크립트

    private Vector3 dir; // 캐릭터 이동 벡터

    [Header("Speed Settings")]
    public float currentSpeed = 1f;     // 현재 이동 속도
    public float walkSpeed = 1f;        // 걷기 이동 속도
    public float crouchSpeed = 0.6f;    // 앉기 이동 속도
    public float sprintSpeed = 1.6f;    // 달리기 속도

    [Header("Other Settings")]
    private bool jumpFlag = false;          // 점프 상태 판별
    public float baseJumpHeight = 0.6f;     // 일반 점프력
    public float runJumpExtra = 0.3f;       // 달릴 때 점프력
    public float gravity = 7;               // 스케일 보정된 중력
    public float jumpFallMultiplier = 0.6f; // 하강 시 중력 배율
    public float jumpRiseMultiplier = 0.9f; // 상승 시 중력 배율


    private float moveX = 0f;
    private float moveZ = 0f;

    [Header("Mouse Settings")]
    public Transform viewPivot;         // 머리 회전 기준점
    public Transform headBone;          // 실제 머리 본 (ex: Bip001 Head)
    [SerializeField] private float mouseSpeed = 8f; // 마우스 이동 속도
    private float mouseX = 0f;          // 좌우 회전값 변수
    private float mouseY = 0f;          // 상하 회전값 변수

    public float WalkSpeed => walkSpeed; // 읽기 전용 Getter

    [Header("Sound Emitter Settings")]
    public Sound_Emitter footstepEmitter; // 발소리 SoundEmitter 컴포넌트 (에디터에서 할당)
    public AudioClip[] footstepClips;     // 발소리 AudioClip 배열
    public float baseStepInterval = 0.5f; // 걷기 속도 기준 기본 발걸음 간격 (초)
    private float footstepTimer = 0f;     // 발걸음 타이머

    public float CurrentSpeed
    {
        get => currentSpeed;
        set => currentSpeed = Mathf.Clamp(value, 0.5f, 5f); // 이동 속도 값 제한
    }

    // -----------------------------------------------------------------
    // [Header] Footstep Settings
    // [Header("Footstep Settings")]
    // public AudioSource footstepSource;          // 발소리 재생용 AudioSource
    // public AudioClip[] footstepClips;          // 발소리 AudioClip 배열
    // public float baseStepInterval = 0.5f;      // 걷기 속도 기준 기본 발걸음 간격 (초)
    // private float footstepTimer = 0f;          // 발걸음 타이머
    // -----------------------------------------------------------------

    // [Header("Sound Emitter Settings")]
    // public GameObject footstepEmitterPrefab; // 발소리 SoundEmitter 프리팹

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        crouchHandler = GetComponent<CrouchHandler>();

        dir = Vector3.zero; // 벡터 초기화
        currentSpeed = walkSpeed; // 기본 속도 설정
        //gravity = 1f;      // 중력값 설정


        // 마우스 중앙 고정 포인터 숨기기
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Sound_Emitter 컴포넌트가 할당되지 않았을 경우 현재 오브젝트에서 찾음
        if (footstepEmitter == null)
        {
            footstepEmitter = GetComponent<Sound_Emitter>();
            if (footstepEmitter == null)
            {
                Debug.LogError("Sound_Emitter 컴포넌트가 필요합니다.");
                enabled = false; // 컴포넌트 비활성화
            }
            else
            {
                // AudioMethod를 AudioClip으로 설정 (Sound_Emitter를 직접 사용하므로)
                footstepEmitter.AudioMethod = Sound_Emitter.audioChoice.AudioClip;
            }
        }
        else
        {
            // AudioMethod를 AudioClip으로 설정 (Sound_Emitter를 직접 사용하므로)
            footstepEmitter.AudioMethod = Sound_Emitter.audioChoice.AudioClip;
        }
    }

    void Update()
    {
        PlayerMove();      // 플레이어 이동
        PlayerCrouch();    // 구르기(앉기)
        ScreenMove();      // 화면(카메라) 이동
        PlayerJump();      // 플레이어 점프
        HandleFootsteps(); // 발소리 처리
    }



    /* void PlayerMove()
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

     */
    void PlayerMove()
    {
        if (Time.timeScale == 0) return; // 일시정지 중이면 무시

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(moveX, 0, moveZ);
        moveDir = controller.transform.TransformDirection(moveDir);

        dir.x = moveDir.x;
        dir.z = moveDir.z;

        // 현재 속도 설정
        if (crouchHandler.IsCrouching())
        {
            currentSpeed = crouchSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && GetComponent<PlayerStamina>().currentStamina > 0)
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        // Y는 점프/중력에 따로 맡기고, 이동은 XZ만 속도 곱해서 처리
        Vector3 moveXZ = new Vector3(dir.x, 0, dir.z) * currentSpeed;
        Vector3 moveY = new Vector3(0, dir.y, 0);
        Vector3 finalMove = moveXZ + moveY;

        controller.Move(finalMove * Time.deltaTime);
        //Debug.Log($"점프시 dir.y: {dir.y}, isGrounded: {controller.isGrounded}");

    }
    /*
    void PlayerJump()
    {
        // 점프 입력 & 착지 상태일 때
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            jumpFlag = true;

            // 현재 속도 기반 점프력 계산
            float finalJumpForce = baseJumpForce + (currentSpeed * speedMultiplier);

            // 중력 기반 점프 초기속도 계산 (v = sqrt(2gh) 응용)
            dir.y = Mathf.Sqrt(finalJumpForce * 2f * gravity);

            Debug.Log($"점프: 최종점프력={finalJumpForce}, 초기속도={dir.y}");
        }

        // 공중에 있을 때 중력 적용
        if (!controller.isGrounded)
        {
            if (jumpFlag)
            {
                jumpFlag = false; // 한 번만 점프 가능
            }
            else
            {
                dir.y -= gravity * Time.deltaTime;
            }
        }   

        // 땅에 닿았을 때 Y속도 초기화
        if (controller.isGrounded && dir.y < 0f)
        {
            dir.y = -2f; // 가볍게 눌러붙게
        }
    }
    */
    void PlayerJump()
    {
        // 점프 시도 (스페이스 + 지면)
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            float jumpHeight = baseJumpHeight;

            // 달리는 중이면 추가 높이
            if (Input.GetKey(KeyCode.LeftShift))
                jumpHeight += runJumpExtra;

            dir.y = Mathf.Sqrt(jumpHeight * 2f * gravity); // 초기 점프 속도 계산
            jumpFlag = true;
        }

        // 공중 중력 처리 << 왜 적용해도 거의 차이 없게 느껴지지
        if (!controller.isGrounded)
        {
            if (dir.y > 0)
            {
                // 상승 중 : 중력 감속을 완화시켜 더 오래 정점에 머물게
                dir.y -= gravity * jumpRiseMultiplier * Time.deltaTime;
            }
            else
            {
                // 하강 중 : 더 빠르게 떨어지게 하여 포물선 완성
                dir.y -= gravity * jumpFallMultiplier * Time.deltaTime;
            }

            jumpFlag = false; // 공중에 뜨면 jumpFlag 초기화
        }

        // 땅에 닿았으면 y속도 초기화
        if (controller.isGrounded && dir.y < 0)
        {
            dir.y = -2f; // 땅에 붙게 << 의미 없어보이는데 일단 적용함
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

    void HandleFootsteps()
    {
        Vector3 horizontalMove = new Vector3(dir.x, 0, dir.z);
        if (controller.isGrounded && horizontalMove.magnitude > 0.1f)
        {
            footstepTimer += Time.deltaTime;
            float stepInterval = baseStepInterval * (walkSpeed / currentSpeed);

            if (footstepTimer >= stepInterval)
            {
                footstepTimer = 0f;

                // 발소리 재생 (Sound_Emitter 사용)
                if (footstepEmitter != null && footstepClips != null && footstepClips.Length > 0)
                {
                    int clipIndex = Random.Range(0, footstepClips.Length);
                    footstepEmitter.m_AudioClip = footstepClips[clipIndex];
                    footstepEmitter.m_Pitch = Time.timeScale > 0 ? 1f : 0f; // 일시정지 시 피치 0으로 설정
                    footstepEmitter.ClipPlay(); // Sound_Emitter의 ClipPlay 함수 호출
                }
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    /*
    // 수정 전 버전
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
    */

/*
void HandleFootsteps()
{
    Vector3 horizontalMove = new Vector3(dir.x, 0, dir.z);
    if (controller.isGrounded && horizontalMove.magnitude > 0.1f)
    {
        footstepTimer += Time.deltaTime;
        float stepInterval = baseStepInterval * (walkSpeed / currentSpeed);

        if (footstepTimer >= stepInterval)
        {
            footstepTimer = 0f;

            // 발소리 오디오 재생
            if (footstepSource != null && footstepClips != null && footstepClips.Length > 0)
            {
                int clipIndex = Random.Range(0, footstepClips.Length);
                float volume = Mathf.Clamp(currentSpeed / sprintSpeed, 0.3f, 1f);
                footstepSource.PlayOneShot(footstepClips[clipIndex], volume);
            }

            //  발소리 SoundEmitter 생성
            if (footstepEmitterPrefab != null)
            {
                // 현재 플레이어 위치에 생성
                GameObject emitter = Instantiate(footstepEmitterPrefab, transform.position, Quaternion.identity);

                // Sound_Emitter 스크립트가 있다면, 범위 설정
                Sound_Emitter emitterScript = emitter.GetComponent<Sound_Emitter>();
                if (emitterScript != null)
                {
                    //emitterScript.soundRange = 5f; // 발소리 범위 설정 (원하는 값으로)
                    //emitterScript.lifetime = 0.5f; // 몇 초 동안 유지할지 설정
                }
            }
        }
    }
    else
    {
        footstepTimer = 0f;
    }
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMove : MonoBehaviour
{
    private CharacterController controller; // 캐릭터 컨트롤러
    private CrouchHandler crouchHandler;     // 캐릭터 앉기 스크립트

    private Vector3 dir; // 캐릭터 이동 벡터

    public Vector3 MoveDirection => dir;
    public bool IsJumping => jumpFlag; // 점프 상태를 외부에서 알 수 있도록 함

    [Header("Speed Settings")]
    public float currentSpeed = 1f;     // 현재 이동 속도
    public float walkSpeed = 1f;        // 걷기 이동 속도
    public float crouchSpeed = 0.6f;      // 앉기 이동 속도
    public float sprintSpeed = 1.6f;      // 달리기 속도

    [Header("Other Settings")]
    public float jumpForce = 2.2f;        // 점프력
    public float gravity;                // 캐릭터 적용 중력
    private bool jumpFlag = false;       // 점프 상태 판별

    private float moveX = 0f;
    private float moveZ = 0f;

    [Header("Mouse Settings")]
    public Transform viewPivot;          // 머리 회전 기준점
    public Transform headBone;           // 실제 머리 본 (ex: Bip001 Head)
    [SerializeField] private float mouseSpeed = 8f; // 마우스 이동 속도
    private float mouseX = 0f;            // 좌우 회전값 변수
    private float mouseY = 0f;            // 상하 회전값 변수

    public float WalkSpeed => walkSpeed; // 읽기 전용 Getter

    public float CurrentSpeed
    {
        get => currentSpeed;
        set => currentSpeed = Mathf.Clamp(value, 0.5f, 5f); // 이동 속도 값 제한
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        crouchHandler = GetComponent<CrouchHandler>();

        dir = Vector3.zero; // 벡터 초기화
        currentSpeed = walkSpeed; // 기본 속도 설정
        gravity = 10f;       // 중력값 설정

        // 마우스 중앙 고정 포인터 숨기기
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        PlayerMove();     // 플레이어 이동
        PlayerCrouch();   // 구르기(앉기)
        ScreenMove();     // 화면(카메라) 이동
        PlayerJump();     // 플레이어 점프
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
                jumpFlag = false;
            }
            else
            {
                dir.y -= gravity * Time.deltaTime * 0.4f;
            }
        }

        // 바닥에 있으면
        if (controller.isGrounded && dir.y < 0)
        {
            dir.y = Mathf.Lerp(dir.y, -2f, Time.deltaTime * 20f);
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
}