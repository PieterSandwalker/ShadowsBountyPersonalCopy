﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public int score;
    [SerializeField] public TextMeshProUGUI Bounty;
    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("InventorySystem").GetComponent<Inventory>();
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
        inventory.AddScore(point);
        Bounty.text = score.ToString();
    }
}