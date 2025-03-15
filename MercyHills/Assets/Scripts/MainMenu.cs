using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool hasPlayed = false;

   public void PlayGameDemo()
    {
        SceneManager.LoadScene("Loading_Scene");
        hasPlayed = true;
        FindObjectOfType<AudioManager>().gameObject.SetActive(true);
    }

    public void PlayGameFinal()
    {
        //SceneManager.LoadScene("Tasks");
    }

    public void QuitGame()
    {
        Debug.Log("quit is working");
        Application.Quit();
    }
}
