using UnityEngine;

public class buffManager : MonoBehaviour
{
    
    public GameObject Player;
    public int type = 0;
    float duration;
    float timer;
    bool trigger;
    PlayerDetectionStats detectionStats;
    PlayerMovement2 playerMovement;
   

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        trigger = false;
        duration = 5;
        playerMovement = Player.GetComponent<PlayerMovement2>();
        detectionStats = Player.GetComponent<PlayerDetectionStats>();
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
                    playerMovement.sprintMaxSpeed = 25;
                else if (type == 1)
                {

                    detectionStats.visibilityWeight = 0f;
                    detectionStats.audibilityWeight = 0f;
                }

            }
            else
            {
                timer = 0;
                trigger = false;
                //reset

                playerMovement.sprintMaxSpeed = 15;

                detectionStats.visibilityWeight = 1f;
                detectionStats.audibilityWeight = 1f;
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
