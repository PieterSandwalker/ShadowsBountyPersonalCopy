using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntervalSpawner : MonoBehaviour
{
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject projectile;
    public Transform orientation;
    public float spawnRate = 1.0f;
    public bool fire = true;
    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {
        // Instantiate at position (0, 10, 0) and zero rotation.
        Instantiate(projectile, new Vector3(orientation.position.x+1, orientation.position.y + 2, orientation.position.z+1), Quaternion.identity);
    }

    void resetFire()
    {
        fire = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (fire)
        {
            Instantiate(projectile, new Vector3(orientation.position.x+1, orientation.position.y + 2, orientation.position.z+1), Quaternion.identity);
            fire = false;
            Invoke("resetFire", spawnRate);
        }
    }
}
