using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerScript2 : Bolt.EntityBehaviour<ITimerState>
{
    [SerializeField] TextMeshProUGUI CountDown_text;
    public Canvas scoreboard;
    public float roundTime;
    bool canCount = true;
    bool done = false;
    int roundNumber;
    string roundString;


    public override void Attached()
    {
        state.SecondsLeft = roundTime;
        state.Round = PlayerPrefs.GetInt("round");
    }

    // Start is called before the first frame update
    void Start()
    {
        state.SecondsLeft = roundTime;
        CountDown_text.text = ConvertTime();
        roundNumber = PlayerPrefs.GetInt("round");
        roundString = "Round" + state.Round.ToString() + " ";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) RoundOver(); //Force scene transition; makes level testing easier

        if (state.SecondsLeft >= 0.0f && canCount)
        {
            state.SecondsLeft -= Time.deltaTime;
            CountDown_text.text = ConvertTime();
        }
        else if (state.SecondsLeft <= 0.0f && !done)
        {
            canCount = false;
            done = true;
            CountDown_text.text = roundString + "0:00";
            state.SecondsLeft = 0.0f;
        }
        else if (done)
        {
            RoundOver();
        }

    }

    private string ConvertTime()
    {
        int minute = (int)state.SecondsLeft / 60;
        int second = (int)state.SecondsLeft % 60;
        if (second < 10)
        {
            return roundString + minute.ToString() + ":0" + second.ToString();
        }
        return roundString + minute.ToString() + ":" + second.ToString();
    }

    private void RoundOver()
    {
        state.Round++;
        roundNumber++;
        scoreboard.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        //Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        PlayerPrefs.SetInt("round", roundNumber);
        
    }


}
