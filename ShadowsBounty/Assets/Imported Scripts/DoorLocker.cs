using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLocker : MonoBehaviour
{
    public string doorTag = "Door";
    public GameObject[] doors;
    public int numberOfDoors;
    public bool randomNumberLocked = false;
    public int desiredLocked = 1;
    public int locked = 0;

    void Start()
    {
        doors = GameObject.FindGameObjectsWithTag(doorTag);
        numberOfDoors = doors.Length;
        //always lock at least one door
        int initialSelection = Random.Range(0, numberOfDoors);
        DoorStats initial = doors[initialSelection].GetComponent(typeof(DoorStats)) as DoorStats;
        initial.locked = true;
        locked++;

        if (randomNumberLocked)
        {
            //randomly lock as you go
            foreach (var door in doors)
            {
                DoorStats status = door.GetComponent(typeof(DoorStats)) as DoorStats;
                if (Random.Range(0, 100) % 2 == 0 && !status.locked)
                {
                    status.locked = true;
                    locked++; //track how many are locked
                }
            }
        } else
        {
            while (locked < desiredLocked)
            {
                int selected = Random.Range(0, numberOfDoors);
                DoorStats doorToLock = doors[selected].GetComponent(typeof(DoorStats)) as DoorStats;
                if (!doorToLock.locked)
                {
                    doorToLock.locked = true;
                    locked++;
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
