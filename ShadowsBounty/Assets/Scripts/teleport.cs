using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    Vector3 pos;
    // Update is called once per frame

    Camera cam;

    bool ready;

    void Start()
    {
        ready = false;
        cam = GetComponent<Camera>();
    }
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.T) || Input.GetKey(KeyCode.T) || ready )
        {
            //   RaycastHit hit;
            ready = true;
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) { 
                print("I'm looking at " + hit.transform.name);
                if (hit.transform.name == "TeleportTarget")
                {
                }
                else
                {
                    pos = hit.point;
                    GameObject tg = GameObject.Find("TeleportTarget");
                    //tg.active = true;
                   // if (pos.y <= 1.5F  ) pos.y = 1.5F;
                    tg.transform.position = pos;
                }
                
            }
        else
            print("I'm looking at nothing!");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            // print(pos);
            pos.y = pos.y + 2.0F;
            GameObject.Find("Collector").transform.position = pos;
        }

    }
}

