using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    public GameObject cam;
    public GameObject cd;

    [Header("TP Data")]
    public GameObject target;


    private bool holdCheck;


    private void Start()
    {
        holdCheck = false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!target.activeSelf) target.SetActive(true);
            Ray ray = cam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            Vector3 temp = hit.point;
            temp.y = temp.y + 5;
            holdCheck = true;
            target.transform.position = temp;
            if( Input.GetMouseButtonDown(1))
            {
                holdCheck = false;
                target.SetActive(true);
            }


        }
        if(Input.GetMouseButtonUp(0) && holdCheck)
        {
            player.transform.position = target.transform.position; // final teleport
            cd.GetComponent<CoolDown>().Use();
        }
            
    }
}

