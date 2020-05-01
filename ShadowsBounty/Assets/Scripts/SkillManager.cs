using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    Vector3 pos;
    // Update is called once per frame

    Camera cam;

    bool useOnce;
    bool ready;

    void Start()
    {
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        useOnce = false;
        ready = false;
        //cam = Camera.current;
    }
    void Update()   
    {
        if (!useOnce)
        {
            if (Input.GetKeyDown(KeyCode.E) || ready)
            {
                ready = true;
                //   RaycastHit hit;
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    print("I'm looking at " + hit.transform.name);
                    if (hit.transform.name == "TeleportTarget")
                    {
                    }
                    else
                    {
                        // Debug.Log("here");
                        pos = hit.point;
                        GameObject tg = GameObject.Find("TeleportTarget");
                        tg.transform.position = pos;
                    }
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        print(1);
                        pos.y = pos.y + 2.0F;
                        GameObject.Find("Player2.0").transform.position = pos;
                        useOnce = true;
                        GameObject.Find("TeleportTarget").transform.position = new Vector3(0, -5, 0);
                        GameObject.Find("skillButtom").SetActive(false);
                    }
                }
                else
                {
                    print("I'm looking at nothing!");
                }

                /* else if (Input.GetKeyDown(KeyCode.R))
                {
                    GameObject.Find("TeleportTarget").transform.position = new Vector3(0,-5,0);
                }*/
            }
            
        }
    }
}
