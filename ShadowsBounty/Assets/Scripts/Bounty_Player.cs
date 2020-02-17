using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounty_Player : MonoBehaviour
{

    public int bounty;

    // Start is called before the first frame update
    void Start()
    {
        bounty = PlayerPrefs.GetInt("score", 10);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(bounty);
    }

    // Upload current bounty to upstream
    void OnDisable()
    {
        PlayerPrefs.SetInt("score", bounty);
    }
}
