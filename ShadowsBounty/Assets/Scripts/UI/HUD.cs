using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public int score;
    [SerializeField] public TextMeshProUGUI Bounty;
    public GameObject inventoryObj;
    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = inventoryObj.GetComponent<Inventory>();
        score = inventory.GetScore();
        Bounty.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //score = inventory.GetScore();
        //Bounty.text = score.ToString();
    }

    public void AddScore(int point)
    {
        score += point;
        if (score < 0)
        {
            score = 0;
            inventory.setScore(0);
        }
        else
        {
            inventory.AddScore(point);
        }
        Bounty.text = score.ToString();
    }

    public void updateScore(int score)
    {
        this.score = score;
        Bounty.text = score.ToString();
    }
}
