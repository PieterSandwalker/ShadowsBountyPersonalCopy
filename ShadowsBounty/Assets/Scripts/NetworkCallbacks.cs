using UnityEngine;
using System.Collections;

[BoltGlobalBehaviour(BoltNetworkModes.Server, "MountainCity")]
public class NetworkCallbacks : Bolt.GlobalEventListener
{
    public override void SceneLoadLocalDone(string map)
    {
        // put the time cube in a place becuase I don't know how to not make it physical
        var spawnPosition = new Vector3(Random.Range(-16, 16), -50, Random.Range(-16, 16));
        //guard spawns for set1
        var aiSpawn1 = new Vector3(-76, 19, -308);
        var aiSpawn2 = new Vector3(-86, 19, -218);
        var aiSpawn3 = new Vector3(-53, 19, -267);
        var aiSpawn4 = new Vector3(-147, 19, -288);
        var aiSpawn5 = new Vector3(-114, 19, -252);
        //guard spawns for set 2
        var aiSpawn6 = new Vector3(-235, 19, -347);
        var aiSpawn7 = new Vector3(-198, 19, -334);
        var aiSpawn8 = new Vector3(-162, 31, -313);
        //guard spawns for set 3
        var aiSpawn9 = new Vector3(-315, 27, -275);
        var aiSpawn10 = new Vector3(-316, 27, -252);
        var aiSpawn11 = new Vector3(-369, 31, -244);
        //guard spawns for set 4
        var aiSpawn12 = new Vector3(-428, 31, -128);
        var aiSpawn13 = new Vector3(-404, 31, -123);
        //guard spawns for set 5
        var aiSpawn14 = new Vector3(-269, 19, 45);
        var aiSpawn15 = new Vector3(-227, 19, -33);
        var aiSpawn16 = new Vector3(-127, 19, -92);
        var aiSpawn17 = new Vector3(-347, 19, -35);
        var aiSpawn18 = new Vector3(-406, 19, 19);
        var aiSpawn19 = new Vector3(-340, 19, -133);
        var aiSpawn20 = new Vector3(-380, 19, -167);
        var aiSpawn21 = new Vector3(-397, 19, -194);
        var aiSpawn22 = new Vector3(-216, 19, -227);
        var aiSpawn23 = new Vector3(-138, 19, -192);
        var aiSpawn24 = new Vector3(-311, 19, -218);
        var aiSpawn25 = new Vector3(-192, 19, -129);
        //guard spawns for set 6
        var aiSpawn26 = new Vector3(-399, 39, 36);
        //guard spawns for set 7
        var aiSpawn27 = new Vector3(-281, 39, 80);
        //guard spawns for set 8
        var aiSpawn28 = new Vector3(-254, 39, 127);
        //guard spawns for set 9
        var aiSpawn29 = new Vector3(-429, 39, 50);
        //guard spawns for set 10
        var aiSpawn30 = new Vector3(-409, 85, 161);
        //guard spawns for set 11
        var aiSpawn31 = new Vector3(-331, 79, 130);
        //guard spawns for set 12
        var aiSpawn32 = new Vector3(-310, 19, 159);
        var aiSpawn33 = new Vector3(-298, 19, 99);
        var aiSpawn34 = new Vector3(-313, 19, 127);
        var aiSpawn35 = new Vector3(-264, 19, 127);
        var aiSpawn36 = new Vector3(-416, 19, 48);
        var aiSpawn37 = new Vector3(-382, 19, 102);
        var aiSpawn38 = new Vector3(-436, 19, 118);
        var aiSpawn39 = new Vector3(-360, 19, 87);


        // Spawn the time cube to keep time

        BoltNetwork.Instantiate(BoltPrefabs.TimeCube, spawnPosition, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI1_Variant, aiSpawn1, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI1_Variant, aiSpawn2, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI1_Variant, aiSpawn3, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI1_Variant, aiSpawn4, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI1_Variant, aiSpawn5, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI2_Variant, aiSpawn6, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI2_Variant, aiSpawn7, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI2_Variant, aiSpawn8, Quaternion.identity);
        

        BoltNetwork.Instantiate(BoltPrefabs.NavAI3_Variant, aiSpawn9, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI3_Variant, aiSpawn10, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI3_Variant, aiSpawn11, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI4_Variant, aiSpawn12, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI4_Variant, aiSpawn13, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI5, aiSpawn14, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI5, aiSpawn15, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI5, aiSpawn16, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI5, aiSpawn17, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI5, aiSpawn18, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI5, aiSpawn19, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI5, aiSpawn20, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI5, aiSpawn21, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI5, aiSpawn22, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI5, aiSpawn23, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI5, aiSpawn24, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI5, aiSpawn25, Quaternion.identity);


        BoltNetwork.Instantiate(BoltPrefabs.NavAI6_Variant, aiSpawn26, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI7_Variant, aiSpawn27, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI8_Variant, aiSpawn28, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI9_Variant, aiSpawn29, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI10_Variant, aiSpawn30, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI11_Variant, aiSpawn31, Quaternion.identity);

        BoltNetwork.Instantiate(BoltPrefabs.NavAI12_Variant, aiSpawn32, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI12_Variant, aiSpawn33, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI12_Variant, aiSpawn34, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI12_Variant, aiSpawn35, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI12_Variant, aiSpawn36, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI12_Variant, aiSpawn37, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI12_Variant, aiSpawn38, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.NavAI12_Variant, aiSpawn39, Quaternion.identity);
    }
}