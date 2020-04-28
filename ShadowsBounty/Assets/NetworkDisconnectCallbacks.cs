using Bolt;
using System.Collections;
using System.Collections.Generic;
using UdpKit;
using UnityEngine;
using UnityEngine.SceneManagement;

[BoltGlobalBehaviour]
public class NetworkDisconnectCallbacks : Bolt.GlobalEventListener
{
    public override void Disconnected(BoltConnection connection)
    {
        SceneManager.LoadScene(0);
    }

    public override void SessionCreationFailed(UdpSession session)
    {
        SceneManager.LoadScene(0);
    }

    public override void ConnectFailed(UdpEndPoint endpoint, IProtocolToken token)
    {
        SceneManager.LoadScene(0);
    }

    public override void ConnectRefused(UdpEndPoint endpoint, IProtocolToken token)
    {
        SceneManager.LoadScene(0);
    }
}
