using UnityEngine;
using System.Collections;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class NetworkCallbacks : Bolt.GlobalEventListener
{
    public override void SceneLoadLocalDone(string map)
    {
        // put the time cube in a place becuase I don't know how to not make it physical
        var spawnPosition = new Vector3(Random.Range(-16, 16), -50, Random.Range(-16, 16));

        var aiSpawn1 = new Vector3(-16, 18, 16);
        var aiSpawn2 = new Vector3(-114, 18, 17);
        var aiSpawn3 = new Vector3(-40, 11, -91);
        var aiSpawn4 = new Vector3(-56, 1, -3);
        var aiSpawn5 = new Vector3(-98, 9, -2);
        var aiSpawn6 = new Vector3(-100, 9, -73);
        var aiSpawn7 = new Vector3(-61, 1, -67);
        var aiSpawn8 = new Vector3(-125, 1, -57);
        var aiSpawn9 = new Vector3(Random.Range(-16, 16), -50, Random.Range(-16, 16));
        var aiSpawn10 = new Vector3(Random.Range(-16, 16), -50, Random.Range(-16, 16));
        var aiSpawn11 = new Vector3(Random.Range(-16, 16), -50, Random.Range(-16, 16));
        var aiSpawn12 = new Vector3(Random.Range(-16, 16), -50, Random.Range(-16, 16));
        // Spawn the time cube to keep time

        BoltNetwork.Instantiate(BoltPrefabs.TimeCube, spawnPosition, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI1_Variant, aiSpawn1, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI2_Variant, aiSpawn2, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI3_Variant, aiSpawn3, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI4_Variant, aiSpawn4, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI5, aiSpawn5, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI6_Variant, aiSpawn6, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI7_Variant, aiSpawn7, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI8_Variant, aiSpawn8, Quaternion.identity);
    }
}