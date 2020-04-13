using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoined : Bolt.EntityBehaviour<IPlayerMoveState>
{
    public Camera entityCamera;
    public Canvas HUD;
    public Canvas teamSelect;
    private string username;
    private bool teamActive = false;

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
        if (entity.IsOwner && !teamActive && teamSelect.gameObject.activeInHierarchy == false)
        {
            Debug.Log(1);
            teamSelect.gameObject.SetActive(true);
            teamActive = true;
        }
    }
}
