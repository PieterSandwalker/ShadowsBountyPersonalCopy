using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamNetwrokScript : Bolt.EntityBehaviour<IPlayerMoveState>
{
   
    public void setTeam1()
    {
        state.TeamNumber = 1;
    }


    public void setTeam2()
    {
        state.TeamNumber = 2;
    }

    public void setTeam3()
    {
        state.TeamNumber = 3;
    }
    public void setTeam4()
    {
        state.TeamNumber = 4;
    }
}
