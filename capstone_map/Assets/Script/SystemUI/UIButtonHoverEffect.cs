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
        Vector3 targetScale = isHovered ? originalScale * scaleMultiplier : originalScale;
        targetTransform.localScale = Vector3.Lerp(targetTransform.localScale, targetScale, Time.deltaTime * animationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}
