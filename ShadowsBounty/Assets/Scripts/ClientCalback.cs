using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[BoltGlobalBehaviour]
public class ClientCalback : Bolt.GlobalEventListener
{

    public override void SceneLoadLocalDone(string map)
    {
        // randomize a position
        var spawnPosition = new Vector3(Random.Range(-16, 16), 0, Random.Range(-16, 16));
 
        // instantiate Player
        BoltNetwork.Instantiate(BoltPrefabs.Player2_0, spawnPosition, Quaternion.identity);
  
    }
}
