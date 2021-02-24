using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public delegate void ClickButton();
    public static event ClickButton clickedButton;

    public void StartGame() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        clickedButton();
    }
    void Resume() { clickedButton(); }
    void Pause() { clickedButton(); }
    void LoadMenu() { clickedButton(); }

    void QuitGame() { clickedButton(); }






}
