using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teamManager : Bolt.EntityBehaviour<IPlayerMoveState>
{
    public GameObject UI;
    public GameObject Player;
    public GameObject teamUI;

    [Header("images")]
    public GameObject image1;
    public GameObject image2;
    public GameObject image3;
    public GameObject image4;

    private bool finishSelect;


    // Start is called before the first frame update
    void Start()
    {
        finishSelect = false;
        if (entity.IsOwner)
        {
            UI.SetActive(false);
/*            Player.GetComponent<PlayerMovement2>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;*/
            //teamUI.SetActive(true);
        }
        //disable other UI and movement
        //UI.SetActive(false);
        //Player.GetComponent<PlayerMovement2>().enabled = false;
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()   
    {
        if(UI.activeSelf) {
            Player.GetComponent<PlayerMovement2>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            if (!finishSelect )
            {
                UI.SetActive(false);
            }
        }
        if (finishSelect)
        {
            this.gameObject.GetComponent<teamManager>().enabled = false;
        }
    }

    /*
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UI.SetActive(true);
        Player.GetComponent<PlayerMovement2>().enabled = true;
        if (image1.activeSelf)
        {
            Player.GetComponent<Inventory>().AddScore(100);
        } else if (image2.activeSelf)
        {
            Player.GetComponent<Inventory>().GetKey();
        }
        else if (image3.activeSelf)
        {
            //teleport enabled
            //Player.GetComponent<Inventory>().AddScore(100);
        }
        else if (image4.activeSelf)
        {
            //gun
            //Player.GetComponent<Inventory>().AddScore(100);
        }
    }*/


    public void Close()
    {
        if (entity.IsOwner) {
            teamUI.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            UI.SetActive(true);
            Player.GetComponent<PlayerMovement2>().enabled = true;
            finishSelect = true;
        }

        /*if (image1.activeSelf)
        {
            Player.GetComponent<Inventory>().AddScore(100);
        }
        else if (image2.activeSelf)
        {
            Player.GetComponent<Inventory>().GetKey();
        }
        else if (image3.activeSelf)
        {
            //teleport enabled
            //Player.GetComponent<Inventory>().AddScore(100);
        }
        else if (image4.activeSelf)
        {
            //gun
            //Player.GetComponent<Inventory>().AddScore(100);
        }*/
    }
}
