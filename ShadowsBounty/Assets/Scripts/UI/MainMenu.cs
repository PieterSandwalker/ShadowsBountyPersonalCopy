using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    DataJSON data;


    public void PlayGame()
    {
        data = new DataJSON(500);
        DataJSON.Save(data);
        //Debug.Log(data);
        //PlayerPrefs.SetInt("score", 100);
        PlayerPrefs.SetInt("round", 1);
        SceneManager.LoadScene("MultiplayerMenu");
    }

    public void QuiteGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

}
