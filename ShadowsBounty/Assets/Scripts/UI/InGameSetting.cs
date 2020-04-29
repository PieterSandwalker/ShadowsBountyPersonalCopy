using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameSetting : Bolt.GlobalEventListener
{

    public void exitScript()
    {
        if (BoltNetwork.IsClient)
        {
            BoltNetwork.Server.Disconnect();
        } else
        {
            foreach(BoltConnection connection in BoltNetwork.Clients)
            {
                connection.Disconnect();
            }
            BoltLauncher.Shutdown();
        }
        Cursor.lockState = CursorLockMode.None;
        //Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene(0);

    }
}
