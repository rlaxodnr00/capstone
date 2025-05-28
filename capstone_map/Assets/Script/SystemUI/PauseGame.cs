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
            OnPauseAction();
        }
    }

    void OpenPauseMenu()
    {
        Debug.LogWarning($"currentState = {currentState}, OpenPauseMenu() 호출");
        Time.timeScale = 0;
        menu.SetActive(true);
        settingScreen.SetActive(false);
        currentState = PauseState.Menu;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        Debug.LogWarning($"currentState = {currentState}, Resume() 호출");
        Time.timeScale = 1;
        menu.SetActive(false);
        settingScreen.SetActive(false);
        currentState = PauseState.None;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Setting() //설정창 열기
    {
        Debug.LogWarning($"currentState = {currentState}, Setting() 호출");
        menu.SetActive(false);
        settingScreen.SetActive(true);
        currentState = PauseState.Settings;
    }

    public void CloseSettings() //설정창 닫기
    {
        Debug.LogWarning($"currentState = {currentState}, CloseSettings() 호출");
        settingScreen.SetActive(false);
        menu.SetActive(true);
        currentState = PauseState.Menu;

        // 버튼 초기화 시도
        foreach (UIButtonHoverEffect hover in menu.GetComponentsInChildren<UIButtonHoverEffect>(true))
        {
            hover.ResetScaleImmediately();
        }
    }

    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void OnPauseAction() //ESC 눌렸을 때 위치에 따른 반응
    {
        switch (currentState)
        {
            case PauseState.None:
                OpenPauseMenu();
                break;

            case PauseState.Menu:
                Resume();
                break;

            case PauseState.Settings:
                CloseSettings();
                break;
        }
    }
}