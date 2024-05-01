using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Mirror;

public class MovePlayer : NetworkBehaviour
{
    public Camera cam;
    public Transform camTransform;
    public CharacterController cControl;    //stores player's character controller
    public Transform gunTransform;
    public float moveSpeed; //amount of force used for player movement
    public float jumpSpeed = 1.2f; //amount of force used for jumping
    public int maxJumps; //amount of jumps player can do above 1
    private int jumps; //current amount of jumps player has
    public float sensitivity = 10f;
    private float gravity = 0.1f;

    private float horiz = 0f;

    private float vert = 0f;

    private float tempY;

    private Vector2 turn;

    float turnSmooth;

    private GameObject[] spawnPoints;

    void Start() {
        maxJumps--;
        Cursor.lockState = CursorLockMode.Locked;
        if(!isLocalPlayer) {
            cam.gameObject.SetActive(false);
        }
        jumps = maxJumps;
        cControl.enabled = false;
        gameObject.transform.position = new Vector3(11.16191f, -23f, -209.7f);
        cControl.enabled = true;
    }

    public void getSpawnPoints(){
        if(spawnPoints == null){
            if(gameObject.GetComponent<PlayerAttributes>().team == "red") {
                spawnPoints = GameObject.FindGameObjectsWithTag("RedSpawn");
            }else{
                spawnPoints = GameObject.FindGameObjectsWithTag("BlueSpawn");
            }
        }
    }

    void FixedUpdate() {
        HandleMovement();
    }

    void Update() {
       handleInput();
       cameraRotation();
    }

    private void LateUpdate(){
        if (cControl.enabled == false) {
            cControl.enabled = true;
        }
    }

    void HandleMovement() {
        if(isLocalPlayer){
            Vector3 move = new Vector3(horiz, 0f, vert);
            if(!cControl.isGrounded) {
                tempY -= gravity;       //apply gravity
            }
            move.y = tempY;
            cControl.Move(transform.rotation * move * moveSpeed * Time.deltaTime);
        }
    }
    
    void cameraRotation(){
        turn.x += Input.GetAxis("Mouse X") * sensitivity;
        turn.y += Input.GetAxis("Mouse Y") * sensitivity;
        turn.y = Mathf.Clamp(turn.y, -90f, 90f);
        transform.localRotation = Quaternion.Euler(0, turn.x, 0);
        camTransform.localRotation = Quaternion.Euler(-turn.y, 0, 0);
        gunTransform.localRotation = Quaternion.Euler(-turn.y, 0, 0);
    }
    void handleInput(){
        horiz = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");
        if(Input.GetButtonDown("Jump") && (jumps > 0 || cControl.isGrounded)){
            tempY = jumpSpeed;
            jumps--;
        }
        if(cControl.isGrounded) {
            jumps = maxJumps;       //reset jumps after touching ground
        }
        if(!cControl.isGrounded || horiz != 0 || vert != 0) {   //if player is moving, set visible
            gameObject.GetComponent<PlayerAttributes>().setInvisibility(0.2f, false);
        }
    }

    public void RpcQuickRespawn() {
        cControl.enabled = false;
        gameObject.transform.position = randomSpawn();
        cControl.enabled = true;
        gameObject.GetComponent<PlayerAttributes>().setInvisibility(0f, false);
        gameObject.GetComponent<PlayerAttributes>().replenishHealth();
        gameObject.GetComponent<PlayerAttributes>().replenishAmmo();
    }

    public void RpcRespawn(){
        cControl.enabled = false;
        gameObject.transform.position = randomSpawn();
        cControl.enabled = true;
        gameObject.GetComponent<PlayerAttributes>().setInvisibility(0f, false);
        gameObject.GetComponent<PlayerAttributes>().replenishHealth();
        gameObject.GetComponent<PlayerAttributes>().replenishAmmo();
    }

    private Vector3 randomSpawn() {
        int index =  Random.Range(0, spawnPoints.Length);
        return spawnPoints[index].GetComponent<Transform>().position;
    }
}
