using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureMaster : MonoBehaviour
{
    public int value = 100;

    //HUD hud;

    private void Start()
    {
      //  hud = GameObject.Find("HUD").GetComponent<HUD>();
    }

    public int getValue()
    {
        return value;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            HUD hud = other.gameObject.transform.GetChild(5).GetComponent<HUD>();
            hud.AddScore(value);
            //other.gameObject.GetComponent<TreasureCollider>().addScore(value);
            //Debug.Log(other.gameObject.GetComponent<TreasureCollider>().getScore());

        }
    }
}
