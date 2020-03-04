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
    public GameObject[] objs;
    public GameObject[] players;
    public int index = 0;
    private int numberOfPoints;
    private bool playerDetected = false;
    public Vector3 HitLocation;

    void Start()
    {
        objs = GameObject.FindGameObjectsWithTag(patrolTag);
        players = GameObject.FindGameObjectsWithTag(playerTag);
        numberOfPoints = objs.Length;
        goal.position = objs[index].transform.position;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }

    // Update is called once per frame
    void detectBorderPointProximity()
    {
        if (Vector3.Distance(gameObject.transform.position, goal.transform.position) < radiusOfSatisfaction)
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            index++;
            if (index >= numberOfPoints) index = 0;
            agent.destination = objs[index].transform.position;
            goal.position = objs[index].transform.position;
            Patrol();
        }
    }
    void Patrol()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        goal.position = objs[index].transform.position;
        agent.destination = goal.position;
    }
    void detectPlayer()
    {
        float minimumDistance = float.MaxValue;
        playerDetected = false;
        foreach (var player in players) {
            float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
            if (distance < engageRadius && distance < minimumDistance)
            {
                minimumDistance = distance;


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

                        minimumDistance = distance;
                        playerDetected = true;
                        NavMeshAgent agent = GetComponent<NavMeshAgent>();
                        agent.destination = player.transform.position;
                        goal.position = player.transform.position;
                    }
                }
                else
                {
                    Debug.DrawRay(transform.position, direction * 1000, Color.white);
                    Debug.Log("Did not Hit");
                }
            }
        }
    }
    void Update()
    {
        detectPlayer();
        if (!playerDetected)
        {
            //Patrol();
            detectBorderPointProximity();
        }
    }
}
