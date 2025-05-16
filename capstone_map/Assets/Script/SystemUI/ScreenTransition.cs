using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.Hierarchy;

public class ScreenTransition : MonoBehaviour
{
    //싱글톤 패턴
    public static ScreenTransition Instance { get; private set; }

    [Header("UI References")]
    public Image transitionImage; // SceenTransitionDarkness
    public GameObject inputBlocker; // Raycast 막는 투명 오브젝트

    [Header("Transition Settings")]
    public float fadeDuration = 1.3f; //화면 전환 시간

    private void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 넘어가도 유지(계속 해당 객체가 연출 담당)
        }
        else
        {
            Destroy(gameObject); //인스턴스 하나만 존재할 수 있도록
        }
    }

    public void StartFadeOut(string sceneName) //타 스크립트에서의 참조용 << 암전
    {
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }

    public void StartFadeIn() //타 스크립트에서의 참조용 << 명전
    {
        StartCoroutine(FadeInAndLoadScene());
    }


    private IEnumerator FadeOutAndLoadScene(string sceneName) //씬 전환 시 사용할 메소드 << 암전
    {
        // 마우스 입력 차단 시작
        inputBlocker.SetActive(true);

        float time = 0f;
        Color c = transitionImage.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            //Clamp01 : 주어진 값을 0 1 범위의 값으로 집어넣는 데에 사용
            float alpha = Mathf.Clamp01(time / fadeDuration); // time << 시간이 지남에 따라 서서히 커짐, 0 ~> 1
            transitionImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        // 입력 차단 해제
        inputBlocker.SetActive(false);
        SceneManager.LoadScene(sceneName);
    }


    private IEnumerator FadeInAndLoadScene() // 씬 전환 후 사용할 메소드 << 명전
    {
        float time = 0;
        Color c = transitionImage.color;

        while(time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeDuration - (time / fadeDuration)); //1에서 시간이 지남에 따라 서서히 감소, 1 ~> 0
            transitionImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
    }
}
