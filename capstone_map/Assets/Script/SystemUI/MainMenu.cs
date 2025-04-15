using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [Header("Main Button")]
    public GameObject gameStart; //start 버튼 연결
    public GameObject gameGuide; //guide 버튼 연결
    public GameObject gameOption; //option 버튼 연결
    public GameObject gameQuit; //quit 버튼 연결

    [Header("Guide Screens")]
    public CanvasGroup guideScreens; //가이드 스크린 부모
    public CanvasGroup guideScreen;
    public CanvasGroup playGuideScreen;
    public CanvasGroup keyGuideScreen;


    [Header("Others")]
    private CanvasGroup currentScreen; //화면저장용


    private void Start()
    {
        DefaultMainScreen();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //ESC 키로 뒤로가기
        {
            GoBack();
            Debug.Log("GoBack 실행");
        }
    }

    public void DefaultMainScreen()
    {
        SetCanvasGroup(guideScreens, false);
        SetCanvasGroup(guideScreen, true);
        SetCanvasGroup(playGuideScreen, false);
        SetCanvasGroup(keyGuideScreen, false);
        currentScreen = null;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void GuideGame() //가이드 오픈
    {
        SetCanvasGroup(guideScreens, true); //가이드 화면 활성화
        SetCanvasGroup(guideScreen, true);
        SetCanvasGroup(playGuideScreen, false);
        SetCanvasGroup(keyGuideScreen, false);
        currentScreen = guideScreen;  // 현재 화면 = 가이드 화면으로 설정
        Debug.Log("GuideGame currentScreen : " + currentScreen);
    }

    public void OpenKeyGuide() //키 가이드 오픈
    {
        SetCanvasGroup(guideScreens, false);
        SetCanvasGroup(guideScreen, false);
        SetCanvasGroup(keyGuideScreen, true);
        currentScreen = keyGuideScreen;  // 현재 화면 =  키 가이드 화면으로 설정
        Debug.Log("OpenKeyGuide currentScreen : " + currentScreen);
    }

    public void OpenPlayGuide() //플레이 가이드 오픈
    {
        SetCanvasGroup(guideScreens, false);
        SetCanvasGroup(playGuideScreen, true);
        SetCanvasGroup(guideScreen, false);
        currentScreen = playGuideScreen; // 현재 화면 = 플레이 가이드 화면으로 설정
        Debug.Log("OpenPlayGuide currentScreen : " + currentScreen);
    }


    public void OptionGame()
    {
        Debug.Log("설정창 오픈");
    }

    public void QuitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();


        // 유니티 에디터에서는 종료되지 않으므로, 아래 코드 추가 가능 (에디터에서만 실행)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }


    public void GoBack()
    {
        //플레이 가이드 or 키 가이드의 경우 가이드 화면으로 돌아감
        if (currentScreen == playGuideScreen || currentScreen == keyGuideScreen)
        {
            GuideGame();
            /*
            SetCanvasGroup(playGuideScreen, false);
            SetCanvasGroup(keyGuideScreen, false);
            SetCanvasGroup(guideScreen, true);
            currentScreen = guideScreen;*/
            Debug.Log("GoBack if currentScreen : " + currentScreen);
        }


        //가이드 화면에서 뒤로 갈 시
        else if (currentScreen == guideScreen)
        {
            DefaultMainScreen();
            Debug.Log("GoBack elif currentScreen : " + currentScreen);
        }
    }

    private void SetCanvasGroup(CanvasGroup canvasGroup, bool isActive)
    {
        if (canvasGroup == null) return;
        canvasGroup.alpha = isActive ? 1 : 0; //투명도 조절
        canvasGroup.interactable = isActive; //상호작용 on / off
        canvasGroup.blocksRaycasts = isActive; //raycast 활성 / 비활성화
    }



   
}
