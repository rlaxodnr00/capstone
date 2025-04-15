using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform targetTransform; // X �̹����� RectTransform
    public float scaleMultiplier = 1.2f;  // Ȯ���� ����
    public float animationSpeed = 8f;     // �ִϸ��̼� �ӵ�

    private Vector3 originalScale;
    private bool isHovered = false; //���콺 Ŀ���� ��ȣ�ۿ� ������

    private void Start()
    {
        originalScale = targetTransform.localScale;
    }

    private void Update()
    {
        // ���콺�� �ø��� Ȯ��, �ƴϸ� ���� ũ��� ����
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
