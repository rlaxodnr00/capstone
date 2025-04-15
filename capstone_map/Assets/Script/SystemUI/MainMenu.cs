using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [Header("Main Button")]
    public GameObject gameStart; //start ��ư ����
    public GameObject gameGuide; //guide ��ư ����
    public GameObject gameOption; //option ��ư ����
    public GameObject gameQuit; //quit ��ư ����

    [Header("Guide Screens")]
    public CanvasGroup guideScreens; //���̵� ��ũ�� �θ�
    public CanvasGroup guideScreen;
    public CanvasGroup playGuideScreen;
    public CanvasGroup keyGuideScreen;


    [Header("Others")]
    private CanvasGroup currentScreen; //ȭ�������


    private void Start()
    {
        DefaultMainScreen();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //ESC Ű�� �ڷΰ���
        {
            GoBack();
            Debug.Log("GoBack ����");
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

    public void GuideGame() //���̵� ����
    {
        SetCanvasGroup(guideScreens, true); //���̵� ȭ�� Ȱ��ȭ
        SetCanvasGroup(guideScreen, true);
        SetCanvasGroup(playGuideScreen, false);
        SetCanvasGroup(keyGuideScreen, false);
        currentScreen = guideScreen;  // ���� ȭ�� = ���̵� ȭ������ ����
        Debug.Log("GuideGame currentScreen : " + currentScreen);
    }

    public void OpenKeyGuide() //Ű ���̵� ����
    {
        SetCanvasGroup(guideScreens, false);
        SetCanvasGroup(guideScreen, false);
        SetCanvasGroup(keyGuideScreen, true);
        currentScreen = keyGuideScreen;  // ���� ȭ�� =  Ű ���̵� ȭ������ ����
        Debug.Log("OpenKeyGuide currentScreen : " + currentScreen);
    }

    public void OpenPlayGuide() //�÷��� ���̵� ����
    {
        SetCanvasGroup(guideScreens, false);
        SetCanvasGroup(playGuideScreen, true);
        SetCanvasGroup(guideScreen, false);
        currentScreen = playGuideScreen; // ���� ȭ�� = �÷��� ���̵� ȭ������ ����
        Debug.Log("OpenPlayGuide currentScreen : " + currentScreen);
    }


    public void OptionGame()
    {
        Debug.Log("����â ����");
    }

    public void QuitGame()
    {
        Debug.Log("���� ����");
        Application.Quit();


        // ����Ƽ �����Ϳ����� ������� �����Ƿ�, �Ʒ� �ڵ� �߰� ���� (�����Ϳ����� ����)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }


    public void GoBack()
    {
        //�÷��� ���̵� or Ű ���̵��� ��� ���̵� ȭ������ ���ư�
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


        //���̵� ȭ�鿡�� �ڷ� �� ��
        else if (currentScreen == guideScreen)
        {
            DefaultMainScreen();
            Debug.Log("GoBack elif currentScreen : " + currentScreen);
        }
    }

    private void SetCanvasGroup(CanvasGroup canvasGroup, bool isActive)
    {
        if (canvasGroup == null) return;
        canvasGroup.alpha = isActive ? 1 : 0; //���� ����
        canvasGroup.interactable = isActive; //��ȣ�ۿ� on / off
        canvasGroup.blocksRaycasts = isActive; //raycast Ȱ�� / ��Ȱ��ȭ
    }



   
}
