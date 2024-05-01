using UnityEngine;
using UnityEngine.UI;
//using Mirror;

public class WeaponFire : MonoBehaviour
{
    public int damage = 25;
    public int range = 100;
    public int ammo;
    public Camera playerCam;
    public GameObject impactEffect;
    public AudioSource gunshot;
    public AudioSource gunshotHit;
    public Image crosshair;
    public float cooldown = 1f;
    private float timer = 0f;
    private Color32 hitColor = new Color32(255, 0, 0, 170);
    private Color32 defaultColor = new Color32(0, 0, 0, 170);

    void Start() {
        gameObject.GetComponentInParent<PlayerAttributes>().setWeapon(gameObject.name, ammo);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0 && Input.GetButtonDown("Fire1")) {
            if(gameObject.GetComponentInParent<PlayerAttributes>().Shoot()) {
                Shoot();
            }
        }
        if(timer < cooldown - 0.5) {
            crosshair.color = defaultColor;
        }
    }

    void Shoot() {
        if(gameObject.tag == "Bullet") {
            ShootBullet();
        } else if(gameObject.tag == "Shell") {
            ShootShell();
        } else if(gameObject.tag == "Melee") {
            Melee();
        }
    }

    void ShootBullet() {
        gunshot.Play();
        timer = cooldown;
        RaycastHit hit;
        crosshair.color = hitColor;
        if(Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, range)) {
            if(hit.collider.tag == "Player") {
                crosshair.color = hitColor;
                gunshotHit.Play();
                //hit.collider.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponentInParent<NetworkIdentity>().connectionToClient);
                hit.collider.GetComponent<PlayerAttributes>().dealDamage(damage);
                //hit.collider.GetComponent<NetworkIdentity>().RemoveClientAuthority();
            }else if(hit.collider.tag == "Selection"){
                gunshotHit.Play();
                hit.collider.GetComponent<ClassSelection>().assignClass(this.transform.parent.gameObject);
            }
        }
    }

    void ShootShell() {
        //change code for shell spread
        gunshot.Play();
        timer = cooldown;
        RaycastHit[] hit = new RaycastHit[5];
        Vector3 forward;
        for(int i = 0; i < 5; i++) {
            forward = playerCam.transform.forward;
            forward.x += Random.Range(-0.05f, 0.051f);
            forward.y += Random.Range(-0.05f, 0.051f);
            Physics.Raycast(playerCam.transform.position, forward, out hit[i], range);
        }
        bool isHit = false;
        for(int i = 0; i < 5; i++) {
            if(hit[i].collider.tag == "Player") {
                isHit = true;
                //hit.collider.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponentInParent<NetworkIdentity>().connectionToClient);
                hit[i].collider.GetComponent<PlayerAttributes>().dealDamage(damage);
                //hit.collider.GetComponent<NetworkIdentity>().RemoveClientAuthority();
            }
        }
        if(isHit) {
            crosshair.color = hitColor;
            gunshotHit.Play();
        }
    }

    void Melee() {
        //change code for melee attacks
        timer = cooldown;
        RaycastHit hit;
        if(Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, range)) {
            gunshotHit.Play();
            if(hit.collider.tag == "Player") {
                crosshair.color = hitColor;
                //hit.collider.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponentInParent<NetworkIdentity>().connectionToClient);
                hit.collider.GetComponent<PlayerAttributes>().dealDamage(damage);
                //hit.collider.GetComponent<NetworkIdentity>().RemoveClientAuthority();
            }
        } else {
            gunshot.Play();
        }
    }
}
