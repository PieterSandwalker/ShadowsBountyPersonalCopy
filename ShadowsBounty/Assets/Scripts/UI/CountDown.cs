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
    int roundNumber;
    string roundString;
    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("InventorySystem").GetComponent<Inventory>();
        timer = roundTime;
        CountDown_text.text = ConvertTime();
        roundNumber = PlayerPrefs.GetInt("round");
        roundString = "Round" + roundNumber.ToString() + " ";
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
            CountDown_text.text = roundString + "0:00";
            timer = 0.0f;
        } else if (done)
        {
            RoundOver();
        }
    }

    private string ConvertTime()
    {
        int minute = (int)timer / 60;
        int second = (int)timer % 60;
        if (second < 10)
        {
            return roundString + minute.ToString() + ":0" + second.ToString();
        }
        return roundString + minute.ToString() + ":" + second.ToString();
    }

    private void RoundOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //PlayerPrefs.SetInt("score", score);
        //Debug.Log(PlayerPrefs.GetInt("score"));
        roundNumber++;
        if (roundNumber > 3)
        {
            SceneOver(false);
        } 
        else
        {
            PlayerPrefs.SetInt("round", roundNumber);
            SceneOver(true);
        }
    }

    private void SceneOver(bool isShopping)
    {
        inventory.SceneOver();
        if (isShopping)
        {
            SceneManager.LoadScene("ShoppingMenu");
        } else
        {
            SceneManager.LoadScene("GameOverMenu");
        }
    }
        
}
