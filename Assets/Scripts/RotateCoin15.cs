using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RotateCoin15 : NetworkBehaviour
{
    public int rotateSpeed;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 30 * Time.deltaTime, 0);
    }
}
