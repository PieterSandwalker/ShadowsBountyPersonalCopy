using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
    public bool inTrigger;
     void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            inTrigger = true;
        } else
        {
            inTrigger = false;
        }

    }

     void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }

    private void Update()
    {
       if(inTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Animmanager.doorKey = true;
                Destroy(this.gameObject);
            }
        }
    }

    private void OnGUI()
    {
        if (inTrigger)
            GUI.Box(new Rect(0, 60, 200, 25), "Press E to take Key");
    }




}
