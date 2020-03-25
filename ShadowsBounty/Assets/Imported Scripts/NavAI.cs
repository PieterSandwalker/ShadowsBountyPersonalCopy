using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAI : MonoBehaviour
{
    #region SceneData
    public GameObject[] patrolPoints;
    public GameObject[] players;
    #endregion
    #region NavigationTargeting
    public Transform goal;
    public float radiusOfSatisfaction = 5.0f;
    public float pursuitROS = 20.0f;
    public string patrolTag = "PatrolPoint";
    public string playerTag = "Player";
    #endregion
    #region PlayerTargeting
    public float engageRadius = 100.0f;
    public float satisfactoryDetectionLevelVisual = 50.0f;
    public float satisfactoryDetectionLevelAudio = 50.0f;
    private bool playerDetected = false;
    public int index = 0;
    public Vector3 HitLocation;
    public bool ranged = false;
    #endregion
    #region Insistence
    public float food = 0.0f;
    public float water = 0.0f;
    public float rest = 0.0f;
    public bool resting = false;
    public bool eating = false;
    public bool drinking = false;
    #endregion Insistence
    /*
     * On scene load, load all of necessary scene data into AI for use
     */
    void Start()
    {
        patrolPoints = GameObject.FindGameObjectsWithTag(patrolTag);
        goal.position = patrolPoints[index].transform.position;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
        CheckForPlayers();
    }
    /*
     * update the players list to contain the players in the scene
     */
    void CheckForPlayers()
    {
        players = GameObject.FindGameObjectsWithTag(playerTag);
    }
    /*
     * updates each frame. Change what decision is being made based on environment
     */
    void Update()
    {
        CheckForPlayers();
        RunDecisionTree();
    }
    #region DecisionTree
    /*
     * Moderately bootlegged decision tree. this relies on an if tree rather than actual data nodes
     * the returns short-circuit the tree, but currently the player pursuit functions in an unintended fashion
     * leading to guards to bob when near the player
     */
    bool RunDecisionTree()
    {
        if (PlayerNearby()) return true; //scan for players
        if (NeedsMet()) return true; //check insistance values
        if (Patrol()) return true; //do patrol route/behavior
        return false;
    }
    #region PlayerDetection
    /*
     * are there players near me?
     */
    bool PlayerNearby()
    {
        float minimumDistance = float.MaxValue; //use this to find the closest target
        playerDetected = false; //might change to an int
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
                if (!seen && PlayerHeard(player)) //only take heard players into account if none are seen
                {
                    heard = true;
                }
            }
        }
        if (heard || seen) return true; //short circuit if player found
        return false;
    }
    /*
     * Can I hear any players?
     */
    bool PlayerHeard(GameObject player)
    {
        
        PlayerDetectionStats detectionlevel = player.GetComponent<PlayerDetectionStats>();
        if (detectionlevel.AudibleFactor > satisfactoryDetectionLevelAudio)
        {
            if (Investigate(player.transform)) return true;
        }
        return false;
    }
    /*
     * Can I see any players?
     * need to implement angle based targeting
     */
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
                PlayerDetectionStats detectionlevel = player.GetComponent<PlayerDetectionStats>();
                if (detectionlevel.VisibiityFactor > satisfactoryDetectionLevelVisual)
                {
                    playerDetected = true;
                    if (Pursue(player)) return true;
                }
            }
        }
        else
        {
            Debug.DrawRay(transform.position, direction * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
        return false;
    }
    /*
     * Check out a particular area, usually where a player is heard
     */
    bool Investigate(Transform location)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = location.position;
        goal.position = location.position;
        return true;
    }
    /*
     * Based on player movement, predict and pursue
     * Need to change dynamically based on distance
     */
    bool Pursue(GameObject player)
    {
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (distance > pursuitROS)
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            Rigidbody rb = player.GetComponent<Rigidbody>();
            agent.destination = player.transform.position + rb.velocity;
            goal.position = player.transform.position + rb.velocity;
        } else
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = player.transform.position;
            goal.position = player.transform.position;
        }
        return true;
    }
    #endregion PlayerDetection
    #region Needs
    bool NeedsMet()
    {
        food++;
        water++;
        rest++;
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
    #endregion Needs
    #region Patrol
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
        if (index >= patrolPoints.Length) index = 0;
        agent.destination = patrolPoints[index].transform.position;
        goal.position = patrolPoints[index].transform.position;
        return true;
    }
    bool Scan()
    {
        //move raycast
        if (Vector3.Distance(gameObject.transform.position, goal.transform.position) < radiusOfSatisfaction)
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = patrolPoints[0].transform.position;
            goal.position = patrolPoints[0].transform.position;
        }
        return true;
    }
    #endregion Patrol
    #endregion DecisionTree
}
