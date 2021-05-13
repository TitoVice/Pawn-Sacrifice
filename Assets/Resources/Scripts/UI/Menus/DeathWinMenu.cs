using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathWinMenu : MonoBehaviour
{
    public GameObject deadText;
    public GameObject winText;
    
    void OnEnable()
    {
        Time.timeScale = 0.0f;
    }

        public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Death()
    {
        deadText.SetActive(true);
    }
    public void Win()
    {
        winText.SetActive(true);
    }
}
