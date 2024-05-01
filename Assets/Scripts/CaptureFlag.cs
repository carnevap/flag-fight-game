using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureFlag : MonoBehaviour
{
    public GameObject enemyFlag;
    public Canvas UI;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            if(other.GetComponent<PlayerAttributes>().team == gameObject.tag && other.GetComponent<PlayerAttributes>().hasFlag) {
                other.GetComponent<PlayerAttributes>().toggleHasFlag(false);
                enemyFlag.GetComponent<CollectFlag>().toggleVisibility(true);
                if(gameObject.tag == "blue") {
                    if(UI.GetComponent<Score>().addPointBlue()) {
                        other.GetComponent<PlayerAttributes>().win(gameObject.tag);
                    }
                } else if(gameObject.tag == "red") {
                    if(UI.GetComponent<Score>().addPointRed()) {
                        other.GetComponent<PlayerAttributes>().win(gameObject.tag);
                    }
                }
            }
        }
    }
}
