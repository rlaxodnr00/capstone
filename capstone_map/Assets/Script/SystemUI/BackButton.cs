using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    private MainMenu mainMenu;
    private PauseGame pauseGame;

    void Start()
    {
        // 각각의 매니저를 찾아 연결
        mainMenu = Object.FindFirstObjectByType<MainMenu>();
        pauseGame = Object.FindFirstObjectByType<PauseGame>();

        GetComponent<Button>().onClick.AddListener(OnBackButtonClicked);
    }

    private void OnBackButtonClicked()
    {
        if (mainMenu != null)
        {
            Debug.LogWarning("mainMenu 실행");
            mainMenu.GoBack(); // 메인 메뉴에서 ESC 기능
        }
        else if (pauseGame != null)
        {
            Debug.LogWarning("pause 실행");
            pauseGame.OnPauseAction(); // 인게임 일시정지에서 ESC 기능
        }
    }
}
