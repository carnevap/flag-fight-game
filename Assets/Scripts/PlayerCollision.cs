using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public CharacterController cControl;
    public Enemy[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnControllerColliderHit(ControllerColliderHit collision){
            Debug.Log(collision.gameObject.name + " hit me");
            if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "DeathCube"){
                cControl.enabled = false;
                //GetComponent<Score>().addDeath();
                GetComponent<Transform>().position = new Vector3(0,3,40);
                foreach(Enemy e in enemies){
                    e.resetPosition();
                }
            }
    }
}
