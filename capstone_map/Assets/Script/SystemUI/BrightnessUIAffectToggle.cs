using UnityEngine;
using UnityEngine.UI;

public class BrightnessUIAffectToggle : MonoBehaviour
{
    public Toggle affectUIToggle; // UI 토글 컴포넌트
    public Canvas brightnessOverlayCanvas; // 밝기 오버레이가 포함된 캔버스 (이 오브젝트의 sortingOrder를 조정함)

    // UI 포함 여부에 따라 설정할 sortingOrder 값(UI 캔버스 값 : -6)
    private const int orderWhenAffectUI = -4;   // UI와 함께 밝기 적용
    private const int orderWhenIgnoreUI = -8;   // UI 위에 밝기 적용 방지

    // PlayerPrefs 키 이름 (설정 저장용)
    private const string PREF_KEY = "AffectUIWithBrightness";

    private void Start()
    {
        //인게임에서 인스턴스로 생성한 밝기 화면을 스크립트에 할당하기 위한 코드
        if (brightnessOverlayCanvas == null && GameSettingsManager.Instance != null)
        {
            //GameSettingsManager가 갖고 있는 밝기 이미지를 가져옴
            var brightnessOverlayImage = GameSettingsManager.Instance.brightnessOverlay;

            //만약 이미지가 null이 아니라면
            if (brightnessOverlayImage != null)
            {
                //이미지의 부모 오브젝트들 중에서 Canvas컴포넌트를 가진 첫 번째 객체를 찾음
                brightnessOverlayCanvas = brightnessOverlayImage.GetComponentInParent<Canvas>();
            }
        }

        // 저장된 설정값을 불러옴 (기본값: 0 → false)
        bool affectUI = PlayerPrefs.GetInt(PREF_KEY, 0) == 1;

        // 토글 UI에 반영
        affectUIToggle.isOn = affectUI;

        // 밝기 오버레이의 sortingOrder 설정
        UpdateSortingOrder(affectUI);

        // 토글 상태가 바뀌면 처리할 리스너 등록
        affectUIToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    // 토글 상태가 변경되었을 때 호출
    void OnToggleChanged(bool isOn)
    {
        // 설정값 저장 (1: true, 0: false)
        PlayerPrefs.SetInt(PREF_KEY, isOn ? 1 : 0);

        // 밝기 오버레이 캔버스의 정렬 순서 업데이트
        UpdateSortingOrder(isOn);
    }

    // 오버레이 캔버스의 정렬 순서를 실제로 변경하는 함수
    void UpdateSortingOrder(bool affectUI)
    {
        if (brightnessOverlayCanvas != null)
        {
            // 체크 시: UI 포함 (높은 순서)
            // 해제 시: UI 미포함 (낮은 순서)
            brightnessOverlayCanvas.sortingOrder = affectUI ? orderWhenAffectUI : orderWhenIgnoreUI;
        }
    }
}
