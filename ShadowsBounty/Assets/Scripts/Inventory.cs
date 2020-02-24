using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    public bool hasKey;
    void Start()
    {
        hasKey = false;
    }

    public void getKey()
    {
        hasKey = true;

    }

    
}
