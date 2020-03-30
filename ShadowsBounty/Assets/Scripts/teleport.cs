using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    public GameObject cam;
    public GameObject cd;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {           
            Ray ray = cam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            player.transform.position = hit.point;
            cd.GetComponent<CoolDown>().Use();
        }
            
    }
}

