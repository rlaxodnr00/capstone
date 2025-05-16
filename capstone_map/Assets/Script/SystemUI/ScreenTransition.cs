using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.Hierarchy;

public class ScreenTransition : MonoBehaviour
{
    //�̱��� ����
    public static ScreenTransition Instance { get; private set; }

    [Header("UI References")]
    public Image transitionImage; // SceenTransitionDarkness
    public GameObject inputBlocker; // Raycast ���� ���� ������Ʈ

    [Header("Transition Settings")]
    public float fadeDuration = 1.3f; //ȭ�� ��ȯ �ð�

    private void Awake()
    {
        // �̱��� �ʱ�ȭ
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� �Ѿ�� ����(��� �ش� ��ü�� ���� ���)
        }
        else
        {
            Destroy(gameObject); //�ν��Ͻ� �ϳ��� ������ �� �ֵ���
        }
    }

    public void StartFadeOut(string sceneName) //Ÿ ��ũ��Ʈ������ ������ << ����
    {
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }

    public void StartFadeIn() //Ÿ ��ũ��Ʈ������ ������ << ����
    {
        StartCoroutine(FadeInAndLoadScene());
    }


    private IEnumerator FadeOutAndLoadScene(string sceneName) //�� ��ȯ �� ����� �޼ҵ� << ����
    {
        // ���콺 �Է� ���� ����
        inputBlocker.SetActive(true);

        float time = 0f;
        Color c = transitionImage.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            //Clamp01 : �־��� ���� 0 1 ������ ������ ����ִ� ���� ���
            float alpha = Mathf.Clamp01(time / fadeDuration); // time << �ð��� ������ ���� ������ Ŀ��, 0 ~> 1
            transitionImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        // �Է� ���� ����
        inputBlocker.SetActive(false);
        SceneManager.LoadScene(sceneName);
    }


    private IEnumerator FadeInAndLoadScene() // �� ��ȯ �� ����� �޼ҵ� << ����
    {
        float time = 0;
        Color c = transitionImage.color;

        while(time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeDuration - (time / fadeDuration)); //1���� �ð��� ������ ���� ������ ����, 1 ~> 0
            transitionImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
    }
}
