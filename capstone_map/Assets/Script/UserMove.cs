using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserMove : MonoBehaviour
{
    private CharacterController controller; // ĳ���� ��Ʈ�ѷ�
    private CrouchHandler crouchHandler;      // ĳ���� �ɱ� ��ũ��Ʈ

    private Vector3 dir; // ĳ���� �̵� ����

    [Header("Speed Settings")]
    public float currentSpeed = 2f;  // ���� �̵� �ӵ�
    public float walkSpeed = 2f;     // �ȱ� �̵� �ӵ�
    public float crouchSpeed = 1f;   // �ɱ� �̵� �ӵ�
    public float sprintSpeed = 4f;   // �޸��� �ӵ�

    [Header("Other Settings")]
    private float jumpForce = 2f;    // ������
    public float gravity;          // ĳ���� ���� �߷�
    private bool jumpFlag = false; // ���� ���� �Ǻ�

    private float moveX = 0f;
    private float moveZ = 0f;

    [Header("Mouse Settings")]
    public Transform viewPivot;    // �Ӹ� ȸ�� ������
    public Transform headBone;     // ���� �Ӹ� �� (ex: Bip001 Head)
    [SerializeField] private float mouseSpeed = 8f; // ���콺 �̵� �ӵ�
    private float mouseX = 0f;     // �¿� ȸ���� ����
    private float mouseY = 0f;     // ���� ȸ���� ����

    public float WalkSpeed => walkSpeed; // �б� ���� Getter

    public float CurrentSpeed
    {
        get => currentSpeed;
        set => currentSpeed = Mathf.Clamp(value, 0.5f, 5f); // �̵� �ӵ� �� ����
    }

    // -----------------------------------------------------------------
    // [Header] Footstep Settings
    [Header("Footstep Settings")]
    public AudioSource footstepSource;      // �߼Ҹ� ����� AudioSource
    public AudioClip[] footstepClips;         // �߼Ҹ� AudioClip �迭
    public float baseStepInterval = 0.5f;     // �ȱ� �ӵ� ���� �⺻ �߰��� ���� (��)
    private float footstepTimer = 0f;         // �߰��� Ÿ�̸�
    // -----------------------------------------------------------------

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        crouchHandler = GetComponent<CrouchHandler>();

        dir = Vector3.zero; // ���� �ʱ�ȭ
        currentSpeed = walkSpeed; // �⺻ �ӵ� ����
        gravity = 10f;    // �߷°� ����
    }

    void Update()
    {
        PlayerMove();     // �÷��̾� �̵�
        PlayerCrouch();   // ������(�ɱ�)
        ScreenMove();     // ȭ��(ī�޶�) �̵�
        PlayerJump();     // �÷��̾� ����
        HandleFootsteps(); // �߼Ҹ� ó�� �߰�
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

    /// <summary>
    /// �߼Ҹ� ���� ó��.
    /// �÷��̾ ���鿡 �ְ�, ���� �ӵ��� ������ ��, ���� ���ݸ��� �߼Ҹ��� ����մϴ�.
    /// </summary>
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
}
