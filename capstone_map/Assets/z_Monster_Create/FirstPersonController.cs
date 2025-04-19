using UnityEngine;

public class First_PersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float verticalLookLimit = 80f;

    //private CharacterController characterController;
    private float verticalRotation = 0f;
    private Transform cameraTransform;

    void Start()
    {
        //characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        // ���콺 Ŀ�� ����� ����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        // ���콺 �Է�
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // ī�޶� ���� ȸ��
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // �÷��̾� �¿� ȸ��
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");  // A/D
        float moveZ = Input.GetAxis("Vertical");    // W/S

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        //characterController.Move(move * moveSpeed * Time.deltaTime);
    }
}
