using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenuUI;
    public bool isPaused;

    [SerializeField] GameObject taskList;
    [SerializeField] GameObject mapList;
    
    // Start is called before the first frame update
    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetButtonDown("PauseButton")))
        {
            if (isPaused)
            {
                ResumeGame();
                taskList.SetActive(true);
                mapList.SetActive(true);
            }
            else if(!isPaused)
            {
                PauseGame();
                taskList.SetActive(false);
                mapList.SetActive(false);
            }
        }

        //Debug.Log(Input.GetAxis("DPad"));

        

    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Main_Menu");
        Time.timeScale = 1f;
       // SceneManager.Load("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Qutting game...");
        Application.Quit();
    }
}
