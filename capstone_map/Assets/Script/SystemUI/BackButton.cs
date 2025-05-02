using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{

    private MainMenu mainMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 씬 내에서 MainMenu 스크립트 찾기 (여러 UI에서 사용 가능)
        mainMenu = FindObjectOfType<MainMenu>();

        // 현재 오브젝트의 Button 컴포넌트를 가져와서 클릭 이벤트 추가
        GetComponent<Button>().onClick.AddListener(OnBackButtonClicked);

    }


    private void OnBackButtonClicked()
    {
        if (mainMenu != null)
        {
            mainMenu.GoBack(); // ESC 키와 동일한 기능 수행
        }
    }

}
