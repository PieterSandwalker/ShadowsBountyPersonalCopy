using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[BoltGlobalBehaviour(BoltNetworkModes.Server, "Castle")]
public class CastleServerCallback : Bolt.GlobalEventListener
{
    public override void SceneLoadLocalDone(string map)
    {
        // put the time cube in a place becuase I don't know how to not make it physical
        var spawnPosition = new Vector3(Random.Range(-16, 16), -50, Random.Range(-16, 16));
        //guard spawns for set1
        var aiSpawn1 = new Vector3(-78, 0, -93);

        //guard spawns for set 2
        var aiSpawn6 = new Vector3(46, 0, 4);

        //guard spawns for set 3
        var aiSpawn9 = new Vector3(-15, 0, -13);

        //guard spawns for set 4
        var aiSpawn12 = new Vector3(41, 0, -79);

        //guard spawns for set 5
        var aiSpawn14 = new Vector3(-15, 0, 30);

        //guard spawns for set 6
        var aiSpawn26 = new Vector3(70, 0, -24);
        //guard spawns for set 7
        var aiSpawn27 = new Vector3(40, 8, -64);
        //guard spawns for set 8
        var aiSpawn28 = new Vector3(68, 8, -79);
        //guard spawns for set 9
        var aiSpawn29 = new Vector3(-15, 9, 28);
        //guard spawns for set 10
        var aiSpawn30 = new Vector3(73, 9, 62);
        //guard spawns for set 11
        var aiSpawn31 = new Vector3(99, 9, 3);
        



        // Spawn the time cube to keep time

        BoltNetwork.Instantiate(BoltPrefabs.TimeCube, spawnPosition, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI1_Variant, aiSpawn1, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI2_Variant, aiSpawn6, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI3_Variant, aiSpawn9, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI4_Variant, aiSpawn12, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI5, aiSpawn14, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI6_Variant, aiSpawn26, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI7_Variant, aiSpawn27, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI8_Variant, aiSpawn28, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI9_Variant, aiSpawn29, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI10_Variant, aiSpawn30, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI11_Variant, aiSpawn31, Quaternion.identity);


    }
}
