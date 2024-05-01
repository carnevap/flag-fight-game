using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Collections;
using UnityEngine.Networking;

public class PlayerAttributes : NetworkBehaviour
{
    [SyncVar] private int health;
    public int maxHealth = 100;
    private int ammo;
    public int maxAmmo = 25;
    private string weapon;
    public bool hasFlag = false;
    private float moveTimer = 0f;
    private float actionTimer = 0f;
    [SyncVar] private float iOpacity = 0f;
    public float iCooldown = 2f;
    public Text healthUI;
    public Text ammoUI;
    public Text endUI;
    [SyncVar] private string winningTeam;
    private GameObject TeamManager;
    public string team;

    void Start() 
    {
        health = maxHealth;
        ammo = maxAmmo;
        setTeam();
    }

    void Update() {
        if(isLocalPlayer) {
            healthUI.text = "Health: " + health;
            if(winningTeam != null) {
                if(winningTeam == team) {
                    endUI.text = "YOU WIN";
                } else {
                    endUI.text = "YOU LOSE";
                }
                disableWeapon();
            }
            if(weapon != "Knife" && !hasFlag) {
                ammoUI.text = "Ammo: " + ammo;
            } else {
                ammoUI.text = "";
            }
            if(hasFlag) {
                setInvisibility(1f, true);
            }
        }
        if(actionTimer <= 0 && moveTimer <= 0) {
            iOpacity = 0f;
        } else if(actionTimer <= 0) {
            iOpacity = 0.2f;
        }
        if(moveTimer > 0) {
            moveTimer -= Time.deltaTime;
        }
        if(actionTimer > 0) {
            actionTimer -= Time.deltaTime;
        }
        updateVisibility();
    }

    public void setWeapon(string name, int amt) {
        if(name != "Flag") {
            weapon = name;
            maxAmmo = amt;
            ammo = amt;
        }
    }

    public int getWeaponIndex(string name) {
        switch(name) {
            case "ShotGun": return 2;
            case "Sniper": return 3;
            case "MachineGun": return 4;
            case "Knife": return 5;
            case "Flag": return 6;
        }
        return -1;
    }

    [ClientRpc]
    public void toggleHasFlag(bool hasFlag) {
        gameObject.transform.GetChild(getWeaponIndex("Flag")).gameObject.SetActive(hasFlag);
        Debug.Log(getWeaponIndex("Flag"));
        gameObject.transform.GetChild(getWeaponIndex(weapon)).gameObject.SetActive(!hasFlag);
        Debug.Log(getWeaponIndex(weapon));
        this.hasFlag = hasFlag;
    }

    [ClientRpc]
    public void disableWeapon() {
        gameObject.transform.GetChild(getWeaponIndex(weapon)).gameObject.SetActive(false);
    }
    
    [ClientRpc]
    public void enableWeapon() {
        gameObject.transform.GetChild(getWeaponIndex(weapon)).gameObject.SetActive(true);
    }

    public void replenishHealth() {
        health = maxHealth;
        setInvisibility(1f, true);
    }

    public void replenishAmmo() {
        ammo = maxAmmo;
        setInvisibility(1f, true);
    }

    public bool Shoot() {
        if(ammo >= 1) {
            if(weapon != "Knife" && !hasFlag) { // 1000 is used as num for infinite ammo
                ammo--;
            }
            setInvisibility(1f, true);
            return true;
        }
        return false;
    }

    [Command(requiresAuthority = false)]
    public void dealDamage(int damage) {
        if(health - damage > 0) {
            health -= damage;
            setClientInvisibility(1f, true);
        } else {
            health = 0;
            toggleHasFlag(false);
            gameObject.GetComponent<MovePlayer>().RpcRespawn();
        }
    }

    [Command]
    public void setInvisibility(float opacity, bool isAction) {
        if(isAction) {
            actionTimer = iCooldown;
        } else {
            moveTimer = iCooldown;
        }
        if(opacity > iOpacity) {
            iOpacity = opacity;
        }
    }

    [ClientRpc]
    public void setClientInvisibility(float opacity, bool isAction) {
        if(isAction) {
            actionTimer = iCooldown;
        } else {
            moveTimer = iCooldown;
        }
        if(opacity > iOpacity) {
            iOpacity = opacity;
        }
    }

    [Command]
    public void updateVisibility() {
        Color tempCol = gameObject.GetComponent<Renderer>().material.color;
        if(tempCol.a < iOpacity) {
            tempCol.a += Time.deltaTime;
        } else if(tempCol.a > iOpacity) {
            tempCol.a -= Time.deltaTime;
        }
        if(tempCol.a - iOpacity <= 0.04) {   //to avoid constant fluctuation
            tempCol.a = iOpacity;
        }
        setPlayerColor(tempCol);
    }

    [ClientRpc]
    public void setPlayerColor(Color tempCol) {
        gameObject.GetComponent<Renderer>().material.color = tempCol;
    }

    public void setTeam() {
        TeamManager = GameObject.Find("TeamManager");
        TeamManager.GetComponent<TeamManager>().getTeam(gameObject);
        Debug.Log("This is my Team: " + team);
        gameObject.GetComponent<MovePlayer>().getSpawnPoints();
        if(team == "blue") {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        } else {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    public void win(string teamName) {
        winningTeam = teamName;
    }
}
