using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        PlayerPrefs.SetInt("score", 100);
        SceneManager.LoadScene("ShoppingMenu");
    }

    public void QuiteGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

}
