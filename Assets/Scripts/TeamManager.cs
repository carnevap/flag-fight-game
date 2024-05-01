using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TeamManager : NetworkBehaviour
{
    private string nextTeam = "blue";

    public void getTeam(GameObject player)
    {
        Debug.Log("Get Team called: " + nextTeam);
        if(player.GetComponent<PlayerAttributes>().team == ""){
            player.GetComponent<PlayerAttributes>().team = nextTeam;
        }
        changeTeam();
    }
    private void changeTeam()
    {
        if(nextTeam == "blue")
        {
            nextTeam = "red";
        }
        else
        {
            nextTeam = "blue";
        }
        Debug.Log("Next Team: " + nextTeam);
    }
}
