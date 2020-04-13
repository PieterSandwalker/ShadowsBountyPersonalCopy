using UnityEngine;
using System.Collections;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class NetworkCallbacks : Bolt.GlobalEventListener
{
    public override void SceneLoadLocalDone(string map)
    {
        // put the time cube in a place becuase I don't know how to not make it physical
        var spawnPosition = new Vector3(Random.Range(-16, 16), -50, Random.Range(-16, 16));

        // Spawn the time cube to keep time

        BoltNetwork.Instantiate(BoltPrefabs.TimeCube, spawnPosition, Quaternion.identity);
    }
}