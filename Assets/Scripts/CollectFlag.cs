using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectFlag : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            if(other.GetComponent<PlayerAttributes>().team != gameObject.tag) {
                other.GetComponent<PlayerAttributes>().toggleHasFlag(true);
                toggleVisibility(false);
            }
        }
    }

    public void toggleVisibility(bool toggle) {
        for(int i = 0; i < gameObject.transform.childCount - 1; i++) {  //the children render the object
            gameObject.transform.GetChild(i).gameObject.SetActive(toggle);
        }
    }
}
