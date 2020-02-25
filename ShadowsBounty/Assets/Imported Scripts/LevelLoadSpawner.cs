using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoadSpawner : MonoBehaviour
{
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject item;
    public Transform orientation;
    public int spawnNumber = 1;
    public float spawnWidth = 1.0f;
    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {
        // Instantiate at position (0, 10, 0) and zero rotation.
        if (spawnNumber > 0) //proof against no-spawn and negative values
        {
            //intitial item
            Instantiate(item, new Vector3(orientation.position.x, orientation.position.y + 2, orientation.position.z), Quaternion.identity);
            float spawninterval = spawnWidth;
            //spawn instances side by side with the initial item at the center
            for (int i = 1; i < spawnNumber; i++)
            {
                if (i % 2 == 0)
                {
                    Instantiate(item, new Vector3(orientation.position.x + spawninterval, orientation.position.y + 2, orientation.position.z), Quaternion.identity);
                    spawninterval += spawnWidth;
                }
                else
                {
                    Instantiate(item, new Vector3(orientation.position.x - spawninterval, orientation.position.y + 2, orientation.position.z), Quaternion.identity);
                    spawninterval += spawnWidth;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
}
