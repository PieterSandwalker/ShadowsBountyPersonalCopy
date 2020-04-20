using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBombScript : MonoBehaviour
{
    [Header("Player")]
    public GameObject player; // used to buff
    public GameObject cd; // used to count cd\
    public GameObject smokeSample;
    public GameObject buffSystem;
    // Update is called once per frame
    void Update()
    {
        //create a smoke on specific location, which is a instance and auto delet after time
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 a = player.transform.position;
            Quaternion b = new Quaternion(0,0,0,0);
            Instantiate(smokeSample, a,  b);
            cd.GetComponent<CoolDown>().Use();
            buffSystem.GetComponent<buffManager>().Use();
            //set up a buff on player



        }
    }
}
