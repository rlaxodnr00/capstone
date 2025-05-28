using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform targetTransform; // X 이미지의 RectTransform
    public float scaleMultiplier = 1.2f;  // 확대할 비율
    public float animationSpeed = 8f;     // 애니메이션 속도

    private Vector3 originalScale;
    private bool isHovered = false; //마우스 커서와 상호작용 중인지

    private void Start()
    {
        originalScale = targetTransform.localScale;
    }

    private void Update()
    {
        // 마우스를 올리면 확대, 아니면 원래 크기로 복귀
        //Vector3 targetScale = isHovered ? originalScale * scaleMultiplier : originalScale;
        //targetTransform.localScale = Vector3.Lerp(targetTransform.localScale, targetScale, Time.deltaTime * animationSpeed);

        //게임이 일시정지(Time.timeScale = 0) 상태여도 동작하게 만듦
        Vector3 targetScale = isHovered ? originalScale * scaleMultiplier : originalScale;
        targetTransform.localScale = Vector3.Lerp(targetTransform.localScale, targetScale, Time.unscaledDeltaTime * animationSpeed);
    }

    //OnDisable : 오브젝트 비활성화 시 호출
    private void OnDisable() //UI 비활성화 시 애니메이션 영향으로 커진 상태 초기화
    {
        ResetScaleImmediately();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    public void ResetScaleImmediately() //스케일 복구 코드
    {
        targetTransform.localScale = originalScale;
        isHovered = false;
    }
}
