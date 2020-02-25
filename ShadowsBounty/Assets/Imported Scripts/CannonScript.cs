using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    public GameObject projectile;
    public Transform firePoint;
    public float accuracy = Mathf.PI;
    public float engageRadius = 15.0f;
    public string playerTag = "Player";
    public GameObject[] players;
    public bool readyToFire = true;
    public float fireRate = 5.0f;
    public float power = 1000.0f;

    //values that will be set in the Inspector
    public Transform Target;
    public float RotationSpeed = 0.01f;

    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;

    void resetFire() { readyToFire = true; }

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag(playerTag);
    }
    // Update is called once per frame
    void Update()
    {
        //find the vector pointing from our position to the target
        _direction = (Target.position - transform.position).normalized;
        float angleView = Vector3.Angle(_direction, transform.forward);
        if (readyToFire && angleView <= accuracy)
        {
            readyToFire = false;
            GameObject bullet = Instantiate(projectile, firePoint.position, Quaternion.identity);
            Invoke("resetFire", fireRate);
        }
        //create the rotation we need to be in to look at the target
        _lookRotation = Quaternion.LookRotation(_direction);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);

    }
}
