using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFire : MonoBehaviour
{
    public GameObject projectile;
    public Transform firePoint;
    public GameObject player;
    public bool readyToFire = true;
    public float fireRate = 5.0f;
    public float power = 1000.0f;

    void resetFire() { readyToFire = true; }
    void CheckFire()
    {
        if (readyToFire)
        {
            readyToFire = false;
            GameObject bullet = Instantiate(projectile, firePoint.position, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = player.GetComponent<Rigidbody>().velocity;
            rb.AddForce(transform.forward * power);
            Invoke("resetFire", fireRate);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        if (Input.GetButton("Fire1"))
        {
            CheckFire();
        }
    }
}
