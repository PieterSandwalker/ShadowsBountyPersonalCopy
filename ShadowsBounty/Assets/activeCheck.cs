using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activeCheck : MonoBehaviour
{
    public bool check;
    public bool check2;
    // Update is called once per frame
    void Update()
    {
        if(check)
        gameObject.SetActive(false);
        if (check2)
        gameObject.SetActive(true);
    }
}
