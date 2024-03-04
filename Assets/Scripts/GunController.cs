using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Rigidbody2D rb;
    public Transform firePoint;
    public float bulletSpeed = 10.0f;

    public bool left = true;
    public int magSize = 25;
    public int bulletsLeft = 25;
    public float reloadTime = 2f; // Time it takes to reload
    private bool reloading = false;
    public GameObject AmmoTextGO;
    public GameObject reloadTextGO;
    private TextMeshProUGUI ammoText; // Assign this in the inspector
    private TextMeshProUGUI reloadingText; // Assign this in the inspector
    public AudioSource audioSource;
    public AudioClip reloadSound;
    public float reloadTextBounceSpeed = 0.1f;
    private Vector3 originalReloadTextSize;

    private GameObject body;

    void Start()
    {
        originalReloadTextSize = reloadTextGO.transform.position;
        ammoText = AmmoTextGO.GetComponent<TextMeshProUGUI>();
        reloadingText = reloadTextGO.GetComponent<TextMeshProUGUI>();
        rb = GetComponent<Rigidbody2D>();
        ammoText.text = $"Ammo: {bulletsLeft}/{magSize}";
        reloadingText.text = "";
        body = transform.parent.gameObject;
    }
    void Update(){
        ammoText.text = $"Ammo: {bulletsLeft}/{magSize}";
        reloadingText.text = reloading? "Reloading!" : "";

        if (Input.GetMouseButtonDown(0) && !PauseMenuController.IsPaused & !body.GetComponent<CharacterController>().cutScene)
        {
            if(bulletsLeft > 0 && !reloading)
            {
                bulletsLeft --;
                Shoot();
            } 
            if(bulletsLeft == 0 && !reloading) {
                reloading = true;
                reloadingText.text = "Reloading!";
                StartCoroutine(ReloadGun());
            }   
            if(bulletsLeft == 0 && reloading) {
                // play click sounds
                audioSource.PlayOneShot(reloadSound);
                // Add LeanTween effect to reloading text
                LeanTween.scale(reloadTextGO, new Vector3(4f, 4f, 4f), reloadTextBounceSpeed)
                    .setEaseOutQuad()
                    .setOnComplete(() =>
                    {
                        LeanTween.scale(reloadTextGO, new Vector3(1.5f, 1.5f, 1.5f), reloadTextBounceSpeed)
                            .setEaseOutQuad();
                    });
            }    
               
        }

        if(Input.GetKey(KeyCode.R) && !reloading){
            reloading = true;
            reloadingText.text = "Reloading!";
            StartCoroutine(ReloadGun());
        } 
    }

    IEnumerator ReloadGun()
    {
        // Set reloading flag to true
        reloading = true;

        // Wait for reload time
        yield return new WaitForSeconds(reloadTime);

        // Refill bullets and reset reloading flag
        bulletsLeft = magSize;
        reloading = false;
    }

    void FixedUpdate()
    {
        // Get the direction from the gun position to the mouse cursor
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        direction.z = 0f; // Make sure the direction is in the 2D plane

        // Calculate the angle to rotate the gun
        float angle = Mathf.Atan2(direction.y, Mathf.Abs(direction.x)) * Mathf.Rad2Deg;

        // Rotate the gun towards the mouse cursor
        handleArmLookDirection(angle);
    }
    private void handleArmLookDirection(float angle)
    {
        if(body.GetComponent<CharacterController>().cutScene){
                Vector3 rotation = new Vector3(transform.rotation.x,0,0);
                transform.rotation = Quaternion.Euler(rotation); 
                left = false;
                return;
        }
        
        if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x){ // if moving right
                // rotate character along y axis to face right
                Vector3 rotation = new Vector3(transform.rotation.x,0,angle);
                transform.rotation = Quaternion.Euler(rotation); 
                left = false;
        } else if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x) { // moving left
                // rotate character along y axis to face left
                
                Vector3 rotation = new Vector3(transform.rotation.x,-180,angle);
                transform.rotation = Quaternion.Euler(rotation); 
                left = true;
        }
    }
    void Shoot()
    {
        GameObject spawnGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Vector2 VectorAngle = new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad),Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad));

        if(left){
            VectorAngle.x = VectorAngle.x * -1;
        }else{
        }
        spawnGO.GetComponent<Rigidbody2D>().velocity = VectorAngle * bulletSpeed;
    }
}
