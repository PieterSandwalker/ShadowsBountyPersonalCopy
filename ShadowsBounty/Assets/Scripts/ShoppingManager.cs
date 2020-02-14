﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ShoppingManager : MonoBehaviour
{
    PersistentManagerScript Instance;

    [SerializeField] TextMeshProUGUI M_Object;

    [SerializeField] TextMeshProUGUI Message_Object;

    const string OpeningTxt = "Buying some magic?";

    const string BuySuccessTxt = "Good Choose";

    const string BuyFailTxt = "I can count, don't fool me";

    // Start is called before the first frame update
    private void Start()
    {
        Instance = PersistentManagerScript.Instance;
        //BountyTxt.text = Instance.Bounty.ToString();
        M_Object.text = Instance.Bounty.ToString();
        Message_Object.text = OpeningTxt;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PersistentManagerScript.Instance.AddBounty(100);
            //BountyTxt.text = Instance.Bounty.ToString();
            M_Object.text = Instance.Bounty.ToString();
            Message_Object.text = OpeningTxt;
        }

    }

    public void NextGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Shopping(int cost)
    {
        if (Instance.CostBounty(cost))
        {
            M_Object.text = Instance.Bounty.ToString();
            Message_Object.text = BuySuccessTxt;
        } else
        {
            Message_Object.text = BuyFailTxt;
        }
    }


}
