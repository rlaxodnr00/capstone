using UnityEngine;

public class CrouchHandler : MonoBehaviour
{
    /* 해당 스크립트는 웅크리기 상태만 관리하며
     * 이동 속도 관련 코드는 UserMove 스크립트에서 다룹니다
    */

    private CharacterController controller;
    private Vector3 standingScale;
    private float standingHeight;
    private bool isCrouching = false;

    [Header("Crounch Stettings")]
    [SerializeField] private float crouchHeight = 0.5f; //웅크리기 크기
    [SerializeField] private float crouchScaleFactor = 0.5f; //스케일 조절 비율

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        standingScale = transform.localScale; // 원래 크기 저장
        standingHeight = controller.height; //캐릭터 높이 
    }

    public bool IsCrouching()
    {
        return isCrouching;
    }

    public void CrouchTransition()
    {
        isCrouching = !isCrouching;
        Debug.Log("현재 상태: " + isCrouching);
        if (isCrouching)
        {
            //웅크리기 동작
            transform.localScale = new Vector3(standingScale.x, standingScale.y * crouchScaleFactor, standingScale.z);
            controller.height = crouchHeight;
            
        }
        else
        {
            //원래 크기로 복구
            transform.localScale = standingScale;
            controller.height = standingHeight;
        }
    }
}
