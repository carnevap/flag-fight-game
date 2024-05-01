using UnityEngine;
using Mirror;

public class CollectAmmo : NetworkBehaviour
{
    public float cooldown = 20f;
    private float timer = 0f;

    void Update()
    {
        if(timer > 0) {
            timer -= Time.deltaTime;
        } else {
            toggleVisibility(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            timer = cooldown;
            other.GetComponent<PlayerAttributes>().replenishAmmo();
            toggleVisibility(false);
        }
    }

    void toggleVisibility(bool toggle) {
        for(int i = 0; i < gameObject.transform.childCount; i++) {  //the children render the object
            gameObject.transform.GetChild(i).gameObject.SetActive(toggle);
        }
    }
}
