using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameOverTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;         // UI�� ����� Text ������Ʈ
    public float startTimeSet = 300f; // 5�� (�� ����)

    private float timeRemaining;
    private bool isRunning = true;

    void Start()
    {
        timeRemaining = startTimeSet;
    }

    void Update()
    {
        if (isRunning)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isRunning = false;
                OnTimerEnd();
            }

            UpdateTimerUI();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void OnTimerEnd()
    {
        Debug.Log("Ÿ�̸� ����, ���� ���� ��� ����");
        // ���⿡ ���� �̺�Ʈ ȣ�� �Ǵ� GameManager ȣ��
        // ��: GameManager.Instance.GameOver();
    }
}
