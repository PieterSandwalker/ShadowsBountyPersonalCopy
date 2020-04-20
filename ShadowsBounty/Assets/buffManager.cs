using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buffManager : MonoBehaviour
{
    
    public GameObject Player;
    public int type = 0;
    float duration;
    float timer;
    bool trigger;
   

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        trigger = false;
        duration = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger)
        {
            if (timer > 0.0f)
            {
                timer -= Time.deltaTime;
                if (type == 0)
                    Player.GetComponent<PlayerMovement2>().sprintMaxSpeed = 25;
                else if (type == 1)
                {

                    Player.GetComponent<PlayerDetectionStats>().visibilityWeight = 0.2f;
                    Player.GetComponent<PlayerDetectionStats>().audibilityWeight = 0.2f;
                }

            }
            else
            {
                timer = 0;
                trigger = false;
                //reset

                Player.GetComponent<PlayerMovement2>().sprintMaxSpeed = 15;

                Player.GetComponent<PlayerDetectionStats>().visibilityWeight = 1f;
                Player.GetComponent<PlayerDetectionStats>().audibilityWeight = 1f;
            }
        }
    }

    public bool IsReady()
    {
        return trigger;
    }

    public void Use()
    {
        if (!trigger)
        {
            trigger = true;
            timer = duration;
        }
    }
}
