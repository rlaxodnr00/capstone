using UnityEngine;

public class BaseDoor : Door
{
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public override void OnInteract()
    {
        // 문이 잠겼을 때 (마스크가 있는지 확인하는 코드 필요,
        // 문이 가까이 있어서 마스크 없이 탈출하는 경우 방지)
        if (animator.GetBool("locked") || "mask" == "true")
        {
            // 

            // 문을 연다.
            animator.SetBool("locked", false);
        }
    }
}
