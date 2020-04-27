using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RankSystem : Bolt.GlobalEventListener
{
    [SerializeField] TextMeshProUGUI playerScore_1;
    [SerializeField] TextMeshProUGUI playerScore_2;
    [SerializeField] TextMeshProUGUI playerScore_3;
    [SerializeField] TextMeshProUGUI playerScore_4;

    public Image thief;
    public Image royal;
    public Image magic;
    public Image mafia;
    private Image[] images;
    public Image score1;
    public Image score2;
    public Image score3;
    public Image score4;

    DataJSON data;

    // Start is called before the first frame update
    void Start()
    {
        data = DataJSON.Load();
        playerScore_1.text = data.bounty.ToString();
        playerScore_2.text = PlayerPrefs.GetInt("score").ToString();
        playerScore_3.text = PlayerPrefs.GetInt("score").ToString();
        playerScore_4.text = PlayerPrefs.GetInt("score").ToString();
        images = new Image[4];
        images[0] = mafia;
        images[1] = thief;
        images[2] = royal;
        images[3] = magic;

    }




    private void Update()
    {
        float firstScore =-1;
        float firstEmblem = -1;
        float secondScore = -1;
        float secondEmblem = -1;
        float thirdScore = -1;
        float thirdEmblem = -1;
        float forthScore = -1;
        float forthEmblem = -1;
        foreach (BoltEntity ent in BoltNetwork.Entities)
        {

            IPlayerMoveState state;
            if (ent.TryFindState<IPlayerMoveState>(out state))
            {
                float tempTeam = state.TeamNumber;
                float tempScore = state.Score;
                
                if(tempScore > firstScore)
                {
                    forthScore = thirdScore;
                    forthEmblem = thirdEmblem;
                    thirdScore = secondScore;
                    thirdEmblem = secondEmblem;
                    secondScore = firstScore;
                    secondEmblem = firstEmblem;
                    firstScore = tempScore;
                    firstEmblem = tempTeam;
                }
            }
        }

        playerScore_1.text = firstScore.ToString();
        playerScore_2.text = secondScore.ToString();
        playerScore_3.text = thirdScore.ToString();
        playerScore_4.text = forthScore.ToString();
        if (firstEmblem != -1)
        {
            score1.sprite = images[(int) firstEmblem - 1].sprite;
        }

        if (secondEmblem != -1)
        {
            score1.sprite = images[(int)firstEmblem - 1].sprite;
        }

        if (thirdEmblem != -1)
        {
            score1.sprite = images[(int)firstEmblem - 1].sprite;
        }

        if (forthEmblem != -1)
        {
            score1.sprite = images[(int)firstEmblem - 1].sprite;
        }


    }

    public void BackMainMenu()
    {
        Debug.Log("end");
        SceneManager.LoadScene("MainMenu");
    }
}
