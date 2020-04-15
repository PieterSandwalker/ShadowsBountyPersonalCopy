using UnityEngine;

public class QuitButtonTemp : Bolt.EntityBehaviour<IPlayerMoveState>
{
    void Update()
    {
        if (Input.GetKey("escape") && entity.IsOwner)
        {
            Application.Quit();
        }
    }
}
