using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAI : MonoBehaviour
{
    #region SceneData
    public GameObject[] patrolPoints;
    public GameObject[] waterLocations;
    public GameObject[] foodLocations;
    public GameObject[] players;
    #endregion
    #region NavigationTargeting
    public Transform goal;
    public float radiusOfSatisfaction = 5.0f;
    public float patrolSpeed = 5.0f;
    public float investigateSpeed = 7.0f;
    public float pursuitSpeed = 10.0f;
    public float pursuitROS = 20.0f;
    public string patrolTag = "PatrolPoint";
    public string waterTag = "WaterStation";
    public string foodTag = "MessHall";
    public string playerTag = "Player";
    #endregion
    #region PlayerTargeting
    public float engageRadius = 100.0f;
    public float satisfactoryDetectionLevelVisual = 10.0f;
    public float satisfactoryDetectionLevelAudio = 10.0f;
    private bool playerDetected = false;
    public float visionAngle = 60.0f; //degrees
    public float distanceWeightRatio = 30.0f; 
    public int index = 0;
    public Vector3 HitLocation;
    public bool ranged = false;
    #endregion
    #region Insistence
    public float food = 0.0f;
    public float water = 0.0f;
    public float rest = 0.0f;
    public float patrol = 0.0f;
    public float foodRecoverySpeed = 2.0f;
    public float waterRecoverySpeed = 2.0f;
    public float restRecoverySpeed = 3.0f;
    public float foodRequirement = 500.0f;
    public float waterRequirement = 300.0f;
    public float restRequirement = 600.0f;
    public float patrolRequirement = 300.0f;
    public bool resting = false;
    public bool eating = false;
    public bool drinking = false;
    #endregion Insistence
    /*
     * On scene load, load all of necessary scene data into AI for use
     */
    void Start()
    {
        food = Random.value * 100;
        water = Random.value * 100;
        rest = Random.value * 100;
        patrolPoints = GameObject.FindGameObjectsWithTag(patrolTag);
        waterLocations = GameObject.FindGameObjectsWithTag(waterTag);
        foodLocations = GameObject.FindGameObjectsWithTag(foodTag);
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
     * the returns short-circuit the tree
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
                if (PlayerSeen(player, distance))
                {
                    minimumDistance = distance;
                    seen = true;
                }
                if (!seen && PlayerHeard(player, distance)) //only take heard players into account if none are seen
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
    bool PlayerHeard(GameObject player, float distance)
    {
        distance /= distanceWeightRatio;
        PlayerDetectionStats detectionlevel = player.GetComponent<PlayerDetectionStats>();
        if (detectionlevel.AudibleFactor/distance > satisfactoryDetectionLevelAudio)
        {
            if (Investigate(player.transform)) return true;
        }
        return false;
    }
    /*
     * Can I see any players?
     * need to implement angle based targeting
     */
    bool PlayerSeen(GameObject player, float distance)
    {

        // Bit shift the index of the layer (13) to get a bit mask
        int layerMask = 1 << 13;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        // Does the ray intersect any objects excluding the player layer
        if (Vector3.Angle(direction, transform.forward) <= visionAngle && Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, direction * hit.distance, Color.yellow);
            //Debug.Log("Did Hit " + hit.collider.gameObject.tag);
            HitLocation = hit.point;
            if (hit.collider.gameObject.tag == "Player")
            {
                distance /= distanceWeightRatio;
                PlayerDetectionStats detectionlevel = player.GetComponent<PlayerDetectionStats>();
                Debug.Log("DetectionValue: " + detectionlevel.VisibilityFactor / distance);
                if (detectionlevel.VisibilityFactor / distance > satisfactoryDetectionLevelVisual)
                {
                    playerDetected = true;
                    if (Pursue(player)) return true;
                }
            }
        }
        else
        {
            Debug.DrawRay(transform.position, direction * distance, Color.white);
            //Debug.Log("Did not Hit");
        }
        return false;
    }
    /*
     * Check out a particular area, usually where a player is heard
     */
    bool Investigate(Transform location)
    {
        patrol++; //add to need to keep watch.
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.speed = investigateSpeed;
        agent.destination = location.position;
        goal.position = location.position;
        return true;
    }
    /*
     * Based on player movement, predict and pursue
     * Need to change dynamically based on distance
     * currently does the fixed distance approach
     */
    bool Pursue(GameObject player)
    {
        rest++; //increment this to exemplify tiring out after a chase
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.speed = pursuitSpeed;
        if (distance > pursuitROS)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            agent.destination = player.transform.position + rb.velocity;
            goal.position = player.transform.position + rb.velocity;
        } else
        {
            agent.destination = player.transform.position;
            goal.position = player.transform.position;
        }
        return true;
    }
    #endregion PlayerDetection
    #region Needs
    bool NeedsMet()
    {
        food += Random.value;
        water += Random.value;
        rest += Random.value;
        if (Rest()) return true;
        if (Water()) return true;
        if (Food()) return true;
        return false;
    }
    bool Food()
    {
        if (foodLocations.Length < 2) //remove this behavior for maps that can't handle it
            return false;
        if (eating)
        {
            food--; // undo insistence increase
            food -= foodRecoverySpeed; //proceed with recovery
            if (food <= 0.0f)
                eating = false; //am I full?
            else
                return true;
        } 
        else if (food >= foodRequirement)
        {
            float minimumDistance = float.MaxValue; //use this to find the closest target
            foreach (var location in foodLocations)
            {
                float distance = Vector3.Distance(gameObject.transform.position, location.transform.position);
                if (distance < minimumDistance)
                {
                    minimumDistance = distance;
                    NavMeshAgent agent = GetComponent<NavMeshAgent>();
                    agent.speed = patrolSpeed / 10 * food; //honestly is not needed, but I thought it would be amusing for a guard to move faster based on
                                                            //how hungry he is
                    agent.destination = location.transform.position;
                    goal.position = location.transform.position;
                }
            }

            if (Vector3.Distance(gameObject.transform.position, goal.transform.position) < radiusOfSatisfaction)
            {
                eating = true;
            }
            return true;
        }
        return false;
    }
    bool Water()
    {
        if (waterLocations.Length < 2) //remove this behavior for maps that can't handle it
            return false;
        if (drinking)
        {
            water--; // undo insistence increase
            water -= waterRecoverySpeed; //recover
            if (water <= 0.0f)
                drinking = false; //am I full?
            else
                return true;
        } 
        else if (water >= waterRequirement)
        {
            float minimumDistance = float.MaxValue; //use this to find the closest target
            foreach (var location in waterLocations)
            {
                float distance = Vector3.Distance(gameObject.transform.position, location.transform.position);
                if (distance < minimumDistance)
                {
                    minimumDistance = distance;
                    NavMeshAgent agent = GetComponent<NavMeshAgent>();
                    agent.speed = patrolSpeed / 10 * water; //honestly is not needed, but I thought it would be amusing for a guard to move faster based on
                                                           //how thirsty he is
                    agent.destination = location.transform.position;
                    goal.position = location.transform.position;
                }
            }

            if (Vector3.Distance(gameObject.transform.position, goal.transform.position) < radiusOfSatisfaction)
            {
                drinking = true;
            }
            return true;
        }
        return false;
    }
    bool Rest()
    {
        if (resting)
        {
            rest--; // undo insistence increase
            rest -= restRecoverySpeed;
            if (rest <= 0.0f)
                resting = false; //am I relaxed I guess? well rested?
            else
                return true;
        } 
        else if (rest >= restRequirement)
        {

            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = gameObject.transform.position;
            goal.position = gameObject.transform.position; //stop and take a breather
            resting = true;
            return true;
        }
        return false;
    }
    #endregion Needs
    #region Patrol
    // Update is called once per frame
    bool Patrol()
    {
        PatrolInsistence();
        if (patrolPoints.Length > 1) return PatrolSet();
        else return Scan();
    }
    bool PatrolInsistence()
    {
        bool targetIsPatrol = false;
        foreach (var location in patrolPoints)
        {
            float distance = Vector3.Distance(goal.position, location.transform.position);
            if (distance < 3.0f)
            {
                targetIsPatrol = true;
                break;
            }
        }
        if (!targetIsPatrol)
        {
            patrol++; //is it worth my time investigating this?
            if (patrol >= patrolRequirement)
            {
                patrol = 0;
                food = 0; //this is a current work around for guards who do not have access to the ground level to get to food and water
                water = 0;
                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                agent.speed = patrolSpeed;
                if (index >= patrolPoints.Length) index = 0;
                agent.destination = patrolPoints[index].transform.position;
                goal.position = patrolPoints[index].transform.position;
            }
            return true;
        } else
        {
            patrol = 0;
        }
        return false;
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
        agent.speed = patrolSpeed;
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
