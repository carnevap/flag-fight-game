using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPong : MonoBehaviour
{

    // Update is called once per frame
    
    public Vector3 pos1 = new Vector3(-24.75f,0f,20.25f);
    public Vector3 pos2 = new Vector3(-10.5f,0f,20.25f);
    public float speed = 0.75f;
 
     void Update() {
        transform.position = Vector3.Lerp (pos1, pos2, Mathf.PingPong(Time.time*speed, 1.0f));
     }
}
