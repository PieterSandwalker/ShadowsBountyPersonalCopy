using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementMult : Bolt.EntityBehaviour<IPlayerMoveState>
{
    public override void Attached()
    {
        state.SetTransforms(state.PlayerMoveTransform, transform);
    }

   
}
