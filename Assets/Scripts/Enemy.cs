using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform Player;
    public Transform myTransform;
    private Vector3 originalPosition;
    void Start()
    {
        originalPosition = gameObject.transform.position;
    }
    void Update()
    {
        transform.LookAt(Player);
        myTransform.Translate(Vector3.forward * 5 * Time.deltaTime);
    }

    public void resetPosition()
    {
        gameObject.transform.position = originalPosition;
    }
    
}