using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedBooster : MonoBehaviour
{
    [Header("Player")]
    public GameObject player; // used to buff
    public GameObject cd; // used to count cd\
    public float duration;
    public float speedScale;


    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            cd.GetComponent<CoolDown>().Use();



        }
    }

}
