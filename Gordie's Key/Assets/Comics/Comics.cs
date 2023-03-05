using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Comics : MonoBehaviour
{
    public GameObject[] Pages;

    private int a=1;

    public int neededA;

    public string GoLevel;

    private void Start()
    {
        Pages[0].SetActive(true);
    }
    public void ButtonForNext()
    {
        if (a<Pages.Length)
        {
            Pages[a].SetActive(true);
            a += 1;
            if (a==neededA)
            {
                StartCoroutine(GoGame());
            }
        }
         
    }

    IEnumerator GoGame()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(GoLevel);
    }
    
}
