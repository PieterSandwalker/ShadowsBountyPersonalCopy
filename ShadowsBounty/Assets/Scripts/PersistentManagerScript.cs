using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentManagerScript : MonoBehaviour
{
    public static PersistentManagerScript Instance { get; private set; }

    public int Bounty = 0;

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
    }

    public bool CostBounty(int costBounty) //(int itemIndex)
    {
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
}
