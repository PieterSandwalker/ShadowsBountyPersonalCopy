using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAI : MonoBehaviour
{
    public Transform goal;
    public float engageRadius = 200.0f;
    public string patrolTag = "PatrolPoint";
    public string playerTag = "Player";
    public float radiusOfSatisfaction = 5.0f;
    public GameObject[] patrolPoints;
    public GameObject[] players;
    public int index = 0;
    private int numberOfPoints;
    private bool playerDetected = false;
    public Vector3 HitLocation;
    public bool ranged = false;

    void Start()
    {
        patrolPoints = GameObject.FindGameObjectsWithTag(patrolTag);
        numberOfPoints = patrolPoints.Length;
        goal.position = patrolPoints[index].transform.position;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
        CheckForPlayers();
    }
    void CheckForPlayers()
    {
        players = GameObject.FindGameObjectsWithTag(playerTag);
    }
    void Update()
    {
        CheckForPlayers();
        RunDecisionTree();
    }
    bool RunDecisionTree()
    {
        if (PlayerNearby()) return true;
        if (NeedsMet()) return true;
        if (Patrol()) return true;
        return false;
    }
    bool PlayerNearby()
    {
        float minimumDistance = float.MaxValue;
        playerDetected = false;
        bool seen = false;
        bool heard = false;
        foreach (var player in players)
        {
            float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
            if (distance < engageRadius && distance < minimumDistance)
            {
                //make it so the minimum distance doesnt get set if a player isnt detected
                if (PlayerSeen(player))
                {
                    minimumDistance = distance;
                    seen = true;
                }
                if (!seen && PlayerHeard(player))
                {
                    heard = true;
                }
            }
        }
        if (heard || seen) return true;
        return false;
    }
    bool PlayerHeard(GameObject player)
    {
        if (Investigate(player)) return true;
        return false;
    }
    bool PlayerSeen(GameObject player)
    {

        // Bit shift the index of the layer (13) to get a bit mask
        int layerMask = 1 << 13;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, direction * hit.distance, Color.yellow);
            Debug.Log("Did Hit " + hit.collider.gameObject.tag);
            HitLocation = hit.point;
            if (hit.collider.gameObject.tag == "Player")
            {
                playerDetected = true;
                if (Pursue(player)) return true;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, direction * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
        return false;
    }
    bool Investigate(GameObject player)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = player.transform.position;
        goal.position = player.transform.position;
        return false;
    }
    bool Pursue(GameObject player)
    {
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Rigidbody rb = player.GetComponent<Rigidbody>();
        agent.destination = player.transform.position + rb.velocity;
        goal.position = player.transform.position + rb.velocity;
        return true;
    }
    bool NeedsMet()
    {
        if (Food()) return true;
        if (Water()) return true;
        if (Rest()) return true;
        return false;
    }
    bool Food()
    {
        return false;
    }
    bool Water()
    {
        return false;
    }
    bool Rest()
    {
        return false;
    }
    // Update is called once per frame
    bool Patrol()
    {
        if (patrolPoints.Length > 1) return PatrolSet();
        else return Scan();
    }
    bool PatrolSet()
    {
        if (Vector3.Distance(gameObject.transform.position, goal.transform.position) < radiusOfSatisfaction) return Rotate();
        else return Move();
    }
    bool Move()
    {
        return true;
    }
    bool Rotate()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        index++;
        if (index >= numberOfPoints) index = 0;
        agent.destination = patrolPoints[index].transform.position;
        goal.position = patrolPoints[index].transform.position;
        return true;
    }
    bool Scan()
    {
        if (Vector3.Distance(gameObject.transform.position, goal.transform.position) < radiusOfSatisfaction)
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = patrolPoints[0].transform.position;
            goal.position = patrolPoints[0].transform.position;
        }
        return true;
    }

}
