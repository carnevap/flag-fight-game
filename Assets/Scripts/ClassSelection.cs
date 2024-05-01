using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ClassSelection : NetworkBehaviour
{
    public string choice;

    public void assignClass(GameObject player) {
        if(choice == "ShotGun"){
            player.GetComponent<PlayerAttributes>().maxHealth = 90;
            player.GetComponent<PlayerAttributes>().setWeapon(choice, 20);
            player.GetComponent<MovePlayer>().moveSpeed = 13f;
            player.GetComponent<MovePlayer>().maxJumps = 0;
        }else if(choice == "Sniper"){
            player.GetComponent<PlayerAttributes>().maxHealth = 100;
            player.GetComponent<PlayerAttributes>().setWeapon(choice, 10);
            player.GetComponent<MovePlayer>().moveSpeed = 10f;
            player.GetComponent<MovePlayer>().maxJumps = 0;
        }else if (choice == "MachineGun"){
            player.GetComponent<PlayerAttributes>().maxHealth = 140;
            player.GetComponent<PlayerAttributes>().setWeapon(choice, 50);
            player.GetComponent<MovePlayer>().moveSpeed = 8f;
            player.GetComponent<MovePlayer>().maxJumps = 0;
        }else if(choice == "Knife"){
            player.GetComponent<PlayerAttributes>().maxHealth = 100;
            player.GetComponent<PlayerAttributes>().setWeapon(choice, 10000);
            player.GetComponent<MovePlayer>().moveSpeed = 17f;
            player.GetComponent<MovePlayer>().maxJumps = 1;
        }
        player.GetComponent<PlayerAttributes>().replenishAmmo();
        player.GetComponent<PlayerAttributes>().replenishHealth();
        // player.GetComponent<PlayerAttributes>().RpcSetWeapon();
        player.GetComponent<MovePlayer>().RpcQuickRespawn();
        setWeapon(choice, player);
        setWeaponRpc(choice, player);
    }

    [Command(requiresAuthority = false)]
    public void setWeapon(string choice, GameObject player) {
        setWeaponRpc(choice, player);
    }

    [ClientRpc]
    public void setWeaponRpc(string choice, GameObject player) {
        player.transform.GetChild(4).gameObject.SetActive(false);
        switch(choice){
            case "ShotGun": 
                player.transform.GetChild(2).gameObject.SetActive(true);
                return;
            case "Sniper": 
                player.transform.GetChild(3).gameObject.SetActive(true);
                return;
            case "MachineGun": 
                player.transform.GetChild(4).gameObject.SetActive(true);
                return;
            case "Knife": 
                player.transform.GetChild(5).gameObject.SetActive(true);
                return;
        }
    }
}
