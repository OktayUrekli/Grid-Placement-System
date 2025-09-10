using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseController : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;

    void Start()
    {
        pausePanel.SetActive(false);    
    }

    public void ContinueButton()
    {
        Time.timeScale = 1.0f;
        pausePanel.SetActive(false);
    }

    public void PauseButton()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

}
