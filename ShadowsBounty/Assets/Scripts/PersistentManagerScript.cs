using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentManagerScript : MonoBehaviour
{
    public static PersistentManagerScript Instance { get; private set; }

    public int Bounty;
    private void Start()
    {
        SetupBounty();
        Debug.Log("Start: " + Bounty);
    }
    private void Awake()
    {
        if ( Instance == null )
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void AddBounty(int addBounty)
    {
        Bounty += addBounty;
        Debug.Log("Current: " + Bounty);
        PlayerPrefs.SetInt("score", Bounty);
        Debug.Log(PlayerPrefs.GetInt("score"));
    }

    public bool CostBounty(int costBounty) //(int itemIndex)
    {
        PlayerPrefs.SetInt("score", Bounty);
        if ( costBounty > Bounty )
        {
            Debug.Log("not enough bounty");
            return false;
        } else
        {
            Bounty -= costBounty;
            return true;
        }
    }

    public void SetupBounty()
    {
        Bounty = PlayerPrefs.GetInt("score");
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("score", Bounty);
    }


}
