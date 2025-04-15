using UnityEngine;

public class CrouchHandler : MonoBehaviour
{
    /* �ش� ��ũ��Ʈ�� ��ũ���� ���¸� �����ϸ�
     * �̵� �ӵ� ���� �ڵ�� UserMove ��ũ��Ʈ���� �ٷ�ϴ�
    */

    private CharacterController controller;
    private Vector3 standingScale;
    private float standingHeight;
    private bool isCrouching = false;

    [Header("Crounch Stettings")]
    [SerializeField] private float crouchHeight = 0.5f; //��ũ���� ũ��
    [SerializeField] private float crouchScaleFactor = 0.5f; //������ ���� ����

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        standingScale = transform.localScale; // ���� ũ�� ����
        standingHeight = controller.height; //ĳ���� ���� 
    }

    public bool IsCrouching()
    {
        return isCrouching;
    }

    public void CrouchTransition()
    {
        isCrouching = !isCrouching;
        Debug.Log("���� ����: " + isCrouching);
        if (isCrouching)
        {
            //��ũ���� ����
            transform.localScale = new Vector3(standingScale.x, standingScale.y * crouchScaleFactor, standingScale.z);
            controller.height = crouchHeight;
            
        }
        else
        {
            //���� ũ��� ����
            transform.localScale = standingScale;
            controller.height = standingHeight;
        }
    }
}
