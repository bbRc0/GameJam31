using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{

    public static bool gameIsPaused;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject panel;
    public AudioSource music;
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(gameIsPaused)
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
        pauseMenu.SetActive(false);
        panel.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        panel.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    public void LoadScene()
    {
        SceneManager.LoadScene("SampleScene2");
    }
    public void Settings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
        gameIsPaused = true;
    }
    public void Fullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void SetMusic(bool isMusic)
    {
        music.mute = !isMusic;
    }
}
