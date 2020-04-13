
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoppingmenuCtrl : MonoBehaviour
{

    public GameObject UI;
    public GameObject movementControl;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            UI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            movementControl.GetComponent<PlayerMovement2>().enabled = false;
        }

    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UI.SetActive(true);
        movementControl.GetComponent<PlayerMovement2>().enabled = true;
    }
}
