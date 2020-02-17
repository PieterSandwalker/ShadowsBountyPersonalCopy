using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TreasureCollider : MonoBehaviour
{
    int score = 0;

    private void Start()
    {
        score = PlayerPrefs.GetInt("score");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Treasure"))
        {
            other.gameObject.SetActive(false);
            score += other.gameObject.GetComponent<TreasureMaster>().value;
            Debug.Log("Score: " + score);
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerPrefs.SetInt("score", score);
            Debug.Log(PlayerPrefs.GetInt("score"));
            SceneManager.LoadScene("ShoppingMenu");
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("score", score);
    }
}
