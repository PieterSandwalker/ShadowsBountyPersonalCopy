using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovementMult : Bolt.EntityBehaviour<IAIState>
{
   
    public override void Attached()
    {
        state.SetTransforms(state.AIStateTransform, transform);
    }
}
