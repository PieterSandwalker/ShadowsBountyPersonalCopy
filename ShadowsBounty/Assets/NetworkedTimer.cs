using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt.Matchmaking;
using TMPro;

public class NetworkedTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CountDown_text;
    bool startTimer = false;
    double timerIncrementValue;
    double startTime;
    [SerializeField] double timer = 60;
    

    void Start()
    {
        if (BoltNetwork.IsServer)
        {
           
            startTime = BoltNetwork.Time;
            startTimer = true;
            
            var session = BoltMatchmaking.CurrentSession;
            if (session != null)
            {
                var photonsession = session as UdpKit.Platform.Photon.PhotonSession;

                photonsession.Properties.Add("StartTime", startTime);
            }
            
        }
        else
        {
            var session = BoltMatchmaking.CurrentSession;
            var photonsession = session as UdpKit.Platform.Photon.PhotonSession;
            startTime = double.Parse(photonsession.Properties["StartTime"].ToString());
            startTimer = true;
        }
    }

    void Update()
    {

        if (!startTimer) return;

        timerIncrementValue = BoltNetwork.Time - startTime;

        if (timerIncrementValue >= timer)
        {
            //Timer Completed
            //Do What Ever You What to Do Here
        }
    }
}
