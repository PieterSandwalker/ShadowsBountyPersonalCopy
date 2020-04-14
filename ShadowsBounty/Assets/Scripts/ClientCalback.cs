using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[BoltGlobalBehaviour("MountainCity")]
public class ClientCalback : Bolt.GlobalEventListener
{

    public override void SceneLoadLocalDone(string map)
    {
        // player spawns positions
        Vector3[] spawnPositions = new Vector3[10];
        spawnPositions[0] = new Vector3(-219, 21, -69);
        spawnPositions[1] = new Vector3(-118, 20, -300);
        spawnPositions[2] = new Vector3(-71, 20, -276);
        spawnPositions[3] = new Vector3(-199, 19, -326);
        spawnPositions[4] = new Vector3(-269, 19, -326);
        spawnPositions[5] = new Vector3(-238, 20, -265);
        spawnPositions[6] = new Vector3(-214, 20, -239);
        spawnPositions[7] = new Vector3(-63, 20, -196);
        spawnPositions[8] = new Vector3(-166, 19, -110);
        spawnPositions[9] = new Vector3(-238, 20, -78);
        var random = Random.Range(0, 10);
        // instantiate Player
        BoltNetwork.Instantiate(BoltPrefabs.Player2_0, spawnPositions[random], Quaternion.identity);
  
    }
}
