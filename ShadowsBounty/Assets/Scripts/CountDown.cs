using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CountDown : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CountDown_text;

    public float roundTime;
    //public float endbattleTime;
    float timer;
    bool canCount = true;
    bool done = false;


    // Start is called before the first frame update
    void Start()
    {
        timer = roundTime;
        CountDown_text.text = ConvertTime();
    }

    private void Update()
    {
        if (timer >= 0.0f && canCount)
        {
            timer -= Time.deltaTime;
            CountDown_text.text = ConvertTime();
        }
        else if (timer <= 0.0f && !done)
        {
            canCount = false;
            done = true;
            CountDown_text.text = "0:00";
            timer = 0.0f;
        } else if (done)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //PlayerPrefs.SetInt("score", score);
            Debug.Log(PlayerPrefs.GetInt("score"));
            SceneManager.LoadScene("ShoppingMenu");
        }
    }

    private string ConvertTime()
    {
        int minute = (int)timer / 60;
        int second = (int)timer % 60;
        if (second < 10)
        {
            return minute.ToString() + ":0" + second.ToString();
        }
        return minute.ToString() + ":" + second.ToString();
    }
}
