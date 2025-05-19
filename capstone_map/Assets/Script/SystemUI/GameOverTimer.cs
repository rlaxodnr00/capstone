using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameOverTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;         // UI에 출력할 Text 오브젝트
    public float startTimeSet = 300f; // 5분 (초 단위)

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
        Debug.Log("타이머 종료, 추후 구현 기능 실행");
        // 여기에 종료 이벤트 호출 또는 GameManager 호출
        // 예: GameManager.Instance.GameOver();
    }
}
