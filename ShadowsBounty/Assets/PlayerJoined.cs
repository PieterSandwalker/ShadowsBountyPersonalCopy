using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoined : Bolt.EntityBehaviour<IPlayerMoveState>
{
    public Camera entityCamera;
    public Canvas HUD;
    public Canvas teamSelect;
    private string username;

    private void Update()
    {
        if(entity.IsOwner && entityCamera.gameObject.activeInHierarchy == false)
        {
            entityCamera.gameObject.SetActive(true);
        }
        if (entity.IsOwner && HUD.gameObject.activeInHierarchy == false)
        {
            HUD.gameObject.SetActive(true);
        }
        if (entity.IsOwner && teamSelect.gameObject.activeInHierarchy == false)
        {
            teamSelect.gameObject.SetActive(true);
        }
    }

    public void closeTeam()
    {
        if (entity.IsOwner)
        {
            teamSelect.gameObject.SetActive(false);
        }
    }
}
