using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MenuControlEndless : MonoBehaviour
{
    public struct Selected
    {
        public PoolObject gameObject;
        public Vector3 transform;
    }

    public static bool GameIsPaused = false;
    public GameObject plane1, plane2;
    public GameObject repeater1, repeater2;
    public List<PoolObject> obstacles;

    Selected selected;



    bool control = true;
    private float startTime;
    public float countDownTime;
    private int coin;

    private PlayerController playerController;
    public Action<String,String,String> ChangePanel;

    public static MenuControlEndless _instance;

    public void Start()
    {
        _instance = this;
        coin = 0;
        Time.timeScale = 0f;
        startTime = Time.realtimeSinceStartup;
        playerController = GameObject.FindObjectOfType<PlayerController>();
        playerController.OnPlayerEvent += Deneme;
        Starter();
    }

    private void Deneme(string obj,string data)
    {
        if(obj == "Die")
        {
            Time.timeScale = 0f;
            ChangePanel.Invoke(obj, GameObject.FindObjectOfType<Timer>().DieTimer().ToString(), coin.ToString());
        }
        else if(obj == "Finish")
        {
            Time.timeScale = 0f;
            ChangePanel.Invoke(obj, GameObject.FindObjectOfType<Timer>().DieTimer().ToString(), coin.ToString());
        }
        else if (obj == "Coin")
        {
            coin++;
            ChangePanel.Invoke(obj, "", coin.ToString());
        }
        else if (obj == "PowerUp"){
            ChangePanel.Invoke(obj, data, "");
        }       
        else if(obj == "Recycle")
        {
            PoolManager.Instance.NotUsedObjects();
            for (int i = 0; i < 10; i++)
            {
                selected = Chooser(i, float.Parse(data)) ;
                PoolManager.Instance.UseObject(selected.gameObject, selected.transform, Quaternion.identity);
            }

            int repeaterCount = (int)(float.Parse(data) / 100);
            if ( repeaterCount % 2 == 1)
            {
                plane1.transform.position = new Vector3(0f, 0f, repeaterCount * 100 + 150);
                repeater1.transform.position = new Vector3(0f, 0f, repeaterCount * 100 + 202);
            }
            else
            {
                plane2.transform.position = new Vector3(0f, 0f, repeaterCount * 100 + 150);
                repeater2.transform.position = new Vector3(0f, 0f, repeaterCount * 100 + 202);
            }

        }
    }

    void Update()
    {
        if ((Time.realtimeSinceStartup - startTime) <= countDownTime)
        {
            ChangePanel.Invoke("CountDown", (countDownTime - (Time.realtimeSinceStartup - startTime)).ToString("0").ToString(), "");
        }
        else if ((Time.realtimeSinceStartup - startTime) <= countDownTime+1)
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

    public void Starter() 
    {
        
        plane1.transform.position = new Vector3(0f, 0f, 50f);
        plane2.transform.position = new Vector3(0f, 0f, 150f);
        repeater1.transform.position = new Vector3(0f, 0f, 102f);
        repeater2.transform.position = new Vector3(0f, 0f, 202f);

        for (int i = 1; i < 20; i++)
        {
            selected = Chooser(i, -100f);
            PoolManager.Instance.UseObject(selected.gameObject, selected.transform, Quaternion.identity);
        }
    }

    public Selected Chooser(int order, float playerZ)
    {
        Selected selectedObject;
        int index = (int)UnityEngine.Random.Range(0f, 5.99f);
        int round = (int)(playerZ/100);
        
        switch (index)
        {
            case 0:
                selectedObject.transform = new Vector3(-2.5f, 0.5f, (round * 100 + 100 + order * 10));
                selectedObject.gameObject = obstacles[0];
                selectedObject.gameObject.poolid = selectedObject.gameObject.GetInstanceID();
                return selectedObject;

            case 1:
                selectedObject.transform = new Vector3(0f, 0.5f, round * 100 + 100 + order * 10);
                selectedObject.gameObject = obstacles[1];
                selectedObject.gameObject.poolid = selectedObject.gameObject.GetInstanceID();
                return selectedObject;
            
            case 2:
                selectedObject.transform = new Vector3(-2.5f, 0.22f, round * 100 + 100 + order * 10);
                selectedObject.gameObject = obstacles[2];
                selectedObject.gameObject.poolid = selectedObject.gameObject.GetInstanceID();
                return selectedObject;
                
            case 3:
                selectedObject.transform = new Vector3(0f, 0.2f, round * 100 + 100 + order * 10);
                selectedObject.gameObject = obstacles[3];
                selectedObject.gameObject.poolid = selectedObject.gameObject.GetInstanceID();
                return selectedObject;

            case 4:
                selectedObject.transform = new Vector3(0f, 0f, round * 100 + 100 + order * 10);
                selectedObject.gameObject = obstacles[4];
                selectedObject.gameObject.poolid = selectedObject.gameObject.GetInstanceID();
                return selectedObject;

            case 5:
                selectedObject.transform = new Vector3(UnityEngine.Random.Range(-4.5f, 4.5f), 0.5f, round * 100 + 100 + order * 10);
                selectedObject.gameObject = obstacles[5];
                return selectedObject;

            default:
                return new Selected();
        }
    }

    public void Resume()
    {
        ChangePanel.Invoke("Resume","","");
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void Pause()
    {
        ChangePanel.Invoke("Pause","","");
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
        ChangePanel.Invoke("StartGame","","");
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
