using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RankSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerScore_1;
    [SerializeField] TextMeshProUGUI playerScore_2;
    [SerializeField] TextMeshProUGUI playerScore_3;
    [SerializeField] TextMeshProUGUI playerScore_4;

    // Start is called before the first frame update
    void Start()
    {
        playerScore_1.text = PlayerPrefs.GetInt("score").ToString();
        playerScore_2.text = PlayerPrefs.GetInt("score").ToString();
        playerScore_3.text = PlayerPrefs.GetInt("score").ToString();
        playerScore_4.text = PlayerPrefs.GetInt("score").ToString();
    }

    public void BackMainMenu()
    {
        Debug.Log("end");
        SceneManager.LoadScene("MainMenu");
    }
}
