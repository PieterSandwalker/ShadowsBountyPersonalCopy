using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[BoltGlobalBehaviour("Castle")]
public class ClientCastleCallback : Bolt.GlobalEventListener
{
    public override void Disconnected(BoltConnection connection)
    {
        SceneManager.LoadScene(0);
    }

    public override void SceneLoadLocalDone(string map)
    {
        // player spawns positions
        Vector3[] spawnPositions = new Vector3[4];
        spawnPositions[0] = new Vector3(-118, 12, 113);
        spawnPositions[1] = new Vector3(128, 12, 113);
        spawnPositions[2] = new Vector3(128, 12, -115);
        spawnPositions[3] = new Vector3(-117, 12, -115);

        var random = Random.Range(0, 4);
        // instantiate Player
        BoltNetwork.Instantiate(BoltPrefabs.Player2_0, spawnPositions[random], Quaternion.identity);
    }
}
