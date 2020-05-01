using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorScripts : MonoBehaviour
{


    private void OnCollisionEnter(Collision collision)
    {
        Collider co = collision.collider;
        if (!co.gameObject.GetComponent<Inventory>().hasKey)
        {
            this.GetComponent<Rigidbody>().freezeRotation = true;
        }
        else
        {
            this.GetComponent<Rigidbody>().freezeRotation = false;
        }


    }

}
