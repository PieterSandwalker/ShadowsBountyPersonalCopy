using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOutScript : Bolt.EntityBehaviour<IPlayerMoveState>
{
    public string respawnTag = "RespawnPoint";

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -20 && entity.IsOwner)
        {
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(respawnTag);
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Vector3 baseVector = spawnPoints[randomIndex].transform.position;
            transform.position = baseVector;
        }
    }
}
