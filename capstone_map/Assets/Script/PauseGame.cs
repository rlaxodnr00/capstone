using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject menu; 
    public GameObject resume;
    public GameObject exit;

    public bool on;
    public bool off;

    void Start()
    {
        menu.SetActive(false);
        off = true;
        on = false;
    }

    void Update()
    {
        if (off && Input.GetButtonDown("Pause"))
        {
            Time.timeScale = 0;
            menu.SetActive(true);
            off = false;
            on = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }

        else if (on && Input.GetButtonDown("Pause"))
        {
            Time.timeScale = 1;
            menu.SetActive(false);
            off = true;
            on = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void Resume()
    {
        menu.SetActive(false);
        off = true;
        on = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1;
    }

    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
