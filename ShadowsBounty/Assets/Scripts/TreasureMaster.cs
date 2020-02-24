using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureMaster : MonoBehaviour
{
    public int value = 100;

    public int getValue()
    {
        return value;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            other.gameObject.GetComponent<TreasureCollider>().addScore(value);
        //    Debug.Log(other.gameObject.GetComponent<TreasureCollider>().getScore());

        }
    }
}
