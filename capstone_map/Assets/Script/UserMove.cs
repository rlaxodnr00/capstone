/*using AiSoundDetect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserMove : MonoBehaviour
{
    private CharacterController controller; // ĳ���� ��Ʈ�ѷ�
    private CrouchHandler crouchHandler;    // ĳ���� �ɱ� ��ũ��Ʈ

    private Vector3 dir; // ĳ���� �̵� ����

    [Header("Speed Settings")]
    public float currentSpeed = 1f;     // ���� �̵� �ӵ�
    public float walkSpeed = 1f;        // �ȱ� �̵� �ӵ�
    public float crouchSpeed = 0.6f;    // �ɱ� �̵� �ӵ�
    public float sprintSpeed = 1.6f;    // �޸��� �ӵ�

    [Header("Other Settings")]
    private bool jumpFlag = false;          // ���� ���� �Ǻ�
    public float baseJumpHeight = 0.6f;     // �Ϲ� ������
    public float runJumpExtra = 0.3f;       // �޸� �� ������
    public float gravity = 7;               // ������ ������ �߷�
    public float jumpFallMultiplier = 0.6f; // �ϰ� �� �߷� ����
    public float jumpRiseMultiplier = 0.9f; // ��� �� �߷� ����


    private float moveX = 0f;
    private float moveZ = 0f;

    [Header("Mouse Settings")]
    public Transform viewPivot;         // �Ӹ� ȸ�� ������
    public Transform headBone;          // ���� �Ӹ� �� (ex: Bip001 Head)
    [SerializeField] private float mouseSpeed = 8f; // ���콺 �̵� �ӵ�
    private float mouseX = 0f;          // �¿� ȸ���� ����
    private float mouseY = 0f;          // ���� ȸ���� ����

    public float WalkSpeed => walkSpeed; // �б� ���� Getter

    [Header("Sound Emitter Settings")]
    public Sound_Emitter footstepEmitter; // �߼Ҹ� SoundEmitter ������Ʈ (�����Ϳ��� �Ҵ�)
    public AudioClip[] footstepClips;     // �߼Ҹ� AudioClip �迭
    public float baseStepInterval = 0.5f; // �ȱ� �ӵ� ���� �⺻ �߰��� ���� (��)
    private float footstepTimer = 0f;     // �߰��� Ÿ�̸�

    public float CurrentSpeed
    {
        get => currentSpeed;
        set => currentSpeed = Mathf.Clamp(value, 0.5f, 5f); // �̵� �ӵ� �� ����
    }

    // -----------------------------------------------------------------
    // [Header] Footstep Settings
    // [Header("Footstep Settings")]
    // public AudioSource footstepSource;          // �߼Ҹ� ����� AudioSource
    // public AudioClip[] footstepClips;          // �߼Ҹ� AudioClip �迭
    // public float baseStepInterval = 0.5f;      // �ȱ� �ӵ� ���� �⺻ �߰��� ���� (��)
    // private float footstepTimer = 0f;          // �߰��� Ÿ�̸�
    // -----------------------------------------------------------------

    // [Header("Sound Emitter Settings")]
    // public GameObject footstepEmitterPrefab; // �߼Ҹ� SoundEmitter ������

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        crouchHandler = GetComponent<CrouchHandler>();

        dir = Vector3.zero; // ���� �ʱ�ȭ
        currentSpeed = walkSpeed; // �⺻ �ӵ� ����
        //gravity = 1f;      // �߷°� ����


        // ���콺 �߾� ���� ������ �����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Sound_Emitter ������Ʈ�� �Ҵ���� �ʾ��� ��� ���� ������Ʈ���� ã��
        if (footstepEmitter == null)
        {
            footstepEmitter = GetComponent<Sound_Emitter>();
            if (footstepEmitter == null)
            {
                Debug.LogError("Sound_Emitter ������Ʈ�� �ʿ��մϴ�.");
                enabled = false; // ������Ʈ ��Ȱ��ȭ
            }
            else
            {
                // AudioMethod�� AudioClip���� ���� (Sound_Emitter�� ���� ����ϹǷ�)
                footstepEmitter.AudioMethod = Sound_Emitter.audioChoice.AudioClip;
            }
        }
        else
        {
            // AudioMethod�� AudioClip���� ���� (Sound_Emitter�� ���� ����ϹǷ�)
            footstepEmitter.AudioMethod = Sound_Emitter.audioChoice.AudioClip;
        }
    }

    void Update()
    {
        PlayerMove();      // �÷��̾� �̵�
        PlayerCrouch();    // ������(�ɱ�)
        ScreenMove();      // ȭ��(ī�޶�) �̵�
        PlayerJump();      // �÷��̾� ����
        HandleFootsteps(); // �߼Ҹ� ó��
    }



    /* void PlayerMove()
     {
         if (Time.timeScale == 0) return; // �Ͻ����� ���̸� �̵� X

         moveX = Input.GetAxis("Horizontal");
         moveZ = Input.GetAxis("Vertical");

         Vector3 moveDir = new Vector3(moveX, 0, moveZ);
         moveDir = controller.transform.TransformDirection(moveDir);

         // ���� �̵� ���� ������Ʈ (Y ���� ������ ���� ó��)
         dir.x = moveDir.x;
         dir.z = moveDir.z;

         // �̵� �ӵ� ����
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
        if (Time.timeScale == 0) return; // �Ͻ����� ���̸� ����

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(moveX, 0, moveZ);
        moveDir = controller.transform.TransformDirection(moveDir);

        dir.x = moveDir.x;
        dir.z = moveDir.z;

        // ���� �ӵ� ����
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

        // Y�� ����/�߷¿� ���� �ñ��, �̵��� XZ�� �ӵ� ���ؼ� ó��
        Vector3 moveXZ = new Vector3(dir.x, 0, dir.z) * currentSpeed;
        Vector3 moveY = new Vector3(0, dir.y, 0);
        Vector3 finalMove = moveXZ + moveY;

        controller.Move(finalMove * Time.deltaTime);
        //Debug.Log($"������ dir.y: {dir.y}, isGrounded: {controller.isGrounded}");

    }
    /*
    void PlayerJump()
    {
        // ���� �Է� & ���� ������ ��
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            jumpFlag = true;

            // ���� �ӵ� ��� ������ ���
            float finalJumpForce = baseJumpForce + (currentSpeed * speedMultiplier);

            // �߷� ��� ���� �ʱ�ӵ� ��� (v = sqrt(2gh) ����)
            dir.y = Mathf.Sqrt(finalJumpForce * 2f * gravity);

            Debug.Log($"����: ����������={finalJumpForce}, �ʱ�ӵ�={dir.y}");
        }

        // ���߿� ���� �� �߷� ����
        if (!controller.isGrounded)
        {
            if (jumpFlag)
            {
                jumpFlag = false; // �� ���� ���� ����
            }
            else
            {
                dir.y -= gravity * Time.deltaTime;
            }
        }   

        // ���� ����� �� Y�ӵ� �ʱ�ȭ
        if (controller.isGrounded && dir.y < 0f)
        {
            dir.y = -2f; // ������ �����ٰ�
        }
    }
    */
    void PlayerJump()
    {
        // ���� �õ� (�����̽� + ����)
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            float jumpHeight = baseJumpHeight;

            // �޸��� ���̸� �߰� ����
            if (Input.GetKey(KeyCode.LeftShift))
                jumpHeight += runJumpExtra;

            dir.y = Mathf.Sqrt(jumpHeight * 2f * gravity); // �ʱ� ���� �ӵ� ���
            jumpFlag = true;
        }

        // ���� �߷� ó�� << �� �����ص� ���� ���� ���� ��������
        if (!controller.isGrounded)
        {
            if (dir.y > 0)
            {
                // ��� �� : �߷� ������ ��ȭ���� �� ���� ������ �ӹ���
                dir.y -= gravity * jumpRiseMultiplier * Time.deltaTime;
            }
            else
            {
                // �ϰ� �� : �� ������ �������� �Ͽ� ������ �ϼ�
                dir.y -= gravity * jumpFallMultiplier * Time.deltaTime;
            }

            jumpFlag = false; // ���߿� �߸� jumpFlag �ʱ�ȭ
        }

        // ���� ������� y�ӵ� �ʱ�ȭ
        if (controller.isGrounded && dir.y < 0)
        {
            dir.y = -2f; // ���� �ٰ� << �ǹ� ����̴µ� �ϴ� ������
        }
    }


    void PlayerCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouchHandler.CrouchTransition();
            Debug.Log("Left Control �Է� ������. ���� ���� �õ�");
        }
    }

    void ScreenMove()
    {
        if (Time.timeScale == 0f) return; // �Ͻ����� �� ���콺 �Է� ����

        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY += Input.GetAxis("Mouse Y") * mouseSpeed;
        mouseY = Mathf.Clamp(mouseY, -60f, 60f);

        // �¿� ȸ��: �÷��̾� ��ü ȸ��
        transform.rotation = Quaternion.Euler(0, mouseX, 0);

        // ���� ȸ��: ViewPivot ȸ��
        viewPivot.localRotation = Quaternion.Euler(-mouseY, 0, 0);
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (headBone == null || viewPivot == null) return;

        Vector3 lookTarget = viewPivot.position + viewPivot.forward * 10f;
        headBone.LookAt(lookTarget);

        Vector3 angles = headBone.localEulerAngles;
        headBone.localEulerAngles = new Vector3(angles.x, angles.y, 0f); // Z�� ����
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

                // �߼Ҹ� ��� (Sound_Emitter ���)
                if (footstepEmitter != null && footstepClips != null && footstepClips.Length > 0)
                {
                    int clipIndex = Random.Range(0, footstepClips.Length);
                    footstepEmitter.m_AudioClip = footstepClips[clipIndex];
                    footstepEmitter.m_Pitch = Time.timeScale > 0 ? 1f : 0f; // �Ͻ����� �� ��ġ 0���� ����
                    footstepEmitter.ClipPlay(); // Sound_Emitter�� ClipPlay �Լ� ȣ��
                }
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    /*
    // ���� �� ����
    void HandleFootsteps()
    {
        // �÷��̾ ���鿡 �ְ�, ����� ���� �̵��� ���� �� �߼Ҹ� �ߵ�
        Vector3 horizontalMove = new Vector3(dir.x, 0, dir.z);
        if (controller.isGrounded && horizontalMove.magnitude > 0.1f)
        {
            // �߼Ҹ� Ÿ�̸� ����
            footstepTimer += Time.deltaTime;

            // ���� �ӵ��� ���� �߰��� ���� ���� (�ӵ��� ������ ���� ª��)
            // �⺻ ����(baseStepInterval)�� �ȱ� �ӵ�(walkSpeed) ������ ��,
            // ���� �ӵ��� �������� ����ؼ� ������ ���δ�.
            float stepInterval = baseStepInterval * (walkSpeed / currentSpeed);

            if (footstepTimer >= stepInterval)
            {
                footstepTimer = 0f;
                // �߼Ҹ� AudioSource�� Ŭ������ �ùٸ��� �Ҵ�Ǿ� �ִ��� Ȯ��
                if (footstepSource != null && footstepClips != null && footstepClips.Length > 0)
                {
                    // �������� �߼Ҹ� Ŭ�� ����
                    int clipIndex = Random.Range(0, footstepClips.Length);
                    // ����: ���� �ӵ��� ��� (��: �޸� �� ���ϰ�)
                    // ����� ������Ʈ �ӵ� �������� ����ȭ�ϵ�, �ּ� 0.3 ������ ����.
                    float volume = Mathf.Clamp(currentSpeed / sprintSpeed, 0.3f, 1f);
                    footstepSource.PlayOneShot(footstepClips[clipIndex], volume);
                }
            }
        }
        else
        {
            // �̵��� ���ų� �����̸� Ÿ�̸� ����
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

            // �߼Ҹ� ����� ���
            if (footstepSource != null && footstepClips != null && footstepClips.Length > 0)
            {
                int clipIndex = Random.Range(0, footstepClips.Length);
                float volume = Mathf.Clamp(currentSpeed / sprintSpeed, 0.3f, 1f);
                footstepSource.PlayOneShot(footstepClips[clipIndex], volume);
            }

            //  �߼Ҹ� SoundEmitter ����
            if (footstepEmitterPrefab != null)
            {
                // ���� �÷��̾� ��ġ�� ����
                GameObject emitter = Instantiate(footstepEmitterPrefab, transform.position, Quaternion.identity);

                // Sound_Emitter ��ũ��Ʈ�� �ִٸ�, ���� ����
                Sound_Emitter emitterScript = emitter.GetComponent<Sound_Emitter>();
                if (emitterScript != null)
                {
                    //emitterScript.soundRange = 5f; // �߼Ҹ� ���� ���� (���ϴ� ������)
                    //emitterScript.lifetime = 0.5f; // �� �� ���� �������� ����
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
    private CharacterController controller; // ĳ���� ��Ʈ�ѷ�
    private CrouchHandler crouchHandler;     // ĳ���� �ɱ� ��ũ��Ʈ

    private Vector3 dir; // ĳ���� �̵� ����

    public Vector3 MoveDirection => dir;
    public bool IsJumping => jumpFlag; // ���� ���¸� �ܺο��� �� �� �ֵ��� ��

    [Header("Speed Settings")]
    public float currentSpeed = 1f;     // ���� �̵� �ӵ�
    public float walkSpeed = 1f;        // �ȱ� �̵� �ӵ�
    public float crouchSpeed = 0.6f;      // �ɱ� �̵� �ӵ�
    public float sprintSpeed = 1.6f;      // �޸��� �ӵ�

    [Header("Other Settings")]
    public float jumpForce = 2.2f;        // ������
    public float gravity;                // ĳ���� ���� �߷�
    private bool jumpFlag = false;       // ���� ���� �Ǻ�

    private float moveX = 0f;
    private float moveZ = 0f;

    [Header("Mouse Settings")]
    public Transform viewPivot;          // �Ӹ� ȸ�� ������
    public Transform headBone;           // ���� �Ӹ� �� (ex: Bip001 Head)
    [SerializeField] private float mouseSpeed = 8f; // ���콺 �̵� �ӵ�
    private float mouseX = 0f;            // �¿� ȸ���� ����
    private float mouseY = 0f;            // ���� ȸ���� ����

    public float WalkSpeed => walkSpeed; // �б� ���� Getter

    public float CurrentSpeed
    {
        get => currentSpeed;
        set => currentSpeed = Mathf.Clamp(value, 0.5f, 5f); // �̵� �ӵ� �� ����
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        crouchHandler = GetComponent<CrouchHandler>();

        dir = Vector3.zero; // ���� �ʱ�ȭ
        currentSpeed = walkSpeed; // �⺻ �ӵ� ����
        gravity = 10f;       // �߷°� ����

        // ���콺 �߾� ���� ������ �����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        PlayerMove();     // �÷��̾� �̵�
        PlayerCrouch();   // ������(�ɱ�)
        ScreenMove();     // ȭ��(ī�޶�) �̵�
        PlayerJump();     // �÷��̾� ����
    }

    void PlayerMove()
    {
        if (Time.timeScale == 0) return; // �Ͻ����� ���̸� �̵� X

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(moveX, 0, moveZ);
        moveDir = controller.transform.TransformDirection(moveDir);

        // ���� �̵� ���� ������Ʈ (Y ���� ������ ���� ó��)
        dir.x = moveDir.x;
        dir.z = moveDir.z;

        // �̵� �ӵ� ����
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
            Debug.Log("����");
            jumpFlag = true;
            dir.y = jumpForce;
        }

        // �ٴڿ� ���� ������ �߷� ����
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

        // �ٴڿ� ������
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
            Debug.Log("Left Control �Է� ������. ���� ���� �õ�");
        }
    }

    void ScreenMove()
    {
        if (Time.timeScale == 0f) return; // �Ͻ����� �� ���콺 �Է� ����

        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY += Input.GetAxis("Mouse Y") * mouseSpeed;
        mouseY = Mathf.Clamp(mouseY, -60f, 60f);

        // �¿� ȸ��: �÷��̾� ��ü ȸ��
        transform.rotation = Quaternion.Euler(0, mouseX, 0);

        // ���� ȸ��: ViewPivot ȸ��
        viewPivot.localRotation = Quaternion.Euler(-mouseY, 0, 0);
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (headBone == null || viewPivot == null) return;

        Vector3 lookTarget = viewPivot.position + viewPivot.forward * 10f;
        headBone.LookAt(lookTarget);

        Vector3 angles = headBone.localEulerAngles;
        headBone.localEulerAngles = new Vector3(angles.x, angles.y, 0f); // Z�� ����
    }
}