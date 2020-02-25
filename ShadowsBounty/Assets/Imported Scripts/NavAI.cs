using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAI : MonoBehaviour
{
    public Transform goal;
    public float engageRadius = 15.0f;
    public string patrolTag = "PatrolPoint";
    public string playerTag = "Player";
    public GameObject[] objs;
    public GameObject[] players;
    public int index = 0;
    private int numberOfPoints;
    private bool playerDetected = false;

    void Start()
    {
        objs = GameObject.FindGameObjectsWithTag(patrolTag);
        players = GameObject.FindGameObjectsWithTag(playerTag);
        numberOfPoints = objs.Length;
        goal = objs[index].transform;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }

    // Update is called once per frame
    void detectBorderPointProximity()
    {
        if (Vector3.Distance(gameObject.transform.position, goal.transform.position) < 5.0f)
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            index++;
            if (index >= numberOfPoints) index = 0;
            agent.destination = objs[index].transform.position;
            goal = objs[index].transform;
        }
    }
    void Patrol()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        goal = objs[index].transform;
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
                playerDetected = true;
                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                agent.destination = player.transform.position;
                goal = player.transform;
            }
        }
    }
    void Update()
    {
        detectPlayer();
        if (!playerDetected)
        {
            Patrol();
            detectBorderPointProximity();
        }
    }
}
