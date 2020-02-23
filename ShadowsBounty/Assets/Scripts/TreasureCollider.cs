using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class TreasureCollider : MonoBehaviour
{
    int score = 0;
    [SerializeField] TextMeshProUGUI Bounty;

    private void Start()
    {
        score = PlayerPrefs.GetInt("score");
        Bounty.text = score.ToString();
    }

/*    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Treasure"))
        {
            other.gameObject.SetActive(false);
            score += other.gameObject.GetComponent<TreasureMaster>().value;
            PlayerPrefs.SetInt("score", score);
            Debug.Log("Score: " + score);
            Bounty.text = score.ToString();
        }
*//*         else if (other.gameObject.CompareTag("Key"))
        {
            Animmanager.doorKey = true;
            Destroy(other.gameObject);
        }*//*
    }*/

    private void OnDisable()
    {
        PlayerPrefs.SetInt("score", score);
    }

    public void addScore(int point)
    {
        score += point;
        Bounty.text = score.ToString();
    }

/*    public int getScore()
    {
        return score;
    }
*/

}
