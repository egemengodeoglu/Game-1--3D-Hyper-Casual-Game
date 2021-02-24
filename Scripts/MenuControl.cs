using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    public static bool GameIsPaused = false;

    bool control = true;
    private float startTime;
    public float countDownTime;
    private int coin;

    private PlayerController playerController;
    public Action<String, String, String> ChangePanel;

    public void Start()
    {
        coin = 0;
        Time.timeScale = 0f;
        startTime = Time.realtimeSinceStartup;
        playerController = GameObject.FindObjectOfType<PlayerController>();
        playerController.OnPlayerEvent += OnPlayerEvent;
    }

    private void OnPlayerEvent(string obj, string data)
    {
        if (obj == "Die")
        {
            Time.timeScale = 0f;
            ChangePanel.Invoke(obj, GameObject.FindObjectOfType<Timer>().DieTimer().ToString(), coin.ToString());
        }
        else if (obj == "Finish")
        {
            Time.timeScale = 0f;
            ChangePanel.Invoke(obj, GameObject.FindObjectOfType<Timer>().DieTimer().ToString(), coin.ToString());
        }
        else if (obj == "Coin")
        {
            coin++;
            ChangePanel.Invoke(obj, "", coin.ToString());
        }
        else if (obj == "PowerUp")
        {
            ChangePanel.Invoke(obj, data, "");
        }
    }

    void Update()
    {
        if ((Time.realtimeSinceStartup - startTime) <= countDownTime)
        {
            ChangePanel.Invoke("CountDown", (countDownTime - (Time.realtimeSinceStartup - startTime)).ToString("0").ToString(), "");
        }
        else if ((Time.realtimeSinceStartup - startTime) <= countDownTime + 1)
        {
            ChangePanel.Invoke("CountDown", "GO!", "");
        }
        else if (control)
        {
            Time.timeScale = 1f;
            ChangePanel.Invoke("StartGame", "", "");
            control = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    
    public void Resume()
    {
        ChangePanel.Invoke("Resume", "", "");
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void Pause()
    {
        ChangePanel.Invoke("Pause", "", "");
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Retry()
    {
        ChangePanel.Invoke("Retry", "", "");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        ChangePanel.Invoke("StartGame", "", "");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quiting Game...");
    }
}
