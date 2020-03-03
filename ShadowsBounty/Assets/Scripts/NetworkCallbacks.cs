using UnityEngine;
using System.Collections;

[BoltGlobalBehaviour]
public class NetworkCallbacks : Bolt.GlobalEventListener
{
    public override void SceneLoadLocalDone(string map)
    {
        // randomize a position
        var spawnPosition = new Vector3(Random.Range(-16, 16), 0, Random.Range(-16, 16));

        // instantiate cube
        BoltNetwork.Instantiate(BoltPrefabs.Player_1, spawnPosition, Quaternion.identity);
    }
}