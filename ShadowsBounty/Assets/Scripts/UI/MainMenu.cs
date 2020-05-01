using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    DataJSON data;

    public void PlayGame()
    {
        data = new DataJSON(0);
        DataJSON.Save(data);
        //Debug.Log(data);
        //PlayerPrefs.SetInt("score", 100);
        PlayerPrefs.SetInt("round", 1);
        SceneManager.LoadScene("MultiplayerMenu");
    }

    private void Update()
    {
        if (!Cursor.visible || Cursor.lockState != CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        //Cursor.lockState = CursorLockMode.Confined;
        
        //Debug.Log(Cursor.lockState + " " + Cursor.visible);
    }

    public void QuiteGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

}
