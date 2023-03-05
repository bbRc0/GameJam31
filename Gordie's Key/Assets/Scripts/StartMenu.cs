using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
   public void Quit()
    {
        Application.Quit();
    }
   public void MainMenu()
    {
        SceneManager.LoadScene("SampleScene2");
    }
    public void ReTryLevel2()
    {
        SceneManager.LoadScene("Level2");
    }
    public void ReTryLevel3()
    {
        SceneManager.LoadScene("Deneme2");
    }
    public void ReTryLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
}
