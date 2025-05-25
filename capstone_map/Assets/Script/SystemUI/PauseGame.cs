using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public enum PauseState
    {
        None,       // 게임 진행 중
        Menu,       // 일시정지 메뉴 열림
        Settings    // 설정창 열림
    }


    [Header("menu")]
    public GameObject menu; 
    public GameObject resume;
    public GameObject setting;
    public GameObject exit;

    [Header("detail")]
    public GameObject settingScreen;

    private PauseState currentState = PauseState.None;

    void Start()
    {
        menu.SetActive(false);
        settingScreen.SetActive(false);
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            switch (currentState)
            {
                case PauseState.None:
                    OpenPauseMenu();
                    break;

                case PauseState.Menu:
                    Resume(); // 다시 ESC → 게임 복귀
                    break;

                case PauseState.Settings:
                    CloseSettings(); // 설정창에서 ESC → 메뉴로 돌아가기
                    break;
            }
        }
    }

    void OpenPauseMenu()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
        settingScreen.SetActive(false);
        currentState = PauseState.Menu;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        menu.SetActive(false);
        settingScreen.SetActive(false);
        currentState = PauseState.None;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Setting()
    {
        menu.SetActive(false);
        settingScreen.SetActive(true);
        currentState = PauseState.Settings;
    }

    void CloseSettings()
    {
        settingScreen.SetActive(false);
        menu.SetActive(true);
        currentState = PauseState.Menu;
    }

    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}